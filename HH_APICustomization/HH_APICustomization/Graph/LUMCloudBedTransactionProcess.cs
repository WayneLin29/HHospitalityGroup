﻿using HH_APICustomization.APIHelper;
using HH_APICustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.GL;
using PX.Data.BQL;
using PX.Objects.CM;

namespace HH_APICustomization.Graph
{
    public class LUMCloudBedTransactionProcess : PXGraph<LUMCloudBedTransactionProcess>
    {
        public PXSave<TransactionFilter> Save;
        public PXCancel<TransactionFilter> Cancel;

        public PXFilter<TransactionFilter> TransacionFilter;
        public PXFilter<ReservationFilter> ReservationFilter;
        public PXFilteredProcessing<LUMCloudBedTransactions, TransactionFilter,
                              Where<LUMCloudBedTransactions.isImported, Equal<Current<TransactionFilter.isImported>>>> Transaction;

        public SelectFrom<LUMCloudBedTransactions>.View CurrentTransaction;

        public SelectFrom<LUMCloudBedReservations>.View Reservations;

        public LUMCloudBedTransactionProcess()
        {
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.isImported>(Transaction.Cache, null, true);

            Transaction.SetProcessDelegate(delegate (List<LUMCloudBedTransactions> list)
            {
                GoProcessing(list);
            });
        }

        #region Action

        public PXAction<TransactionFilter> importTransactionData;
        [PXUIField(DisplayName = "Import Transaction Data", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXProcessButton]
        public virtual void ImportTransactionData()
        {
            PXLongOperation.StartOperation(this, () =>
            {
                GetCloudBedTransactionData(this);
            });
        }

        public PXAction<TransactionFilter> importReservationData;
        [PXUIField(DisplayName = "Import Reservation Data", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXProcessButton]
        public virtual void ImportReservationData()
        {
            PXLongOperation.StartOperation(this, () =>
            {
                GetCloudBedReservationData(this);
            });
        }

        #endregion

        #region Method

        public static void GoProcessing(List<LUMCloudBedTransactions> selectedData)
        {
            var baseGraph = PXGraph.CreateInstance<LUMCloudBedTransactionProcess>();
            baseGraph.CreateJournalTransaction(baseGraph, selectedData);
        }

        public virtual void CreateJournalTransaction(LUMCloudBedTransactionProcess baseGraph, List<LUMCloudBedTransactions> selectedData)
        {
            var groupData = selectedData.Where(x => !(x.IsImported ?? false)).GroupBy(x => new { x.TransactionDateTime.Value.Date, x.PropertyID, x.Currency });
            var cloudbedProperty = SelectFrom<LUMCloudBedPreference>.View.Select(baseGraph).RowCast<LUMCloudBedPreference>();
            var reservationData = baseGraph.Reservations.Select().RowCast<LUMCloudBedReservations>();
            var AcctMapAData = SelectFrom<LUMCloudBedAccountMapping>.View.Select(baseGraph).RowCast<LUMCloudBedAccountMapping>();
            var ledgerInfo = SelectFrom<Ledger>.Where<Ledger.ledgerCD.IsEqual<P.AsString>>.View.Select(baseGraph, "ACTUAL").TopFirst;
            foreach (var cloudBedGroupRow in groupData)
            {
                var glBatchNbr = string.Empty;
                string errorMsg = string.Empty;              // 整筆資料錯誤訊息
                var mapCloudbedProerty = cloudbedProperty.FirstOrDefault(x => x.CloudBedPropertyID == cloudBedGroupRow.Key.PropertyID);
                var lineErrorDic = new Dictionary<string, string>();    // 每筆資料的錯誤訊息
                var lineNbrDic = new Dictionary<string, int?>();        // 每筆資料所對應到的GL Line
                LUMCloudBedAccountMapping winnerAcctMapInfo = null;     // 對應到的Account Mapping物件

                // 整筆資料錯誤Handler
                try
                {
                    #region Header
                    var glGraph = PXGraph.CreateInstance<JournalEntry>();
                    var doc = glGraph.BatchModule.Cache.CreateInstance() as Batch;
                    doc.Module = "GL";
                    doc = glGraph.BatchModule.Cache.Insert(doc) as Batch;
                    doc.DateEntered = cloudBedGroupRow.Key.Date;
                    doc.LedgerID = ledgerInfo?.LedgerID;
                    doc.BranchID = mapCloudbedProerty?.BranchID;
                    doc.Description = $"CloudBed Transaction {cloudBedGroupRow.Key.Date.ToString("yyyy-MM-dd")}";
                    doc.CuryID = cloudBedGroupRow.Key.Currency;
                    glGraph.BatchModule.Cache.Update(doc);
                    #endregion

                    #region Details
                    foreach (var row in cloudBedGroupRow)
                    {
                        errorMsg = string.Empty;
                        // Set CurrentItem
                        PXLongOperation.SetCurrentItem(row);
                        try
                        {
                            var mapReservation = reservationData.FirstOrDefault(x => x.PropertyID == row.PropertyID && x.ReservationID == row.ReservationID);
                            if (mapReservation == null && long.TryParse(row.ReservationID, out _))
                                throw new PXException("Can not Mapping Reservation Data!!");

                            #region RuleA
                            int maxScore = 0;                                 // 最高分
                            var sameScoureList = new List<int>();             // 相同分數清單
                            var target = new LUMCloudBedAccountMapping()
                            {
                                CloudBedPropertyID = row?.PropertyID,
                                TransCategory = row?.Category,
                                HouseAccount = row?.HouseAccountID?.ToString(),
                                TransactionCode = row?.TransactionCode,
                                Description = row?.Description,
                                Source = mapReservation?.Source
                            };  // 比對目標
                            winnerAcctMapInfo = null;// 最高分物件
                                                     // Compare Account Mapping
                            foreach (var acctMapRow in AcctMapAData)
                            {
                                // 只比對相同PropertyID
                                if (acctMapRow.CloudBedPropertyID != target.CloudBedPropertyID)
                                    continue;
                                var matchScore = CompareProps(target, acctMapRow);
                                if (matchScore > maxScore)
                                {
                                    sameScoureList.Clear();
                                    winnerAcctMapInfo = acctMapRow;
                                    maxScore = matchScore;
                                    sameScoureList.Add(acctMapRow.SequenceID.Value);
                                }
                                else if (matchScore == maxScore)
                                    sameScoureList.Add(acctMapRow.SequenceID.Value);
                            }
                            if (winnerAcctMapInfo == null)
                                throw new PXException(" No Account Mapping Found. Please maintain the combination in Preference.");
                            if (sameScoureList.Count > 1)
                                throw new PXException($" No Account Mapping Found. Please maintain the combination in Preference ID: {string.Join(",", sameScoureList)}.");
                            var line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
                            line.AccountID = winnerAcctMapInfo?.AccountID;
                            line.SubID = winnerAcctMapInfo?.SubAccountID;
                            line.RefNbr = string.IsNullOrEmpty(row?.ReservationID) ? row.HouseAccountID.ToString() : row.ReservationID;
                            if (line.RefNbr.Length > 15)
                                line.RefNbr = line?.RefNbr.Substring(0, 15);
                            line.Qty = row?.Quantity;
                            line.TranDesc = $"C/O {mapReservation?.EndDate?.ToString("yyyy-MM-dd")} - {mapReservation?.ThirdPartyIdentifier} - {row.HouseAccountID} - {row.ReservationID} - {row.Description} - {row.TransactionDateTime?.ToString("HH:mm:ss")}";
                            if (line.TranDesc.Length > 512)
                                line.TranDesc = line?.TranDesc?.Substring(0, 512);
                            line.CuryDebitAmt = row.TransactionType?.ToUpper() == "CREDIT" ? row.Amount : 0;
                            line.CuryCreditAmt = row.TransactionType?.ToUpper() == "DEBIT" ? row.Amount : 0;
                            line = glGraph.GLTranModuleBatNbr.Cache.Insert(line) as GLTran;
                            lineNbrDic.Add(row.TransactionID, line.LineNbr);
                            #endregion

                            #region RuleB
                            line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
                            line.AccountID = mapCloudbedProerty?.ClearingAcct;
                            line.SubID = mapCloudbedProerty?.ClearingSub;
                            line.RefNbr = string.IsNullOrEmpty(row?.ReservationID) ? row.HouseAccountID.ToString() : row.ReservationID;
                            if (line.RefNbr.Length > 15)
                                line.RefNbr = line?.RefNbr?.Substring(0, 15);
                            line.Qty = row?.Quantity;
                            line.TranDesc = $"C/O {mapReservation?.EndDate?.ToString("yyyy-MM-dd")} - {mapReservation?.ThirdPartyIdentifier} - {row.HouseAccountID} - {row.ReservationID} - {row.Description} - {row.TransactionDateTime?.ToString("HH:mm:ss")}";
                            if (line.TranDesc.Length > 512)
                                line.TranDesc = line?.TranDesc?.Substring(0, 512);
                            line.CuryDebitAmt = row.TransactionType?.ToUpper() == "DEBIT" ? row.Amount : 0;
                            line.CuryCreditAmt = row.TransactionType?.ToUpper() == "CREDIT" ? row.Amount : 0;
                            glGraph.GLTranModuleBatNbr.Cache.Insert(line);
                            #endregion
                        }
                        catch (PXOuterException ex)
                        {
                            errorMsg = $"Error: {ex.InnerMessages[0]}";
                        }
                        catch (Exception ex)
                        {
                            errorMsg = $"Error: {ex.Message}";
                        }
                        finally
                        {
                            row.IsImported = string.IsNullOrEmpty(errorMsg) ? true : false;
                            row.ErrorMessage = errorMsg;
                            if (!(row.IsImported ?? false))
                                PXProcessing.SetError<LUMCloudBedTransactions>(errorMsg);
                            baseGraph.Transaction.Update(row);
                        }
                    }
                    #endregion

                    // Save Data(只要有一筆成功就Save)
                    if (cloudBedGroupRow.Any(x => x.IsImported ?? false))
                    {

                        errorMsg = string.Empty;
                        glGraph.releaseFromHold.Press();
                        glGraph.Save.Press();
                        glBatchNbr = doc.BatchNbr;
                    }
                }
                catch (PXOuterException ex)
                {
                    errorMsg = $"Error: {ex.InnerMessages[0]}";
                }
                catch (Exception ex)
                {
                    errorMsg = $"Error: {ex.Message}";
                }
                finally
                {
                    // 整組資料都失敗
                    if (!string.IsNullOrEmpty(errorMsg))
                        cloudBedGroupRow.ToList().ForEach(x =>
                        {
                            PXLongOperation.SetCurrentItem(x);
                            PXProcessing.SetError<LUMCloudBedTransactions>(errorMsg);
                            x.IsImported = false;
                            x.ErrorMessage = errorMsg;
                            baseGraph.Transaction.Update(x);
                        });
                    // 回寫BatchNbr & LineNbr
                    else
                        cloudBedGroupRow.ToList().ForEach(x =>
                        {
                            PXLongOperation.SetCurrentItem(x);
                            if (x.IsImported ?? false)
                                PXProcessing.SetProcessed<LUMCloudBedTransactions>();
                            else
                                PXProcessing.SetError<LUMCloudBedTransactions>(x.ErrorMessage);
                            x.BatchNbr = (x.IsImported ?? false) ? glBatchNbr : string.Empty;
                            x.LineNbr = (x?.IsImported ?? false) ? lineNbrDic[x.TransactionID] : null;
                            baseGraph.Transaction.Update(x);
                        });
                }

                // Save Process Result
                baseGraph.Actions.PressSave();
            }

        }

        /// <summary> Get Cloud Bed Transaction Data </summary>
        public static void GetCloudBedTransactionData(LUMCloudBedTransactionProcess baseGraph)
        {
            baseGraph.Transaction.Cache.Clear();
            var filter = baseGraph.TransacionFilter.Current;
            if (!filter.FromDate.HasValue || !filter.ToDate.HasValue)
                throw new PXException("Datetime is required!!");
            var transNewData = CloudBedHelper.GetTransactionData(filter.FromDate.Value, filter.ToDate.Value);
            if (transNewData == null)
                throw new PXException("Get Transaction Failed!!");
            var transOldData = baseGraph.CurrentTransaction.Select().RowCast<LUMCloudBedTransactions>();
            using (PXTransactionScope sc = new PXTransactionScope())
            {
                foreach (var row in transNewData)
                {
                    DateTime newDateTime;
                    var existsRow = transOldData.FirstOrDefault(x => x.TransactionID == row.transactionID);
                    // 如果相同TransactionID資料存在且Imported -> Skip
                    if (existsRow != null && (existsRow.IsImported ?? false))
                        continue;
                    var trans = existsRow ?? baseGraph.CurrentTransaction.Cache.CreateInstance() as LUMCloudBedTransactions;
                    #region Mapping Field
                    trans.IsImported = false;
                    trans.BatchNbr = null;
                    trans.LineNbr = null;
                    trans.PropertyID = row.propertyID;
                    trans.ReservationID = row.reservationID;
                    trans.SubReservationID = row.subReservationID;
                    trans.HouseAccountID = row.houseAccountID;
                    trans.HouseAccountName = row.houseAccountName;
                    trans.GuestID = row.guestID;
                    trans.PropertyName = row.propertyName;
                    if (DateTime.TryParse(row.transactionDateTime, out newDateTime))
                        trans.TransactionDateTime = newDateTime.AddHours(8);
                    if (DateTime.TryParse(row.transactionDateTimeUTC, out newDateTime))
                        trans.TransactionDateTimeUTC = newDateTime;
                    if (DateTime.TryParse(row.transactionModifiedDateTime, out newDateTime))
                        trans.TransactionLastModifiedDateTime = newDateTime;
                    if (DateTime.TryParse(row.transactionModifiedDateTimeUTC, out newDateTime))
                        trans.TransactionLastModifiedDateTimeUTC = newDateTime;
                    if (DateTime.TryParse(row.guestCheckIn, out newDateTime))
                        trans.GuestCheckIn = newDateTime;
                    if (DateTime.TryParse(row.guestCheckOut, out newDateTime))
                        trans.GuestCheckOut = newDateTime;
                    trans.RoomTypeID = row.roomTypeID;
                    trans.RoomTypeName = row.roomTypeName;
                    trans.RoomName = row.roomName;
                    trans.GuestName = row.guestName;
                    trans.Description = row.description;
                    trans.Category = row.category;
                    trans.TransactionCode = row.transactionCode;
                    trans.TransactionNotes = row.notes;
                    trans.Quantity = int.Parse(row.quantity);
                    trans.Amount = (decimal?)row.amount;
                    trans.Currency = row.currency;
                    trans.UserName = row.userName;
                    trans.TransactionType = row.transactionType;
                    trans.TransactionCategory = row.transactionCategory;
                    trans.ItemCategoryName = row.itemCategoryName;
                    trans.TransactionID = row.transactionID;
                    trans.ParentTransactionID = row.parentTransactionID;
                    trans.CardType = row.cardType;
                    trans.IsDeleted = row.isDeleted;
                    #endregion
                    if (existsRow == null)
                        baseGraph.CurrentTransaction.Cache.Insert(trans);
                    else
                        baseGraph.CurrentTransaction.Cache.Update(trans);
                }
                baseGraph.Save.Press();
                sc.Complete();
            }
        }

        /// <summary> Get Cloud Bed Reservation Data </summary>
        public static void GetCloudBedReservationData(LUMCloudBedTransactionProcess baseGraph)
        {
            var filter = baseGraph.ReservationFilter.Current;
            if (!filter.ReservationFromDate.HasValue || !filter.ReservationToDate.HasValue)
                throw new PXException("Datetime is required!!");
            var reservationNewData = CloudBedHelper.GetReservationData(filter.ReservationFromDate.Value, filter.ReservationToDate.Value);
            if (reservationNewData == null)
                throw new PXException("Get Reservation Failed!!");
            var reservationOldData = baseGraph.Reservations.Select().RowCast<LUMCloudBedReservations>();
            using (PXTransactionScope sc = new PXTransactionScope())
            {
                foreach (var row in reservationNewData)
                {
                    var existsRow = reservationOldData.FirstOrDefault(x => x.PropertyID == row.propertyID && x.ReservationID == row.reservationID);
                    var reservation = existsRow ?? baseGraph.Reservations.Cache.CreateInstance() as LUMCloudBedReservations;
                    #region Mapping Field
                    reservation.PropertyID = row.propertyID;
                    reservation.ReservationID = row.reservationID;
                    reservation.DateCreated = DateTime.Parse(row.dateCreated);
                    reservation.DateModified = DateTime.Parse(row.dateModified);
                    reservation.Source = row.sourceName;
                    reservation.ThirdPartyIdentifier = row.thirdPartyIdentifier;
                    reservation.Status = row.status;
                    reservation.StartDate = DateTime.Parse(row.startDate);
                    reservation.EndDate = DateTime.Parse(row.endDate);
                    reservation.Balance = (decimal?)row.balance;
                    #endregion
                    if (existsRow != null)
                        baseGraph.Reservations.Cache.Update(reservation);
                    else
                        baseGraph.Reservations.Cache.Insert(reservation);
                }
                baseGraph.Save.Press();
                sc.Complete();
            }
        }

        private static int CompareProps(LUMCloudBedAccountMapping target, LUMCloudBedAccountMapping source)
        {
            var match = 0;
            var availableName = new string[] { "CloudBedPropertyID", "TransCategory", "HouseAccount", "TransactionCode", "Description", "Source" };
            var aValues = target.GetType().GetProperties().Where(x => availableName.Contains(x.Name)).Select(x => string.IsNullOrEmpty((string)x.GetValue(target, null)) ? "" : x.GetValue(target, null));
            var bValues = source.GetType().GetProperties().Where(x => availableName.Contains(x.Name)).Select(x => string.IsNullOrEmpty((string)x.GetValue(source, null)) ? "" : x.GetValue(source, null));
            for (int i = 0; i < aValues.Count(); i++)
                match = aValues.ElementAt(i)?.ToString()?.Trim()?.ToUpper() == bValues.ElementAt(i)?.ToString()?.Trim()?.ToUpper() ? match + 1 : match;
            return match;
        }

        #endregion
    }

    [Serializable]
    public class TransactionFilter : IBqlTable
    {
        [PXDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Transaction From")]
        public virtual DateTime? FromDate { get; set; }
        public abstract class fromDate : PX.Data.BQL.BqlDateTime.Field<fromDate> { }

        [PXDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Transaction To")]
        public virtual DateTime? ToDate { get; set; }
        public abstract class toDate : PX.Data.BQL.BqlDateTime.Field<toDate> { }

        [PXBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Display Imported Transaction")]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
    }

    [Serializable]
    public class ReservationFilter : IBqlTable
    {
        [PXDateAndTime(DisplayMask = "g", InputMask = "g")]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "ReservationUpdate From")]
        public virtual DateTime? ReservationFromDate { get; set; }
        public abstract class reservationFromDate : PX.Data.BQL.BqlDateTime.Field<reservationFromDate> { }

        [PXDateAndTime(DisplayMask = "g", InputMask = "g")]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "ReservationUpdate To")]
        public virtual DateTime? ReservationToDate { get; set; }
        public abstract class reservationToDate : PX.Data.BQL.BqlDateTime.Field<reservationToDate> { }
    }
}
