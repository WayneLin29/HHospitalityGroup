using HHAPICustomization.DAC;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.BQL.Fluent;
using HH_APICustomization.DAC;
using System.Collections;
using PX.Data.BQL;
using HH_APICustomization.APIHelper;
using PX.Objects.GL;
using PX.Objects.CR;
using PX.Objects.EP;
using HH_APICustomization.Descriptor;
using PX.SM;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PX.Objects.IN.WMS;

namespace HH_APICustomization.Graph
{
    public class LUMCloudBedRemitTransactionProcess : PXGraph<LUMCloudBedRemitTransactionProcess, LUMRemittance>
    {
        #region Hidden BQL View

        [PXHidden]
        public PXSetup<LUMHHSetup> Setup;

        [PXHidden]
        public SelectFrom<LUMCloudBedPreference>
              .Where<LUMCloudBedPreference.branchID.IsEqual<LUMRemittance.branch.FromCurrent>>
              .View ClodBedPreference;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>
              .Where<LUMCloudBedTransactions.propertyID.IsEqual<LUMCloudBedTransactions.propertyID.AsOptional>
                .And<Brackets<LUMCloudBedTransactions.remitRefNbr.IsNull>.Or<LUMCloudBedTransactions.remitRefNbr.IsEqual<LUMCloudBedTransactions.remitRefNbr.AsOptional>>>
                .And<Brackets<LUMCloudBedTransactions.isImported.IsNull>.Or<LUMCloudBedTransactions.isImported.IsEqual<False>>>
                .And<Brackets<LUMCloudBedTransactions.isDeleted.IsNull>.Or<LUMCloudBedTransactions.isDeleted.IsEqual<False>>>
                .And<Brackets<LUMCloudBedTransactions.houseAccountID.IsNotNull>.Or<LUMCloudBedTransactions.reservationID.IsNotNull>>>
              .View PendingTransactions;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>
              .Where<LUMCloudBedTransactions.propertyID.IsEqual<LUMCloudBedTransactions.propertyID.AsOptional>>
              .View PropertyCloudbedTransactions;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>
              .Where<LUMCloudBedTransactions.remitRefNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>
                 .And<Brackets<LUMCloudBedTransactions.accountID.IsNull.Or<LUMCloudBedTransactions.subAccountID.IsNull>>>>
              .View NonValidCloudbedTransactions;

        [PXHidden]
        public SelectFrom<LUMRemitExcludeTransactions>
              .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View ExculdeTransactions;

        [PXHidden]
        public SelectFrom<vHHRemitReservationCheck>
              .Where<vHHRemitReservationCheck.reservationID.IsNotNull
                .And<vHHRemitReservationCheck.propertyID.IsEqual<vHHRemitReservationCheck.propertyID.AsOptional>>>
              .View PropertyRemitReservationCheck;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>
              .Where<LUMCloudBedTransactions.remitRefNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View RemitTransactions;

        [PXHidden]
        public PXSetup<LUMRemitRequestApproval> RemitApproval;

        [PXHidden]
        public SelectFrom<vRemitBlockCheck>.View RemitBlockCheck;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>
              .Where<LUMCloudBedTransactions.remitRefNbr.IsEqual<LUMCloudBedTransactions.remitRefNbr.AsOptional>
                .And<LUMCloudBedTransactions.propertyID.IsEqual<LUMCloudBedTransactions.propertyID.AsOptional>>
                .And<LUMCloudBedTransactions.reservationID.IsEqual<LUMCloudBedTransactions.reservationID.AsOptional>>>
              .View TransactionByReservation;

        #endregion

        [PXViewName("Remit Document")]
        public SelectFrom<LUMRemittance>.View Document;

        [PXViewName("Remit Current Document")]
        public SelectFrom<LUMRemittance>
              .Where<LUMRemittance.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View CurrentDocument;

        [PXViewName("Remit Current Document")]
        public SelectFrom<LUMRemittance>
            .Where<LUMRemittance.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
            .View CurrentDocument2;

        [PXCacheName("PaymentSummary")]
        public SelectFrom<LUMRemitPayment>
              .Where<LUMRemitPayment.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View PaymentSummary;

        [PXCacheName("PaymentSummaryTransactions")]
        public SelectFrom<LUMCloudBedTransactions>
              .LeftJoin<LUMCloudBedReservations>.On<LUMCloudBedTransactions.propertyID.IsEqual<LUMCloudBedReservations.propertyID>
                                               .And<LUMCloudBedTransactions.reservationID.IsEqual<LUMCloudBedReservations.reservationID>>>
              .Where<LUMCloudBedTransactions.description.IsEqual<LUMRemitPayment.description.FromCurrent>
                .And<LUMCloudBedTransactions.remitRefNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>>
              .OrderBy<Desc<LUMCloudBedTransactionsExt.toRemit, Desc<LUMCloudBedTransactions.reservationID, Desc<LUMCloudBedTransactions.remitRefNbr>>>>
              .View PaymentSummaryTransactions;

        [PXCacheName("RemitReservation")]
        public SelectFrom<LUMRemitReservation>
              .Where<LUMRemitReservation.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .OrderBy<LUMRemitReservation.toRemitCount.Desc, LUMRemitReservation.checkMessage.Asc, LUMRemitReservation.checkIn.Asc, LUMRemitReservation.reservationID.Asc>
              .View ReservationSummary;

        [PXCacheName("ReservationSummaryTransactions")]
        public SelectFrom<LUMCloudBedTransactions2>
              .Where<LUMCloudBedTransactions2.reservationID.IsEqual<LUMRemitReservation.reservationID.FromCurrent>>
              .OrderBy<LUMCloudBedTransactionsExt.toRemit.Desc, LUMCloudBedTransactions2.createdDateTime.Asc, LUMCloudBedTransactions2.transactionID.Asc>
              .View ReservationSummaryTransactions;

        [PXCacheName("Remit Block")]
        public SelectFrom<LUMRemitBlock>
              .InnerJoin<LUMCloudBedRoomBlock>.On<LUMRemitBlock.blockID.IsEqual<LUMCloudBedRoomBlock.roomBlockID>>
              .LeftJoin<LUMCloudBedRoomBlockDetails>.On<LUMCloudBedRoomBlock.roomBlockID.IsEqual<LUMCloudBedRoomBlockDetails.roomBlockID>
                                                   .And<LUMCloudBedRoomBlockDetails.propertyID.IsEqual<LUMCloudBedRoomBlock.propertyID>>>
              .Where<LUMRemitBlock.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View RemitBlock;

        [PXCacheName("Folio Transactions")]
        public SelectFrom<LUMCloudBedTransactions3>
              .Where<LUMCloudBedTransactions3.remitRefNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>
                 .Or<Brackets<LUMCloudBedTransactions.remitRefNbr.IsNull>.And<LUMCloudBedTransactions.isImported.IsNotEqual<True>>>>
              .OrderBy<Desc<LUMCloudBedTransactions3.sortNumber>>
              .View FolioTransactioins;

        public PXFilter<LUMShowSystemPostFilter> TransactionFilter;

        public PXFilter<LUMShowSystemPostFilter> FolioFilter;

        public PXFilter<LUMBatchUpdateAcctFilter> QuickAccountDetermine;

        [PXViewName("Approval Remit")]
        public EPApprovalAutomation<LUMRemittance, LUMRemittance.approved, LUMRemittance.rejected, LUMRemittance.hold, LUMRemitRequestApproval> Approval;

        IEnumerable<LUMCloudBedTransactions> transWithFilter = new List<LUMCloudBedTransactions>();

        #region Delegate Data View
        /// <summary> Override Payment summary transactions bql view </summary>
        public IEnumerable paymentSummaryTransactions()
        {
            PXView select = new PXView(this, false, this.PaymentSummaryTransactions.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;

            var _propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
            var _remitRefNbr = this.Document.Current?.RefNbr;
            var toggleOutTrans = SelectFrom<LUMCloudBedTransactions>
                                .InnerJoin<LUMRemitExcludeTransactions>.On<LUMCloudBedTransactions.transactionID.IsEqual<LUMRemitExcludeTransactions.transactionID>
                                      .And<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>>
                                .LeftJoin<LUMCloudBedReservations>.On<LUMCloudBedTransactions.propertyID.IsEqual<LUMCloudBedReservations.propertyID>
                                     .And<LUMCloudBedTransactions.reservationID.IsEqual<LUMCloudBedReservations.reservationID>>>
                                .Where<LUMCloudBedTransactions.description.IsEqual<P.AsString>>
                                .View.Select(this, _remitRefNbr, this.PaymentSummary.Current?.Description);
            PXResultset<LUMCloudBedTransactions, LUMCloudBedReservations> _toggleOutTrans = new PXResultset<LUMCloudBedTransactions, LUMCloudBedReservations>();
            foreach (PXResult<LUMCloudBedTransactions, LUMRemitExcludeTransactions, LUMCloudBedReservations> item in toggleOutTrans)
            {
                _toggleOutTrans.Add(new PXResult<LUMCloudBedTransactions, LUMCloudBedReservations>((LUMCloudBedTransactions)item, (LUMCloudBedReservations)item));
            }
            //var newResult = result.RowCast<LUMCloudBedTransactions>().Where(x => x.PropertyID == _propertyID).ToList();
            var newResult = result.Union(_toggleOutTrans).ToList();
            foreach (PXResult<LUMCloudBedTransactions, LUMCloudBedReservations> item in newResult)
            {
                LUMCloudBedTransactions trans = (LUMCloudBedTransactions)item;
                LUMCloudBedReservations res = (LUMCloudBedReservations)item;
                if (trans.PropertyID == _propertyID && trans?.UserName != "SYSTEM")
                    yield return new PXResult<LUMCloudBedTransactions, LUMCloudBedReservations>(trans, res);
            }
        }

        /// <summary> Override Reservation summary transactions bql view </summary>
        public IEnumerable reservationSummaryTransactions()
        {
            PXView select = new PXView(this, false, this.ReservationSummaryTransactions.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;

            var _propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
            var _remitRefNbr = this.Document.Current?.RefNbr;
            // Untion HoustAccount transaction
            var currentResverationID = this.ReservationSummary.Current?.ReservationID;
            List<LUMCloudBedTransactions2> houseTreans = new List<LUMCloudBedTransactions2>();
            if (currentResverationID != null && currentResverationID?.IndexOf('-') != -1)
            {
                // 找HouseAccount
                var _houstAccountID = currentResverationID.Substring(0, currentResverationID.IndexOf('-'));
                houseTreans = SelectFrom<LUMCloudBedTransactions2>
                             .Where<LUMCloudBedTransactions2.propertyID.IsEqual<P.AsString>
                               .And<LUMCloudBedTransactions2.houseAccountID.IsEqual<P.AsInt>>
                               .And<LUMCloudBedTransactions2.remitRefNbr.IsEqual<P.AsString>>>
                             .View.Select(this, _propertyID, _houstAccountID, _remitRefNbr).RowCast<LUMCloudBedTransactions2>().ToList();

                // 使用Account Determine,因HouseTran資料已經在Update Cache導致畫面不會呈現
                //if (houseTreans.Count == 0 && this.ReservationDetails.Cache.Updated.RowCast<LUMCloudBedTransactions2>().Count() > 0)
                houseTreans = houseTreans.Union(this.ReservationSummaryTransactions.Cache.Updated.RowCast<LUMCloudBedTransactions2>()).ToList();

                var toggleOutTrans = SelectFrom<LUMCloudBedTransactions2>
                                    .InnerJoin<LUMRemitExcludeTransactions>.On<LUMCloudBedTransactions2.transactionID.IsEqual<LUMRemitExcludeTransactions.transactionID>
                                          .And<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>>
                                   .Where<LUMCloudBedTransactions2.houseAccountID.IsEqual<P.AsInt>>
                                    .View.Select(this, _remitRefNbr, _houstAccountID).RowCast<LUMCloudBedTransactions2>().ToList();
                houseTreans = houseTreans.Union(toggleOutTrans).ToList();
            }
            result = result.Union(houseTreans).ToList();

            var isShowPost = this.TransactionFilter.Current?.ShowPost;
            if (isShowPost ?? false)
            {
                foreach (var item in result)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var item in result.Where(x => ((LUMCloudBedTransactions2)x)?.UserName != "SYSTEM"))
                {
                    yield return item;
                }
            }
        }

        /// <summary> Override Folio transactions bql view </summary>
        public IEnumerable folioTransactioins()
        {
            PXView select = new PXView(this, false, this.FolioTransactioins.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;

            var _propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
            var _remitRefNbr = this.Document.Current?.RefNbr;
            // Toggle out transactions
            var toggleOutTrans = SelectFrom<LUMCloudBedTransactions3>
                                .InnerJoin<LUMRemitExcludeTransactions>.On<LUMCloudBedTransactions3.transactionID.IsEqual<LUMRemitExcludeTransactions.transactionID>
                                      .And<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>>
                                .View.Select(this, _remitRefNbr).RowCast<LUMCloudBedTransactions3>();

            //var otherTrans = SelectFrom<LUMCloudBedTransactions3>
            //                .InnerJoin<LUMRemittance>.On<LUMCloudBedTransactions3.remitRefNbr.IsEqual<LUMRemittance.refNbr>
            //                                        .And<LUMRemittance.refNbr.IsNotEqual<P.AsString>>
            //                                        .And<LUMRemittance.status.IsNotIn<P.AsString>>>
            //                .Where<LUMCloudBedTransactions3.propertyID.IsEqual<P.AsString>>
            //                .View.Select(this, _remitRefNbr, new string[] { "R", "V" }, _propertyID).RowCast<LUMCloudBedTransactions3>();

            //var newResult = result.Union(toggleOutTrans).Union(otherTrans);
            var newResult = result.Union(toggleOutTrans);
            //var newResult = result.Union(toggleOutTrans);

            var isShowPost = this.TransactionFilter.Current?.ShowPost;

            foreach (LUMCloudBedTransactions3 item in newResult)
            {
                item.SortNumber = CalculateSortNumber(item, _remitRefNbr, toggleOutTrans);
                if (item.PropertyID != _propertyID)
                    continue;
                // Show All
                if (isShowPost ?? false)
                    yield return item;
                // Show not SYSTEM
                else if (item.UserName != "SYSTEM")
                    yield return item;
            }
            FolioTransactioins.OrderByNew<OrderBy<Asc<LUMCloudBedTransactions3.sortNumber>>>();
        }

        public IEnumerable remitBlock()
        {
            PXView select = new PXView(this, false, this.RemitBlock.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;

            foreach (PXResult<LUMRemitBlock, LUMCloudBedRoomBlock, LUMCloudBedRoomBlockDetails> item in result)
            {
                var room = (LUMCloudBedRoomBlockDetails)item;
                var block = (LUMRemitBlock)item;
                var assignement = SelectFrom<LUMCloudBedRoomAssignment>
                                 .Where<LUMCloudBedRoomAssignment.roomid.IsEqual<P.AsString>>
                                 .View.Select(this, room.Roomid).TopFirst;
                block.RoomName = assignement?.RoomName;
                yield return item;
            }
        }

        #endregion

        #region Action
        public PXAction<LUMRemittance> Refresh;
        [PXButton]
        [PXUIField(DisplayName = "PREPARE", MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable refresh(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                this.Save.Press();
                RemittanceHelper helper = new RemittanceHelper();

                var _refNbr = this.Document.Current?.RefNbr;
                // ReloadCloudBedData();
                var _propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
                // 符合條件且未被處理的Transactions(Old: allowTransByProperty)
                var pendingTransactions = helper.GetBqlCommand<LUMCloudBedTransactions>(this, this.PendingTransactions.View.BqlSelect, _propertyID, _refNbr);
                // 被Toggle out 的Transactions
                var excludedTransactions = helper.GetBqlCommand<LUMRemitExcludeTransactions>(this, this.ExculdeTransactions.View.BqlSelect);
                // expected ExcludeTransaction
                pendingTransactions = helper.FilterExcludedTransactions(pendingTransactions, excludedTransactions);

                // Reservation Check Data
                var reservationChecks = helper.GetBqlCommand<vHHRemitReservationCheck>(this, this.PropertyRemitReservationCheck.View.BqlSelect, _propertyID);
                var groupReservationChecks = reservationChecks.OrderBy(x => x.SortBy).GroupBy(x => new { x.PropertyID, x.ReservationID }).Select(y => y.FirstOrDefault());
                // All cloudbed transaction filter by property
                var propertyTransactions = helper.GetBqlCommand<LUMCloudBedTransactions>(this, this.PropertyCloudbedTransactions.View.BqlSelect, _propertyID);
                // 現有已存在的Remit Reservation
                var currentRemitReservations = helper.GetBqlCommand<LUMRemitReservation>(this, this.ReservationSummary.View.BqlSelect);
                // 有符合條件下的cloudbed transactions
                var matchReservationCheckTransactions = helper.GetMatchReservationCheckTransactions(propertyTransactions, groupReservationChecks, currentRemitReservations);

                #region Insert/Update LUMRemitPayment (according to pendingTransactions) (Payment Summary)

                // 既有的Remit Payment
                var existsRemitPayments = helper.GetBqlCommand<LUMRemitPayment>(this, this.PaymentSummary.View.BqlSelect);
                // Insert/Update LUMRemitPayment 
                foreach (var matchedTransaction in pendingTransactions.Where(x => x.TransactionType == "credit" && !(x?.IsImported ?? false) && x.UserName != "SYSTEM").GroupBy(x => x.Description))
                {
                    var paymentLine = existsRemitPayments.FirstOrDefault(x => x.Description == matchedTransaction.Key);
                    if (paymentLine != null)
                    {
                        paymentLine.RecordedAmt = matchedTransaction.Sum(x => x?.Amount);
                        paymentLine.RemitAmt = 0;
                        paymentLine = this.PaymentSummary.Update(paymentLine);
                    }
                    else
                    {
                        paymentLine = this.PaymentSummary.Cache.CreateInstance() as LUMRemitPayment;
                        paymentLine.Description = matchedTransaction.Key;
                        paymentLine.RecordedAmt = matchedTransaction.Sum(x => x?.Amount);
                        paymentLine.RemitAmt = 0;
                        paymentLine = this.PaymentSummary.Insert(paymentLine);
                    }
                }

                #endregion

                #region Update Cloudbed Transaction Remit Refnbr / AccountID / Sub-AccountID (according to pending transactions)

                // Update Cloudbed Transaction Remit Refnbr / AccountID / Sub-AccountID
                foreach (var pendingItem in pendingTransactions)
                {
                    // 沒有Account/SubAccount 才重新執行
                    if (!pendingItem.AccountID.HasValue || !pendingItem.SubAccountID.HasValue)
                    {
                        var mapAccountInfo = CloudBedHelper.GetCloudbedAccountMappingWithScore(this, pendingItem, false);
                        if (mapAccountInfo != null)
                        {
                            pendingItem.AccountID = mapAccountInfo?.AccountID;
                            pendingItem.SubAccountID = mapAccountInfo?.SubAccountID;
                        }
                    }
                    pendingItem.RemitRefNbr = _refNbr;
                    this.PropertyCloudbedTransactions.Update(pendingItem);
                    InvokeCachePersist<LUMCloudBedTransactions>(PXDBOperation.Update);
                }

                #endregion

                #region Delete Current Remit Reservation and Clean Transaction
                /// 待刪除的Reservation(According to SQL View 中不存在的)
                /// 每次執行如果SQL View中沒有此Reservation就需要刪除
                var deleteReservations = currentRemitReservations.Select(x => x.ReservationID).Except(reservationChecks.Select(y => y.ReservationID));
                foreach (var deleteReservationID in deleteReservations)
                {
                    this.ReservationSummary.Delete(currentRemitReservations.FirstOrDefault(x => x.ReservationID == deleteReservationID));
                    foreach (var deleteTrans in pendingTransactions.Where(x => x.RemitRefNbr == _refNbr && x.ReservationID == deleteReservationID))
                    {
                        deleteTrans.RemitRefNbr = null;
                        this.PropertyCloudbedTransactions.Update(deleteTrans);
                    }
                }

                #endregion

                #region Insert/Update/Remove LUMRemitReservation (Reservation Summary)

                // Insert/Update/Delete LUMRemitReservation
                foreach (var grpAvailableTran in matchReservationCheckTransactions.GroupBy(x => new { x.PropertyID, x.ReservationID, x.HouseAccountID }))
                {
                    var resLine = new LUMRemitReservation();
                    var mapReservation = LUMCloudBedReservations.PK.Find(this, grpAvailableTran.Key.PropertyID, grpAvailableTran.Key.ReservationID);
                    var isExists = currentRemitReservations.FirstOrDefault(x => x.RefNbr == _refNbr && x.ReservationID == grpAvailableTran.Key.ReservationID) != null;
                    resLine = currentRemitReservations.FirstOrDefault(x => x.RefNbr == _refNbr && x.ReservationID == grpAvailableTran.Key.ReservationID) ?? resLine;
                    resLine.RefNbr = _refNbr;
                    resLine.ReservationID = grpAvailableTran.Key.ReservationID ?? grpAvailableTran.Key.HouseAccountID?.ToString();
                    if (resLine?.IsOutOfScope ?? false)
                        continue;

                    var checkObject = reservationChecks.FirstOrDefault(x => x.ReservationID == grpAvailableTran.Key.ReservationID || GetHouseAccountByReservationID(x.ReservationID) == grpAvailableTran.Key.HouseAccountID?.ToString());
                    // 刪除Type != 'RS' 而且該Reservatioin下Transaction 不等於Current RemitRef
                    //if (checkObj.Type != "RS" && !availableReservation)
                    //{
                    //    if (isExists)
                    //        this.ReservationSummary.Delete(resLine);
                    //    continue;
                    //}

                    resLine.CheckMessage = checkObject?.Message;
                    resLine.GuestName = mapReservation?.GuestName;
                    resLine.Status = mapReservation?.Status;
                    resLine.CheckIn = mapReservation?.StartDate;
                    resLine.CheckOut = mapReservation?.EndDate;
                    resLine.Balance = mapReservation?.Balance ?? 0;
                    resLine.Total = mapReservation?.Total ?? 0;
                    resLine.RoomRevenue = 0;
                    resLine.Taxes = 0;
                    resLine.Fees = 0;
                    resLine.Others = 0;
                    resLine.Payment = 0;
                    resLine.PendingCount = 0;
                    resLine.ToRemitCount = 0;
                    CalculateReservationAmount(_refNbr, checkObject, resLine);
                    resLine = isExists ? this.ReservationSummary.Update(resLine) : this.ReservationSummary.Insert(resLine);
                }
                #endregion

                #region Insert/Update LUMRemitBlock
                var exisBlocks = this.RemitBlock.View.SelectMulti().RowCast<LUMRemitBlock>();
                var availabeBlocks = this.RemitBlockCheck.View.SelectMulti().RowCast<vRemitBlockCheck>();
                foreach (var checkBlock in availabeBlocks.Where(x => x.PropertyID == _propertyID).GroupBy(x => new { x.PropertyID, x.RoomBlockID }))
                {
                    var blockLine = new LUMRemitBlock();
                    var isExists = exisBlocks.FirstOrDefault(x => x.RefNbr == _refNbr && x.BlockID == checkBlock.Key.RoomBlockID) != null;
                    blockLine = exisBlocks.FirstOrDefault(x => x.RefNbr == _refNbr && x.BlockID == checkBlock.Key.RoomBlockID) ?? blockLine;
                    blockLine.RefNbr = _refNbr;
                    blockLine.BlockID = checkBlock.Key.RoomBlockID;
                    blockLine.CheckMessage = checkBlock.FirstOrDefault()?.Message;
                    blockLine = isExists ? this.RemitBlock.Update(blockLine) : this.RemitBlock.Insert(blockLine);
                }
                #endregion

                // Calculate Document Amount
                CleanUpDocumentAmount();
                CalculateDocumentAmount(this.RemitTransactions.View.SelectMulti().RowCast<LUMCloudBedTransactions>());

                this.Save.Press();
            });
            return adapter.Get();
        }

        public PXAction<LUMRemittance> Sync;
        [PXButton]
        [PXUIField(DisplayName = "SYNC", MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable sync(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                ReloadCloudBedData();
            });
            return adapter.Get();
        }

        public PXAction<LUMRemittance> Hold;
        [PXButton]
        [PXUIField(DisplayName = "HOLD", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable hold(PXAdapter adapter)
        {
            var doc = this.Document.Current;
            if (doc != null)
            {
                // 重進簽核
                RemoveApprovalHistory(doc);
                this.Document.Cache.SetValue<LUMRemittance.approved>(doc, false);
                this.Document.Cache.SetValue<LUMRemittance.rejected>(doc, false);
                this.Document.Cache.SetValueExt<LUMRemittance.hold>(doc, true);
                this.Document.Update(doc);
                this.Save.Press();
            }
            return adapter.Get();
        }

        public PXAction<LUMRemittance> ReleaseFromHold;
        [PXButton]
        [PXUIField(DisplayName = "REMOVE HOLD", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable releaseFromHold(PXAdapter adapter)
        {
            var doc = this.Document.Current;
            if (doc != null)
            {
                bool isValid = true;
                #region Valid
                // Valid Payment OPRemark/Open Amount
                foreach (var item in this.PaymentSummary.View.SelectMulti().RowCast<LUMRemitPayment>())
                {
                    if (string.IsNullOrEmpty(item.OPRemark))
                    {
                        this.PaymentSummary.Cache.RaiseExceptionHandling<LUMRemitPayment.oPRemark>(item, item.OPRemark,
                            new PXSetPropertyException<LUMRemitPayment.oPRemark>("OPRemark is required.", PXErrorLevel.Error));
                        isValid = false;
                    }

                    if ((item?.OpenAmt ?? 0) != 0)
                    {
                        this.PaymentSummary.Cache.RaiseExceptionHandling<LUMRemitPayment.openAmt>(item, item.OpenAmt,
                           new PXSetPropertyException<LUMRemitPayment.openAmt>("Open Amount is not 0, Please confirm your Remit Amount in Payment Check.", PXErrorLevel.Error));
                        isValid = false;
                    }

                }
                // Valid Reservation OPRemark
                foreach (var item in this.ReservationSummary.View.SelectMulti().RowCast<LUMRemitReservation>())
                {
                    if (string.IsNullOrEmpty(item.OPRemark))
                    {
                        this.ReservationSummary.Cache.RaiseExceptionHandling<LUMRemitReservation.oPRemark>(item, item.OPRemark,
                            new PXSetPropertyException<LUMRemitReservation.oPRemark>("OPRemark is required.", PXErrorLevel.Error));
                        isValid = false;
                    }
                }
                #endregion

                if (!isValid)
                    throw new PXException("Operation Remark is mandatory, please complete necessary entry.");

                this.Document.Cache.SetValueExt<LUMRemittance.hold>(doc, false);
                this.Document.Update(doc);
                // 如果沒有進入簽核則直接將狀態改為Approve
                if (SelectFrom<EPApproval>.Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>.View.Select(this, doc.Noteid).Count == 0)
                {
                    this.Document.Cache.SetValue<LUMRemittance.approved>(doc, true);
                    this.Document.Cache.SetValue<LUMRemittance.status>(doc, LUMRemitStatus.Open);
                    this.Document.Update(doc);
                }
                this.Save.Press();
            }
            return adapter.Get();
        }

        public PXAction<LUMRemittance> Approve;
        [PXButton]
        [PXUIField(DisplayName = "Approve", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable approve(PXAdapter adapter)
        {
            var doc = this.Document.Current;
            if (doc != null)
            {
                doc.Approved = true;
                doc.Status = LUMRemitStatus.Open;
                this.Document.UpdateCurrent();
                this.Save.Press();
                UpdateStatusMaunal(doc);
            }
            return adapter.Get();
        }

        public PXAction<LUMRemittance> Reject;
        [PXButton]
        [PXUIField(DisplayName = "Reject", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable reject(PXAdapter adapter)
        {
            var doc = this.Document.Current;
            if (doc != null)
            {
                doc.Rejected = true;
                doc.Status = LUMRemitStatus.Rejected;
                this.Document.UpdateCurrent();
                this.Save.Press();
            }
            return adapter.Get();
        }

        /// <summary> TOGGLE OUT transaction </summary>
        public PXAction<LUMRemittance> PaymentToggleOut;
        [PXButton]
        [PXUIField(DisplayName = "TOGGLE OUT", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable paymentToggleOut(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedItems = this.PaymentSummaryTransactions.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && (x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleOutTransactions(selectedItems);
            });
            return adapter.Get();
        }

        /// <summary> TOGGLE IN transaction </summary>
        public PXAction<LUMRemittance> PaymentToggleIn;
        [PXButton]
        [PXUIField(DisplayName = "TOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable paymentToggleIn(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedItems = this.PaymentSummaryTransactions.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && !(x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleInTransactions(selectedItems);
            });
            return adapter.Get();
        }

        /// <summary> TOGGLE OUT transaction(Reservation) </summary>
        public PXAction<LUMRemittance> ReservationToggleOut;
        [PXButton]
        [PXUIField(DisplayName = "TOGGLE OUT", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable reservationToggleOut(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedItems = ReservationSummaryTransactions.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && (x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleOutTransactions(selectedItems);
            });
            return adapter.Get();
        }

        /// <summary> TOGGLE IN transaction(Reservations) </summary>
        public PXAction<LUMRemittance> ReservationToggleIn;
        [PXButton]
        [PXUIField(DisplayName = "TOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable reservationToggleIn(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedItems = this.ReservationSummaryTransactions.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && !(x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleInTransactions(selectedItems);
            });
            return adapter.Get();
        }

        /// <summary> TOGGLE IN transaction(Folio) </summary>
        public PXAction<LUMRemittance> FolioToggleIn;
        [PXButton]
        [PXUIField(DisplayName = "TOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable folioToggleIn(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedItems = this.FolioTransactioins.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && !(x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleInTransactions(selectedItems);
            });
            return adapter.Get();
        }

        /// <summary> TOGGLE OUT transaction(Folio) </summary>
        public PXAction<LUMRemittance> FolioToggleOut;
        [PXButton]
        [PXUIField(DisplayName = "TOGGLE OUT", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable folioToggleOut(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedItems = this.FolioTransactioins.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && (x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleOutTransactions(selectedItems);
            });
            return adapter.Get();
        }

        /// <summary> Out of Scope Reservations </summary>
        public PXAction<LUMRemittance> OutOfScope;
        [PXButton]
        [PXUIField(DisplayName = "OUT OF SCOPE", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable outOfScope(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var tempRes = this.ReservationSummary.Cache.Updated.RowCast<LUMRemitReservation>().Where(x => x.Selected ?? false && !(x?.IsOutOfScope ?? false));
                var selectedItems = new List<LUMCloudBedTransactions>();
                foreach (var res in tempRes)
                {
                    // 如果該Reservation 底下有Transaction
                    selectedItems.AddRange(GetTransacitonBelongToReservation(res?.ReservationID));
                    UpdateReservationWithScope(res, true);
                }
                if (selectedItems.Count > 0)
                    ToggleOutTransactions(selectedItems, true);
                else
                    this.Save.Press();
            });
            return adapter.Get();
        }

        /// <summary> In of Scope Reservations </summary>
        public PXAction<LUMRemittance> InScope;
        [PXButton]
        [PXUIField(DisplayName = "IN SCOPE", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable inScope(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedRes = this.ReservationSummary.Cache.Updated.RowCast<LUMRemitReservation>().Where(x => x.Selected ?? false && (x?.IsOutOfScope ?? false));
                var selectedItems = new List<LUMCloudBedTransactions>();
                foreach (var res in selectedRes)
                {
                    // 如果該Reservation 底下有Transaction

                    selectedItems.AddRange(GetTransacitonBelongToReservation(res?.ReservationID)
                                          .Where(x => string.IsNullOrEmpty(x.RemitRefNbr) &&
                                                      !(x.IsImported ?? false) &&
                                                      !(x.IsDeleted ?? false)));
                    UpdateReservationWithScope(res, false);
                }
                if (selectedItems.Count > 0)
                    ToggleInTransactions(selectedItems, false);
                else
                    this.Save.Press();
            });
            return adapter.Get();
        }

        /// <summary> Release Remit and Create GL </summary>
        public PXAction<LUMRemittance> Release;
        [PXButton]
        [PXUIField(DisplayName = "RELEASE", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable release(PXAdapter adapter)
        {
            bool isValid = true;
            #region Valid

            var errorReservations = this.NonValidCloudbedTransactions.View.SelectMulti()
                                        .RowCast<LUMCloudBedTransactions>()
                                        .Select(x => x.ReservationID).Distinct();

            // Valid Payment ADRemark
            foreach (var item in this.PaymentSummary.View.SelectMulti().RowCast<LUMRemitPayment>())
            {
                if (string.IsNullOrEmpty(item.ADRemark))
                {
                    this.PaymentSummary.Cache.RaiseExceptionHandling<LUMRemitPayment.aDRemark>(item, item.ADRemark,
                        new PXSetPropertyException<LUMRemitPayment.aDRemark>("ADRemark is required.", PXErrorLevel.Error));
                    isValid = false;
                }
            }
            // Valid Reservation ADRemark
            foreach (var item in this.ReservationSummary.View.SelectMulti().RowCast<LUMRemitReservation>())
            {
                if (string.IsNullOrEmpty(item.ADRemark))
                {
                    this.ReservationSummary.Cache.RaiseExceptionHandling<LUMRemitReservation.aDRemark>(item, item.ADRemark,
                        new PXSetPropertyException<LUMRemitReservation.aDRemark>("ADRemark is required.", PXErrorLevel.Error));
                    isValid = false;
                }
                if (errorReservations.Any(x => x == item.ReservationID))
                {
                    this.ReservationSummary.Cache.RaiseExceptionHandling<LUMRemitReservation.reservationID>(item, item.ReservationID,
                        new PXSetPropertyException<LUMRemitReservation.reservationID>("Account/Sub Account is mandatory, please complete necessary entry.", PXErrorLevel.Error));
                    isValid = false;
                }
            }
            // Valid Cloudbed Transaction Account/Sub-Account (Screen)
            foreach (var item in this.ReservationSummaryTransactions.View.SelectMulti().RowCast<LUMCloudBedTransactions2>().Where(x => x.RemitRefNbr == this.Document.Current.RefNbr))
            {
                if (!item.AccountID.HasValue)
                {
                    this.ReservationSummaryTransactions.Cache.RaiseExceptionHandling<LUMCloudBedTransactions2.accountID>(item, item.AccountID,
                        new PXSetPropertyException<LUMCloudBedTransactions2.accountID>("AccountID is required.", PXErrorLevel.Error));
                    isValid = false;
                }

                if (!item.SubAccountID.HasValue)
                {
                    this.ReservationSummaryTransactions.Cache.SetStatus(item, PXEntryStatus.Notchanged);
                    this.ReservationSummaryTransactions.Cache.RaiseExceptionHandling<LUMCloudBedTransactions2.subAccountID>(item, item.SubAccountID,
                       new PXSetPropertyException<LUMCloudBedTransactions2.subAccountID>("SubAccountID is required.", PXErrorLevel.Error));
                    isValid = false;
                }
            }

            if (!isValid)
                throw new PXException("Audit Remark and Account/Sub Account is mandatory, please complete necessary entry.");
            #endregion
            PXLongOperation.StartOperation(this, () =>
            {
                var selectedData = this.RemitTransactions.View.SelectMulti().RowCast<LUMCloudBedTransactions>();
                CreateJournalTransaction(this, selectedData.ToList());
                this.Save.Press();
            });
            return adapter.Get();
        }

        /// <summary> Void Remit and Void GL </summary>
        public PXAction<LUMRemittance> GoVoid;
        [PXButton]
        [PXUIField(DisplayName = "VOID", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable goVoid(PXAdapter adapter)
        {
            if (CurrentDocument.AskExt(true) != WebDialogResult.OK)
                return adapter.Get();
            var reason = this.CurrentDocument.Cache.Updated.RowCast<LUMRemittance>().FirstOrDefault()?.VoidReason;
            if (string.IsNullOrEmpty(reason))
                throw new PXException("Please specify void reason");
            PXLongOperation.StartOperation(this, () =>
            {
                using (PXTransactionScope sc = new PXTransactionScope())
                {
                    try
                    {
                        var doc = this.Document.Current;
                        var glGraph = PXGraph.CreateInstance<JournalEntry>();
                        glGraph.BatchModule.Current = glGraph.BatchModule.Search<Batch.batchNbr>(doc.BatchNbr, "GL");
                        if (glGraph.BatchModule.Current != null)
                        {
                            // Reverse Batch
                            glGraph.ReverseBatchProc(glGraph.BatchModule.Current);
                            glGraph.Save.Press();
                            glGraph.releaseFromHold.Press();
                            glGraph.release.Press();
                        }

                        // Update RemitRefNumber
                        PXUpdate<Set<LUMCloudBedTransactions.remitRefNbr, Required<LUMCloudBedTransactions.remitRefNbr>,
                                 Set<LUMCloudBedTransactions.isImported, Required<LUMCloudBedTransactions.isImported>>>,
                            LUMCloudBedTransactions,
                            Where<LUMCloudBedTransactions.remitRefNbr, Equal<Required<LUMCloudBedTransactions.remitRefNbr>>>>
                           .Update(this, null, false, doc?.RefNbr);

                        doc.VoidReason = reason;
                        doc.VoidedBy = Accessinfo.UserID;
                        doc.VoidBatchNbr = glGraph.BatchModule.Current?.BatchNbr;
                        doc.Status = LUMRemitStatus.Voided;
                        this.Document.UpdateCurrent();
                        this.Save.Press();
                        sc.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            });
            return adapter.Get();
        }

        public PXAction<LUMRemittance> FillInADRemark;
        [PXButton]
        [PXUIField(DisplayName = "AD REMARK", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable fillInADRemark(PXAdapter adapter)
        {
            if (this.CurrentDocument2.AskExt(true) != WebDialogResult.OK)
                return adapter.Get();
            var remark = this.CurrentDocument2.Cache.Updated.RowCast<LUMRemittance>().FirstOrDefault()?.ADRemark;
            foreach (var item in this.ReservationSummary.View.SelectMulti().RowCast<LUMRemitReservation>())
            {
                item.ADRemark = remark;
                this.ReservationSummary.Cache.Update(item);
            }
            return adapter.Get();
        }

        public PXAction<LUMRemittance> AccountDetermine;
        [PXButton]
        [PXUIField(DisplayName = "ACCOUNT DETERMINE", Enabled = false, Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable accountDetermine(PXAdapter adapter)
        {
            if (this.QuickAccountDetermine.AskExt(true) != WebDialogResult.OK)
                return adapter.Get();

            var selectedItem = this.ReservationSummaryTransactions.Cache.Updated.RowCast<LUMCloudBedTransactions2>().Where(x => x.Selected ?? false && x.RemitRefNbr == this.Document.Current?.RefNbr);
            foreach (var item in selectedItem)
            {
                item.AccountID = this.QuickAccountDetermine.Current?.AccountID;
                item.SubAccountID = this.QuickAccountDetermine.Current?.SubAccountID;
                this.ReservationSummaryTransactions.Update(item);
            }
            //this.Save.Press();
            return adapter.Get();
        }

        public PXAction<LUMRemittance> AccountAssign;
        [PXButton]
        [PXUIField(DisplayName = "ACCOUNT ASSIGN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable accountAssign(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                RemittanceHelper helper = new RemittanceHelper();
                var selectedTrans = this.FolioTransactioins.Cache.Updated.RowCast<LUMCloudBedTransactions3>().Where(x => x.Selected ?? false);
                // For current remit transactions whose account or sub is empty
                foreach (var item in selectedTrans.Where(x => !x.AccountID.HasValue || !x.SubAccountID.HasValue))
                {
                    var winnerAcctObj = CloudBedHelper.GetCloudbedAccountMappingWithScore(this, item, true);
                    if (winnerAcctObj != null)
                    {
                        item.AccountID = winnerAcctObj?.AccountID;
                        item.SubAccountID = winnerAcctObj?.SubAccountID;
                        this.FolioTransactioins.Update(item);
                    }
                }
                this.Save.Press();
            });
            return adapter.Get();
        }

        #region Audit Action

        /// <summary> AUDIT TOGGLE OUT transaction </summary>
        public PXAction<LUMRemittance> AuditPaymentToggleOut;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT TOGGLE OUT", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditPaymentToggleOut(PXAdapter adapter)
            => this.paymentToggleOut(adapter);

        /// <summary> AUDIT TOGGLE IN transaction </summary>
        public PXAction<LUMRemittance> AuditPaymentToggleIn;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT TOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditPaymentToggleIn(PXAdapter adapter)
            => this.paymentToggleIn(adapter);

        /// <summary> AUDIT TOGGLE OUT transaction(Reservation) </summary>
        public PXAction<LUMRemittance> AuditReservationToggleOut;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT TOGGLE OUT", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditReservationToggleOut(PXAdapter adapter)
            => this.reservationToggleOut(adapter);

        /// <summary> AUDIT TOGGLE IN transaction(Reservations) </summary>
        public PXAction<LUMRemittance> AuditReservationToggleIn;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT TOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditReservationToggleIn(PXAdapter adapter)
            => this.reservationToggleIn(adapter);

        /// <summary> AUDIT TOGGLE OUT transaction(Folio) </summary>
        public PXAction<LUMRemittance> AuditFolioToggleOut;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT TOGGLE OUT", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditFolioToggleOut(PXAdapter adapter)
            => this.reservationToggleOut(adapter);

        /// <summary> AUDIT TOGGLE IN transaction(Folio) </summary>
        public PXAction<LUMRemittance> AuditFolioToggleIn;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT TOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditFolioToggleIn(PXAdapter adapter)
            => this.reservationToggleIn(adapter);

        public PXAction<LUMRemittance> AuditRefresh;
        [PXButton]
        [PXUIField(DisplayName = "AUDIT PREPARE", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditRefresh(PXAdapter adapter)
            => this.refresh(adapter);

        #endregion

        #endregion

        #region Event

        public virtual void _(Events.RowSelecting<LUMRemittance> e)
        {
            if (e.Row is LUMRemittance row)
            {
                using (new PXConnectionScope())
                {
                    var setup = this.Setup.View.SelectSingle() as LUMHHSetup;
                    //If Approvals are enabled, check if the current user is the set approver
                    if (setup?.RemitRequestApproval ?? false)
                    {
                        row.IsApprover = CheckIsApprovalOwner(e.Row);
                        //EPEmployee approver = PXSelectJoin<EPEmployee,
                        //                      InnerJoin<EPRule, On<EPEmployee.userID, Equal<EPRule.ownerID>>,
                        //                      InnerJoin<EPAssignmentMap, On<EPRule.assignmentMapID, Equal<EPAssignmentMap.assignmentMapID>>>>,
                        //                      Where<EPAssignmentMap.assignmentMapID, Equal<Required<EPAssignmentMap.assignmentMapID>>>>.Select(this, leaveApproval.Current.AssignmentMapID);
                        //if (CheckIsApprovalOwner(e.Row))
                        //    row.IsApprover = (approver.UserID == Accessinfo.UserID);
                    }
                }
            }
        }

        public virtual void _(Events.RowSelected<LUMRemittance> e)
        {
            if (e.Row != null && e.Row is LUMRemittance row)
            {
                ButtonControl(row);

                FieldControl(row);

                if (!row.PostingDate.HasValue)
                    row.PostingDate = Accessinfo.BusinessDate;
            }
        }

        public virtual void _(Events.RowSelected<LUMCloudBedTransactions> e)
        {
            var row = e.Row;
            if (row != null)
            {
                var excludeTrans = this.ExculdeTransactions.Select().RowCast<LUMRemitExcludeTransactions>();
                row.GetExtension<LUMCloudBedTransactionsExt>().CurrentRefNbr = this.Document.Current?.RefNbr;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleByID = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedByID;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleDateTime = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedDateTime;
            }
        }

        public virtual void _(Events.RowSelected<LUMCloudBedTransactions2> e)
        {
            var row = e.Row;
            if (row != null)
            {

                if (row.RemitRefNbr != this.Document.Current?.RefNbr && !string.IsNullOrEmpty(row.RemitRefNbr))
                    PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.selected>(e.Cache, row, false);

                var excludeTrans = this.ExculdeTransactions.Select().RowCast<LUMRemitExcludeTransactions>();
                row.GetExtension<LUMCloudBedTransactionsExt>().CurrentRefNbr = this.Document.Current?.RefNbr;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleByID = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedByID;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleDateTime = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedDateTime;
                row.GetExtension<LUMCloudBedTransactionsExt>().CreditAmount = row?.TransactionType?.ToUpper() == "CREDIT" ? row?.Amount : 0;
                row.GetExtension<LUMCloudBedTransactionsExt>().DebitAmount = row?.TransactionType?.ToUpper() == "DEBIT" ? row?.Amount : 0;
            }
        }

        public virtual void _(Events.RowSelected<LUMCloudBedTransactions3> e)
        {
            var row = e.Row;
            if (row != null)
            {
                if (row.RemitRefNbr != this.Document.Current?.RefNbr && !string.IsNullOrEmpty(row.RemitRefNbr))
                    PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.selected>(e.Cache, row, false);

                var excludeTrans = this.ExculdeTransactions.Select().RowCast<LUMRemitExcludeTransactions>();
                row.GetExtension<LUMCloudBedTransactionsExt>().CurrentRefNbr = this.Document.Current?.RefNbr;
                row.GetExtension<LUMCloudBedTransactionsExt>().CreditAmount = row?.TransactionType?.ToUpper() == "CREDIT" ? row?.Amount : 0;
                row.GetExtension<LUMCloudBedTransactionsExt>().DebitAmount = row?.TransactionType?.ToUpper() == "DEBIT" ? row?.Amount : 0;
            }
        }

        public virtual void _(Events.FieldDefaulting<LUMRemitPayment.lineNbr> e)
        {
            var currentList = this.PaymentSummary.Select().RowCast<LUMRemitPayment>();
            var maxLineNbr = currentList.Count() == 0 ? 0 : currentList.Max(x => x?.LineNbr ?? 0);
            e.NewValue = maxLineNbr + 1;
        }

        public virtual void _(Events.FieldUpdated<LUMRemittance.hold> e)
        {
            if (e.Row != null && e.Row is LUMRemittance row)
            {
                if (!row.Hold ?? false)
                    row.Status = LUMRemitStatus.PendingApproval;
                else
                {
                    row.Status = LUMRemitStatus.Hold;
                    row.Rejected = false;
                    row.Approved = false;
                }
            }
        }

        public virtual void _(Events.FieldUpdated<LUMRemittance.status> e)
        {
            if (e.Row != null && e.Row is LUMRemittance row)
            {
                FieldControl(row);
            }
        }

        protected virtual void _(Events.RowInserting<EPApproval> e)
        {
            if (e.Row is EPApproval)
            {
                var doc = this.Document.Current;

                if (doc != null)
                {
                    // 寫入Approval 
                    e.Cache.SetValue<EPApproval.refNoteID>(e.Row, doc.Noteid);

                    var requestContactobj = GetContactObject();

                    if (requestContactobj != null)
                    {
                        e.Cache.SetValue<EPApproval.documentOwnerID>(e.Row, requestContactobj?.ContactID);
                        e.Cache.SetValue<EPApproval.bAccountID>(e.Row, requestContactobj?.BAccountID);
                    }
                    e.Cache.SetValue<EPApproval.docDate>(e.Row, this.Document.Current?.Date);
                }
            }
        }

        public virtual void _(Events.RowDeleting<LUMRemittance> e)
        {
            if (e.Row != null && e.Row is LUMRemittance row)
            {
                if (row.Status == LUMRemitStatus.Released || row.Status == LUMRemitStatus.Voided)
                    throw new Exception("Documents that are released or void cannot be deleted.");
            }
        }

        public virtual void _(Events.RowDeleted<LUMRemittance> e)
        {
            if (e.Row != null && e.Row is LUMRemittance row)
            {
                CleanupAllData(row?.RefNbr);
            }
        }

        #endregion

        #region Method
        /// <summary> 重新載入 Cloudbed API 資料</summary>
        public virtual void ReloadCloudBedData()
        {
            var cloudbedGraph = PXGraph.CreateInstance<LUMCloudBedTransactionProcess>();
            var watch = Stopwatch.StartNew();
            watch.Start();
            // Load Cloudbed transaction data
            cloudbedGraph.TransacionFilter.Current = cloudbedGraph.TransacionFilter.Cache.CreateInstance() as TransactionFilter;
            cloudbedGraph.TransacionFilter.Current.FromDate = Accessinfo.BusinessDate.Value.Date;
            cloudbedGraph.TransacionFilter.Current.ToDate = PX.Common.PXTimeZoneInfo.Now.AddDays(1).Date;
            cloudbedGraph.importTransactionData.Press();
            watch.Stop();
            PXTrace.WriteInformation($"Finfish Get ClodbedTransaction via API :{watch.ElapsedMilliseconds}");

            watch.Restart();
            // Load Cloudbed reservation data
            cloudbedGraph.ReservationFilter.Current = cloudbedGraph.ReservationFilter.Cache.CreateInstance() as ReservationFilter;
            cloudbedGraph.ReservationFilter.Current.ReservationFromDate = Accessinfo.BusinessDate.Value.Date;
            cloudbedGraph.ReservationFilter.Current.ReservationToDate = PX.Common.PXTimeZoneInfo.Now.AddDays(1).Date;
            cloudbedGraph.importReservationData.Press();
            watch.Stop();
            PXTrace.WriteInformation($"Finfish Get Reservation via API :{watch.ElapsedMilliseconds}");
        }

        /// <summary>
        /// Toggle Out Transactions
        /// </summary>
        /// <param name="selectedTransItems">所勾選的Transactions</param>
        /// <param name="_isScopeOut">是否OutOfScope</param>
        public virtual void ToggleOutTransactions(IEnumerable<LUMCloudBedTransactions> selectedTransItems, bool? _isScopeOut = null)
        {
            if (selectedTransItems.Count() == 0)
                return;
            InitialToggleData(selectedTransItems);

            using (PXTransactionScope sc = new PXTransactionScope())
            {
                /// 只執行RemitRefNbr = Current的交易
                foreach (var item in selectedTransItems)
                {
                    if (item?.RemitRefNbr != this.Document.Current?.RefNbr)
                        continue;
                    var excludeData = this.ExculdeTransactions.Cache.CreateInstance() as LUMRemitExcludeTransactions;
                    excludeData.RefNbr = this.Document.Current?.RefNbr;
                    excludeData.TransactionID = item?.TransactionID;
                    excludeData = this.ExculdeTransactions.Insert(excludeData);
                    this.ExculdeTransactions.Cache.PersistInserted(excludeData);
                    // Remove Remit RefNbr(Payment Amount).
                    UpdateTransactionRefNbr(item?.TransactionID, null);
                    // Recalcuate Remit Reservation
                    /// Houst Account的Resvsertion ID會是NULL,須往回找
                    ReCalculateRemitReservation(item, this.Document.Current?.RefNbr, item?.ReservationID ?? this.ReservationSummary.Current?.ReservationID, _isScopeOut, "OUT");
                    item.ReservationID = string.IsNullOrEmpty(item.ReservationID) ? CombineReservationByHouseAccount(item) : item.ReservationID;
                }

                //if (!this.ReservationDetails.View.SelectMulti().RowCast<LUMCloudBedTransactions2>().Any(x => x.RemitRefNbr == this.Document.Current?.RefNbr))
                //{
                //    if (this.ReservationTransactions.Current != null)
                //    {
                //        this.ReservationTransactions.Current.IsOutOfScope = true;
                //        this.ReservationTransactions.UpdateCurrent();
                //    }
                //}
                //FinalCheckScope(selectedTransItems.Select(x => x.ReservationID).Distinct().ToList(), true);

                InvokeCachePersist<LUMRemitReservation>(PXDBOperation.Update);
                InvokeCachePersist<LUMRemitPayment>(PXDBOperation.Update);
                InvokeCachePersist<LUMRemittance>(PXDBOperation.Update);
                sc.Complete();
            }
        }

        /// <summary> TOGGLE IN Cloudbed Transactions Data </summary>
        public virtual void ToggleInTransactions(IEnumerable<LUMCloudBedTransactions> selectedTransItems, bool? _isScopeOut = null)
        {
            if (selectedTransItems.Count() == 0)
                return;
            InitialToggleData(selectedTransItems);

            var tempTrans = new List<LUMCloudBedTransactions>();
            using (PXTransactionScope sc = new PXTransactionScope())
            {
                /// 只執行 RemitRefNbr = null的交易
                foreach (var item in selectedTransItems)
                {
                    if (!string.IsNullOrEmpty(item.RemitRefNbr))
                        continue;
                    var excludeData = new LUMRemitExcludeTransactions();
                    excludeData.RefNbr = this.Document.Current?.RefNbr;
                    excludeData.TransactionID = item?.TransactionID;
                    excludeData = (LUMRemitExcludeTransactions)this.ExculdeTransactions.Cache.Locate(excludeData);
                    if (excludeData != null)
                    {
                        this.ExculdeTransactions.Delete(excludeData);
                        this.ExculdeTransactions.Cache.PersistDeleted(excludeData);
                    }
                    // Add Remit RefNbr.
                    UpdateTransactionRefNbr(item?.TransactionID, this.Document.Current?.RefNbr);
                    // Recalcuate Remit Reservation
                    /// Houst Account的Resvsertion ID會是NULL,須往回找
                    ReCalculateRemitReservation(item, this.Document.Current?.RefNbr, item?.ReservationID ?? this.ReservationSummary.Current?.ReservationID, _isScopeOut, "IN");
                    item.ReservationID = string.IsNullOrEmpty(item.ReservationID) ? CombineReservationByHouseAccount(item) : item.ReservationID;
                    tempTrans.Add(item);
                }

                //FinalCheckScope(tempTrans.Select(x => x.ReservationID).Distinct().ToList(), false);
                InvokeCachePersist<LUMRemitReservation>(PXDBOperation.Update);
                InvokeCachePersist<LUMRemitPayment>(PXDBOperation.Update);
                InvokeCachePersist<LUMRemittance>(PXDBOperation.Update);
                sc.Complete();
            }
        }

        /// <summary> Add/Remote Cloudbed Transaction Remit RefNbr. & Recorded Amount </summary>
        private void UpdateTransactionRefNbr(string _transactionID, string _refNbr)
        {
            // Update/Remova Remit RefNbr.
            var trans = this.transWithFilter.FirstOrDefault(x => x.TransactionID == _transactionID);
            if (trans == null)
                throw new PXException($"Can not find Trans With Filter : {_transactionID}");
            trans.RemitRefNbr = _refNbr;
            trans = this.PropertyCloudbedTransactions.Update(trans);
            this.PropertyCloudbedTransactions.Cache.PersistUpdated(trans);

            // Update Payment-Check Recorded Amount
            var paymentTrans = this.PaymentSummary.View.SelectMulti().RowCast<LUMRemitPayment>();
            var currentPayment = paymentTrans.Where(x => x.Description == trans?.Description).FirstOrDefault();
            if (currentPayment != null)
            {
                currentPayment.RecordedAmt += string.IsNullOrEmpty(_refNbr) ? ((trans?.Amount ?? 0) * -1) : (trans?.Amount ?? 0);
                currentPayment = this.PaymentSummary.Update(currentPayment);
                //InvokeCachePersist<LUMRemitPayment>(PXDBOperation.Update);
                //this.PaymentTransactions.Cache.PersistUpdated(currentPayment);
            }
        }

        /// <summary> ReCalcuate Remit Reservation when trigger transation toggle on/in </summary>
        private void ReCalculateRemitReservation(LUMCloudBedTransactions selectedTrans, string _refNbr, string _reservationID, bool? _isScopeOut, string _actionType)
        {
            vHHRemitReservationCheck checkObj = new vHHRemitReservationCheck()
            {
                ReservationID = _reservationID,
                PropertyID = GetCurrentPropertyID(),
                Type = string.IsNullOrEmpty(_reservationID) ? "H" : "R"
            };
            var prefix = _actionType == "OUT" ? -1 : 1;
            var resLine = new LUMRemitReservation();
            resLine.RefNbr = _refNbr;
            resLine.ReservationID = checkObj.Type == "H" ? $"{selectedTrans?.HouseAccountID}-{selectedTrans?.HouseAccountName}" : _reservationID;
            resLine = this.ReservationSummary.Cache.Locate(resLine) as LUMRemitReservation;
            CalculateReservationAmount(selectedTrans, resLine, prefix);
            resLine.PendingCount = (resLine.PendingCount ?? 0) + (_actionType == "OUT" ? 1 : -1);
            resLine.ToRemitCount = (resLine.ToRemitCount ?? 0) + (_actionType == "OUT" ? -1 : 1);
            //resLine.IsOutOfScope = _actionType == "OUT" ? true : false;
            resLine = this.ReservationSummary.Update(resLine);

            // Recalculate Documaent Amount
            CalculateDocumentAmount(new List<LUMCloudBedTransactions>() { selectedTrans }, prefix);
        }

        /// <summary> Calculate Reservation Amount </summary>
        private void CalculateReservationAmount(string _refNbr, vHHRemitReservationCheck checkObj, LUMRemitReservation reservationobj)
        {
            if (checkObj == null)
                return;

            IEnumerable<LUMCloudBedTransactions> transByReservation = new List<LUMCloudBedTransactions>();
            transByReservation = GetTransacitonBelongToReservation(checkObj?.ReservationID);
            transByReservation.Where(x => ((x?.IsImported ?? false) || x?.RemitRefNbr == this.Document.Current?.RefNbr) && !(x.IsDeleted ?? false));
            switch (checkObj?.Type)
            {
                // Reservation
                case "R":
                case "RS":
                    // Pending Post Count of LUMRemitExcludeTransactions where (RefNbr = Current Remit AND Reservation = Current Reservation)
                    reservationobj.PendingCount = SelectFrom<LUMRemitExcludeTransactions>
                                                 .InnerJoin<LUMCloudBedTransactions>.On<LUMRemitExcludeTransactions.transactionID.IsEqual<LUMCloudBedTransactions.transactionID>
                                                       .And<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>>>
                                                 .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>
                                                 .View.Select(this, checkObj.ReservationID, _refNbr).Count;
                    reservationobj.ToRemitCount = SelectFrom<LUMCloudBedTransactions>
                                                 .Where<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>
                                                   .And<LUMCloudBedTransactions.remitRefNbr.IsEqual<P.AsString>>>
                                                 .View.Select(new PXGraph(), checkObj.ReservationID, _refNbr).Count;
                    break;
                // House Account
                case "H":
                    var _houstAccountID = checkObj?.ReservationID.Substring(0, checkObj.ReservationID.IndexOf('-'));
                    // Pending Post Count of LUMRemitExcludeTransactions where (RefNbr = Current Remit AND Reservation = Current Reservation)
                    reservationobj.PendingCount = SelectFrom<LUMRemitExcludeTransactions>
                                                 .InnerJoin<LUMCloudBedTransactions>.On<LUMRemitExcludeTransactions.transactionID.IsEqual<LUMCloudBedTransactions.transactionID>
                                                       .And<LUMCloudBedTransactions.houseAccountID.IsEqual<P.AsInt>>>
                                                 .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>
                                                 .View.Select(this, _houstAccountID, _refNbr).Count;
                    reservationobj.ToRemitCount = SelectFrom<LUMCloudBedTransactions>
                                                 .Where<LUMCloudBedTransactions.houseAccountID.IsEqual<P.AsInt>
                                                   .And<LUMCloudBedTransactions.remitRefNbr.IsEqual<P.AsString>>>
                                                 .View.Select(new PXGraph(), _houstAccountID, _refNbr).Count;
                    break;
            }
            foreach (var trans in transByReservation)
                CalculateReservationAmount(trans, reservationobj);
        }

        /// <summary> Calcaulete Reservation Amount according to Formular </summary>
        public virtual void CalculateReservationAmount(LUMCloudBedTransactions trans, LUMRemitReservation reservationobj, int prefix = 1)
        {
            // Room Revenue - TransactionType = 'Debit' AND Category = 'Room Revenue'
            if (trans?.TransactionType?.ToUpper() == "DEBIT" && trans?.Category?.ToUpper() == "ROOM REVENUE")
                reservationobj.RoomRevenue = (reservationobj.RoomRevenue ?? 0) + prefix * (trans?.Amount ?? 0);
            // Taxes - TransactionType = 'Debit' AND Category = 'Taxes'
            if (trans?.TransactionType?.ToUpper() == "DEBIT" && trans?.Category?.ToUpper() == "TAXES")
                reservationobj.Taxes = (reservationobj.Taxes ?? 0) + prefix * (trans?.Amount ?? 0);
            // Fees - TransactionType = 'Debit' AND Category = 'Fees'
            if (trans?.TransactionType?.ToUpper() == "DEBIT" && trans?.Category?.ToUpper() == "FEES")
                reservationobj.Fees = (reservationobj.Fees ?? 0) + prefix * (trans?.Amount ?? 0);
            // Others - TransactionType = 'Debit' AND Category NOT IN ('Room Revenue', 'Taxes', 'Fees')
            if (trans?.TransactionType?.ToUpper() == "DEBIT" && !new string[] { "ROOM REVENUE", "TAXES", "FEES" }.Contains(trans.Category))
                reservationobj.Others = (reservationobj.Others ?? 0) + prefix * (trans?.Amount ?? 0);
            // Payment - TransactionType = 'Credit'　
            if (trans?.TransactionType?.ToUpper() == "CREDIT")
                reservationobj.Payment = (reservationobj.Payment ?? 0) + prefix * (trans?.Amount ?? 0);
        }

        /// <summary> Calculate Document Amount (RoomRevenue..) </summary>
        public virtual void CalculateDocumentAmount(IEnumerable<LUMCloudBedTransactions> transactions, int prefix = 1)
        {
            var doc = this.Document.Current;
            if (doc != null)
            {
                foreach (var tran in transactions)
                {
                    var _type = tran?.TransactionType?.ToUpper();
                    var _category = tran?.Category?.ToUpper();
                    var _tranCategory = tran?.TransactionCategory?.ToUpper();
                    var _amount = tran?.Amount ?? 0;
                    //  TransactionType = 'Debit' AND Category = 'Room Revenue' AND Amount >= 0
                    if (_type == "DEBIT" && _category == "ROOM REVENUE" && _amount >= 0)
                        doc.RoomRevenue = (doc.RoomRevenue ?? 0) + prefix * _amount;
                    // TransactionType = 'Debit' AND Category = 'Room Revenue' AND Amount < 0
                    else if (_type == "DEBIT" && _category == "ROOM REVENUE" && _amount < 0)
                        doc.AdjRoomRevenue = (doc.AdjRoomRevenue ?? 0) + prefix * -1 * Math.Abs(_amount);
                    //  TransactionType = 'Debit' AND (Category = 'Taxes' OR Category = 'Fees') AND Amount > 0
                    else if (_type == "DEBIT" && (_category == "TAXES" || _category == "FEES") && _amount >= 0)
                        doc.Taxes = (doc.Taxes ?? 0) + prefix * _amount;
                    //  TransactionType = 'Debit' AND (Category = 'Taxes' OR Category = 'Fees') AND Amount < 0
                    else if (_type == "DEBIT" && (_category == "TAXES" || _category == "FEES") && _amount < 0)
                        doc.AdjTaxes = (doc.AdjTaxes ?? 0) + prefix * -1 * Math.Abs(_amount);
                    //  TransactionType = 'Debit' AND Category NOT IN ('Room Revenue', 'Taxes', 'Fees')  AND Amount > 0
                    else if (_type == "DEBIT" && !new string[] { "TAXES", "FEES" }.Contains(_category) && _amount >= 0)
                        doc.Other = (doc.Other ?? 0) + prefix * _amount;
                    //  TransactionType = 'Debit' AND Category NOT IN ('Room Revenue', 'Taxes', 'Fees')  AND Amount < 0
                    else if (_type == "DEBIT" && !new string[] { "TAXES", "FEES" }.Contains(_category) && _amount < 0)
                        doc.AdjOther = (doc.AdjOther ?? 0) + prefix * -1 * Math.Abs(_amount);
                    //  TransactionType = 'Credit'　AND TransactionCategory NOT IN ( 'refund', 'void') 
                    else if (_type == "CREDIT" && !(new string[] { "REFUND", "VOID" }.Contains(_tranCategory)))
                        doc.Payment = (doc.Payment ?? 0) + prefix * _amount;
                    //  TransactionType = 'Credit' AND TransactionCategory IN('refund', 'void')
                    else if (_type == "CREDIT" && new string[] { "REFUND", "VOID" }.Contains(_tranCategory))
                        doc.Refund = (doc.Refund ?? 0) + prefix * Math.Abs(_amount);

                    doc.CalcRoomRevenue = CalculateDifference(doc.RoomRevenue, Math.Abs(doc.AdjRoomRevenue ?? 0));
                    doc.CalcTaxes = CalculateDifference(doc.Taxes, Math.Abs(doc.AdjTaxes ?? 0));
                    doc.CalcOther = CalculateDifference(doc.Other, Math.Abs(doc.AdjOther ?? 0));
                }
                this.Document.UpdateCurrent();
            }
        }

        /// <summary> Get Cloudbed Transaction belong to Reservation </summary>
        public IEnumerable<LUMCloudBedTransactions> GetTransacitonBelongToReservation(string _reservationID)
        {
            IEnumerable<LUMCloudBedTransactions> transByReservation = new List<LUMCloudBedTransactions>();
            var propertyID = this.ClodBedPreference.Select().TopFirst.CloudBedPropertyID;
            var resType = _reservationID.Contains("-") ? "H" : "R";
            switch (resType)
            {
                case "R":
                    return transByReservation = SelectFrom<LUMCloudBedTransactions>
                                        .Where<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>
                                          .And<LUMCloudBedTransactions.propertyID.IsEqual<P.AsString>>>
                                        .View.Select(this, _reservationID, propertyID).RowCast<LUMCloudBedTransactions>();
                case "H":
                    string _houstAccountID = _reservationID.Substring(0, _reservationID.IndexOf('-'));
                    return transByReservation = SelectFrom<LUMCloudBedTransactions>
                                        .Where<LUMCloudBedTransactions.houseAccountID.IsEqual<P.AsInt>
                                          .And<LUMCloudBedTransactions.propertyID.IsEqual<P.AsString>>>
                                        .View.Select(this, _houstAccountID, propertyID).RowCast<LUMCloudBedTransactions>();
                default:
                    return transByReservation;
            }
        }

        /// <summary>
        /// Inital Toggle Process 所需資料
        /// </summary>
        /// <param name="selectedItems"></param>
        public virtual void InitialToggleData(IEnumerable<LUMCloudBedTransactions> selectedItems)
        {
            var watch = Stopwatch.StartNew(); //啟動Stopwatch

            // 用畫面所選的資料做過濾
            this.transWithFilter = SelectFrom<LUMCloudBedTransactions>
                                  .Where<LUMCloudBedTransactions.transactionID.IsIn<P.AsString>>
                                  .View.Select(this, new object[] { selectedItems.Select(x => x.TransactionID).ToArray() }).RowCast<LUMCloudBedTransactions>();

            watch.Stop();
            PXTrace.WriteInformation($"Get CloudbedTransaction Time:{watch.ElapsedMilliseconds}");
            watch.Restart();

            this.ReservationSummary.View.SelectMulti();
            watch.Stop();
            PXTrace.WriteInformation($"Get Reservation Time:{watch.ElapsedMilliseconds}");
            watch.Restart();
        }

        /// <summary> Create Journal Transaction </summary>
        public virtual void CreateJournalTransaction(LUMCloudBedRemitTransactionProcess baseGraph, List<LUMCloudBedTransactions> selectedData)
        {
            HHHelper helper = new HHHelper();
            var groupData = selectedData.Where(x => !(x.IsImported ?? false)).GroupBy(x => new { x.RemitRefNbr, x.Currency });
            var cloudbedProperty = SelectFrom<LUMCloudBedPreference>.View.Select(baseGraph).RowCast<LUMCloudBedPreference>();
            var ledgerInfo = helper.GetActualLedgerInfo();
            // 如果沒有任何Transaction 可以產生傳票
            if (groupData.Count() == 0)
            {
                // 回寫 Remit Document
                var remitDoc = baseGraph.Document.Current;
                remitDoc.Status = LUMRemitStatus.Released;
                baseGraph.Document.UpdateCurrent();
                baseGraph.Actions.PressSave();
            }
            foreach (var cloudBedGroupRow in groupData)
            {
                var glBatchNbr = string.Empty;
                string errorMsg = string.Empty;              // 整筆資料錯誤訊息
                var lineErrorDic = new Dictionary<string, string>();    // 每筆資料的錯誤訊息
                var lineNbrDic = new Dictionary<string, int?>();        // 每筆資料所對應到的GL Line

                using (PXTransactionScope sc = new PXTransactionScope())
                {
                    // 整筆資料錯誤Handler
                    try
                    {

                        #region Header
                        var glGraph = PXGraph.CreateInstance<JournalEntry>();
                        var doc = glGraph.BatchModule.Cache.CreateInstance() as Batch;
                        doc.Module = "GL";
                        doc = glGraph.BatchModule.Cache.Insert(doc) as Batch;
                        doc.DateEntered = baseGraph.Document.Current?.PostingDate;
                        doc.LedgerID = ledgerInfo?.LedgerID;
                        doc.BranchID = baseGraph.Document.Current?.Branch;
                        doc.Description = $"CloudBed Transaction {baseGraph.Document.Current?.Date?.ToString("yyyy-MM-dd")}";
                        doc.CuryID = cloudBedGroupRow.Key.Currency;
                        glGraph.BatchModule.Cache.Update(doc);
                        #endregion

                        #region Details
                        foreach (var row in cloudBedGroupRow)
                        {
                            var mapReservation = SelectFrom<LUMCloudBedReservations>
                                                .Where<LUMCloudBedReservations.propertyID.IsEqual<P.AsString>
                                                  .And<LUMCloudBedReservations.reservationID.IsEqual<P.AsString>>>
                                                .View.Select(baseGraph, row.PropertyID, row.ReservationID).TopFirst;
                            var mapCloudbedProerty = baseGraph.ClodBedPreference.View.SelectMulti().FirstOrDefault() as LUMCloudBedPreference;
                            errorMsg = string.Empty;
                            // Set CurrentItem
                            PXProcessing.SetCurrentItem(row);
                            try
                            {
                                #region RuleA
                                var line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
                                line.AccountID = row?.AccountID;
                                line.SubID = row?.SubAccountID;
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
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                errorMsg = $"Error: {ex.Message}";
                                throw ex;
                            }
                            finally
                            {
                                row.IsImported = string.IsNullOrEmpty(errorMsg) ? true : false;
                                row.ErrorMessage = errorMsg;
                                baseGraph.PropertyCloudbedTransactions.Update(row);
                            }
                        }
                        #endregion

                        errorMsg = string.Empty;
                        glGraph.releaseFromHold.Press();
                        glGraph.Save.Press();
                        glGraph.release.Press();
                        glBatchNbr = doc.BatchNbr;

                        // 回寫Cloudbed Transaciton BatchNbr.
                        cloudBedGroupRow.ToList().ForEach(x =>
                        {
                            x.BatchNbr = (x.IsImported ?? false) ? glBatchNbr : string.Empty;
                            x.LineNbr = (x?.IsImported ?? false) ? lineNbrDic[x.TransactionID] : null;
                            baseGraph.PropertyCloudbedTransactions.Update(x);
                        });
                        // 回寫 Remit Document
                        var remitDoc = baseGraph.Document.Current;
                        remitDoc.BatchNbr = glBatchNbr;
                        remitDoc.Status = LUMRemitStatus.Released;
                        baseGraph.Document.UpdateCurrent();
                        // Save Process Result
                        baseGraph.Actions.PressSave();
                        sc.Complete();
                    }
                    catch (PXOuterException ex)
                    {
                        errorMsg = $"Error: {ex.InnerMessages[0]}";
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        errorMsg = $"Error: {ex.Message}";
                        throw ex;
                    }
                }// end of transaction scrope
            }

        }

        /// <summary> 檢查是不是審核人 </summary>
        public bool CheckIsApprovalOwner(LUMRemittance doc)
        {
            var approvalRecord = SelectFrom<EPApproval>
                                 .Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>
                                 .View.Select(this, doc.Noteid).RowCast<EPApproval>().FirstOrDefault(x => x.Status == LUMRemitStatus.PendingApproval);
            if (approvalRecord != null)
            {
                BAccount acct = SelectFrom<BAccount>
                                .InnerJoin<EPEmployee>.On<BAccount.bAccountID.IsEqual<EPEmployee.bAccountID>>
                                .Where<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>>.View.Select(this);
                if (acct.DefContactID == approvalRecord.OwnerID)
                    return true;
                return false;
            }
            return false;
        }

        /// <summary> 手動更新Status(多階段審核) </summary>
        public void UpdateStatusMaunal(LUMRemittance doc)
        {
            if (SelectFrom<EPApproval>
               .Where<EPApproval.status.IsEqual<P.AsString>
                    .And<EPApproval.refNoteID.IsEqual<P.AsGuid>>>
               .View.Select(this, LUMRemitStatus.PendingApproval, doc.Noteid).Count > 0 && doc.Status == LUMRemitStatus.Open)
            {
                PXUpdate<Set<LUMRemittance.approved, Required<LUMRemittance.approved>,
                         Set<LUMRemittance.status, Required<LUMRemittance.status>>>,
                         LUMRemittance,
                         Where<LUMRemittance.refNbr, Equal<Required<LUMRemittance.refNbr>>>>.Update(this, false, LUMRemitStatus.PendingApproval, doc.RefNbr);

                doc.Status = LUMRemitStatus.PendingApproval;
            }
        }

        public void RemoveApprovalHistory(LUMRemittance doc)
            => PXDatabase.Delete<EPApproval>(new PXDataFieldRestrict<EPApproval.refNoteID>(doc?.Noteid));

        /// <summary> Get Current PropertyID </summary>
        private string GetCurrentPropertyID()
            => this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;

        /// <summary> Invoke Cache Persist </summary>
        private void InvokeCachePersist<T>(PXDBOperation operation) where T : class, IBqlTable, new()
            => this.Caches<T>().Persist(operation);

        /// <summary> Control All Button according to Status </summary>
        private void ButtonControl(LUMRemittance row)
        {
            #region Hidden all button
            // Hidden all button
            this.PaymentToggleIn.SetEnabled(false);
            this.PaymentToggleOut.SetEnabled(false);
            this.ReservationToggleIn.SetEnabled(false);
            this.ReservationToggleOut.SetEnabled(false);
            this.AuditPaymentToggleIn.SetEnabled(false);
            this.AuditPaymentToggleOut.SetEnabled(false);
            this.AuditReservationToggleIn.SetEnabled(false);
            this.AuditReservationToggleOut.SetEnabled(false);
            this.FillInADRemark.SetEnabled(false);
            this.AccountDetermine.SetEnabled(false);
            this.AuditRefresh.SetVisible(false);
            this.ReleaseFromHold.SetVisible(false);
            this.Refresh.SetVisible(false);
            this.Release.SetVisible(false);
            this.GoVoid.SetVisible(false);
            this.InScope.SetEnabled(false);
            this.OutOfScope.SetEnabled(false);
            this.Approve.SetVisible(false);
            this.Reject.SetVisible(false);
            this.Hold.SetVisible(false);
            #endregion

            this.Sync.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Info);

            switch (row?.Status)
            {
                case LUMRemitStatus.Hold:
                    if (row.RefNbr != "<NEW>")
                    {
                        this.ReleaseFromHold.SetVisible(true);
                        this.ReleaseFromHold.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Success);

                        this.Refresh.SetVisible(true);
                        this.Refresh.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Info);

                        this.PaymentToggleIn.SetEnabled(true);
                        this.PaymentToggleOut.SetEnabled(true);

                        this.ReservationToggleIn.SetEnabled(true);
                        this.ReservationToggleOut.SetEnabled(true);

                        this.InScope.SetEnabled(true);
                        this.OutOfScope.SetEnabled(true);

                        this.FillInADRemark.SetEnabled(true);
                        this.AccountDetermine.SetEnabled(true);
                    }
                    break;
                case LUMRemitStatus.PendingApproval:
                    if (this.Setup.Current?.RemitRequestApproval ?? false)
                    {
                        this.Hold.SetVisible(true);

                        this.Approve.SetVisible(true);
                        this.Reject.SetVisible(true);

                        this.Approve.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Success);
                        this.Reject.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Warning);
                    }
                    break;
                case LUMRemitStatus.Open:

                    this.AuditPaymentToggleIn.SetEnabled(true);
                    this.AuditPaymentToggleOut.SetEnabled(true);

                    this.AuditReservationToggleIn.SetEnabled(true);
                    this.AuditReservationToggleOut.SetEnabled(true);

                    this.AuditRefresh.SetVisible(true);

                    this.Hold.SetVisible(true);

                    this.FillInADRemark.SetEnabled(true);
                    this.AccountDetermine.SetEnabled(true);

                    this.Release.SetVisible(true);
                    this.Release.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Success);
                    break;
                case LUMRemitStatus.Released:
                    this.GoVoid.SetVisible(true);
                    this.GoVoid.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Warning);
                    break;
                case LUMRemitStatus.Voided:
                    break;
                case LUMRemitStatus.Rejected:
                    this.Hold.SetVisible(true);
                    break;
            }
        }

        /// <summary> Control All Field according </summary>
        private void FieldControl(LUMRemittance row)
        {
            #region Set Field Enabled (false)
            PXUIFieldAttribute.SetEnabled<LUMRemittance.ownerID>(this.Document.Cache, null, false);

            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.transactionID>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.reservationID>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.houseAccountName>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.guestName>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.amount>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.userName>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.transactionNotes>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.createdDateTime>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.remitRefNbr>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.isImported>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.batchNbr>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.lineNbr>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactionsExt.toggleByID>(this.PaymentSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactionsExt.toggleDateTime>(this.PaymentSummaryTransactions.Cache, null, false);

            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.transactionID>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.remitRefNbr>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.description>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.transactionNotes>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.userName>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.isImported>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.batchNbr>(this.ReservationSummaryTransactions.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.lineNbr>(this.ReservationSummaryTransactions.Cache, null, false);
            #endregion

            switch (row?.Status)
            {
                case LUMRemitStatus.Hold:
                    PXUIFieldAttribute.SetEnabled<LUMRemittance.ownerID>(this.Document.Cache, null, true);
                    break;
                case LUMRemitStatus.PendingApproval:
                    PXUIFieldAttribute.SetEnabled<LUMRemitPayment.oPRemark>(this.PaymentSummary.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMRemitReservation.oPRemark>(this.PaymentSummary.Cache, null, false);
                    break;
                case LUMRemitStatus.Open:
                    PXUIFieldAttribute.SetEnabled<LUMRemitPayment.remitAmt>(this.PaymentSummary.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMRemitPayment.oPRemark>(this.PaymentSummary.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMRemitReservation.oPRemark>(this.PaymentSummary.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMRemitBlock.oPRemark>(this.RemitBlock.Cache, null, false);
                    break;
                case LUMRemitStatus.Released:
                case LUMRemitStatus.Voided:
                    PXUIFieldAttribute.SetEnabled<LUMRemittance.postingDate>(this.Document.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMRemitPayment.oPRemark>(this.PaymentSummary.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMRemitReservation.oPRemark>(this.PaymentSummary.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.accountID>(this.PaymentSummaryTransactions.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.subAccountID>(this.PaymentSummaryTransactions.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.accountID>(this.ReservationSummaryTransactions.Cache, null, false);
                    PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.subAccountID>(this.ReservationSummaryTransactions.Cache, null, false);
                    break;
            }

            if (this.PaymentSummary.View.SelectMulti().Count > 0 || this.ReservationSummary.View.SelectMulti().Count > 0)
            {
                PXUIFieldAttribute.SetEnabled<LUMRemittance.date>(this.Document.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<LUMRemittance.shift>(this.Document.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<LUMRemittance.branch>(this.Document.Cache, null, false);
            }
        }

        /// <summary> 刪除與此Remit 有關的所有資料 </summary>
        private void CleanupAllData(string _RefNbr)
        {
            PXDatabase.Delete<LUMRemitPayment>(new PXDataFieldRestrict<LUMRemitPayment.refNbr>(_RefNbr));
            PXDatabase.Delete<LUMRemitReservation>(new PXDataFieldRestrict<LUMRemitReservation.refNbr>(_RefNbr));
            PXDatabase.Delete<LUMRemitExcludeTransactions>(new PXDataFieldRestrict<LUMRemitExcludeTransactions.refNbr>(_RefNbr));
            PXDatabase.Update<LUMCloudBedTransactions>(
                new PXDataFieldAssign<LUMCloudBedTransactions.remitRefNbr>(null),
                new PXDataFieldRestrict<LUMCloudBedTransactions.remitRefNbr>(_RefNbr));
        }

        /// <summary>
        /// 更新Reservation 中的 OutOfScope欄位
        /// </summary>
        /// <param name="resLine"></param>
        /// <param name="_isScopeOut"></param>
        private void UpdateReservationWithScope(LUMRemitReservation resLine, bool? _isScopeOut)
        {
            resLine.IsOutOfScope = _isScopeOut;
            resLine = this.ReservationSummary.Update(resLine);
        }

        public Contact GetContactObject()
            => this.Document.Current != null ? SelectFrom<Contact>.Where<Contact.contactID.IsEqual<P.AsInt>>.View.Select(this, this.Document.Current?.OwnerID) : null;

        public EPEmployee GetRequestEmployeeObject()
            => this.Document.Current != null ? SelectFrom<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<P.AsInt>>.View.Select(this, GetContactObject()?.BAccountID) : null;

        public string GetHouseAccountByReservationID(string reservationID)
        {
            int dashIndex = reservationID.IndexOf('-');
            return dashIndex != -1 ? reservationID.Substring(0, dashIndex) : "0";
        }

        public string CombineReservationByHouseAccount(LUMCloudBedTransactions trans)
            => $"{trans.HouseAccountID}-{trans.HouseAccountName}";

        /// <summary>
        /// 檢查Reservation下的"交易"是否還在Scope內
        /// </summary>
        /// <param name="reservationIDs"></param>
        /// <param name="Scope"></param>
        public void FinalCheckScope(List<string> reservationIDs, bool Scope)
        {
            foreach (var resvID in reservationIDs)
            {
                var trans = !resvID.Contains("-") ?
                            SelectFrom<LUMCloudBedTransactions>
                           .Where<LUMCloudBedTransactions.remitRefNbr.IsEqual<P.AsString>
                            .And<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>>>
                            .View.Select(this, this.Document.Current?.RefNbr, resvID) :
                            SelectFrom<LUMCloudBedTransactions>
                           .Where<LUMCloudBedTransactions.remitRefNbr.IsEqual<P.AsString>
                            .And<LUMCloudBedTransactions.houseAccountID.IsEqual<P.AsInt>>>
                            .View.Select(this, this.Document.Current?.RefNbr, GetHouseAccountByReservationID(resvID));
                var resLine = this.ReservationSummary.View.SelectMulti().RowCast<LUMRemitReservation>().FirstOrDefault(x => x.ReservationID == resvID);
                if (resLine != null)
                {
                    resLine.IsOutOfScope = trans.Count == 0 ? true : false;
                    this.ReservationSummary.Update(resLine);
                }
            }
        }

        private decimal CalculateDifference(decimal? value, decimal? adjustment)
        {
            return (value ?? 0) - (adjustment ?? 0);
        }

        private void CleanUpDocumentAmount()
        {
            var doc = this.Document.Current;
            doc.RoomRevenue = 0;
            doc.AdjRoomRevenue = 0;
            doc.CalcRoomRevenue = 0;
            doc.Taxes = 0;
            doc.AdjTaxes = 0;
            doc.CalcTaxes = 0;
            doc.Other = 0;
            doc.AdjOther = 0;
            doc.CalcOther = 0;
            doc.Payment = 0;
            doc.Refund = 0;
            this.Document.UpdateCurrent();
        }

        /// <summary> 計算Folio的排序 </summary>
        private int CalculateSortNumber(LUMCloudBedTransactions3 item, string currentRemit, IEnumerable<LUMCloudBedTransactions3> toggleOutTrans)
        {
            // Transaction RemitRefnbr = Current Remit
            if (item?.RemitRefNbr == currentRemit)
                return 1;
            // Transaction toggle out, and current Remit empty
            else if (string.IsNullOrEmpty(item?.RemitRefNbr) && toggleOutTrans.Contains(item))
                return 2;
            // Transaction where remit refnbr is null and isimported <> true
            else if (string.IsNullOrEmpty(item?.RemitRefNbr) && !(item?.IsImported ?? false))
                return 3;
            else if (!string.IsNullOrEmpty(item?.RemitRefNbr))
                return 4;
            return 5;
        }

        #endregion

    }

    #region Virtual DAC for Remittance

    public class LUMCloudBedTransactionsExt : PXCacheExtension<LUMCloudBedTransactions>
    {
        #region ToRemit
        [PXBool]
        [PXFormula(typeof(Switch<Case<Where<LUMCloudBedTransactions.remitRefNbr, Equal<Current<LUMRemittance.refNbr>>>, True>, False>))]
        [PXUIField(DisplayName = "To Remit", Enabled = false)]
        public virtual bool? ToRemit { get; set; }
        public abstract class toRemit : PX.Data.BQL.BqlBool.Field<toRemit> { }
        #endregion

        #region ToggleByID
        [PXGuid]
        [PXSelector(typeof(Search<Users.pKID>),
                    SubstituteKey = typeof(Users.fullName))]
        [PXUIField(DisplayName = "Toggled Out User", Enabled = false)]
        public virtual Guid? ToggleByID { get; set; }
        public abstract class toggleByID : PX.Data.BQL.BqlGuid.Field<toggleByID> { }
        #endregion

        #region ToggleDateTime
        [PXDate]
        [PXUIField(DisplayName = "Toggled Out Date Time", Enabled = false)]
        public virtual DateTime? ToggleDateTime { get; set; }
        public abstract class toggleDateTime : PX.Data.BQL.BqlDateTime.Field<toggleDateTime> { }
        #endregion

        #region DebitAmount
        [PXDecimal]
        [PXUIField(DisplayName = "Debit Amount", Enabled = false)]
        public virtual decimal? DebitAmount { get; set; }
        public abstract class debitAmount : PX.Data.BQL.BqlDecimal.Field<debitAmount> { }
        #endregion

        #region CreditAmount
        [PXDecimal]
        [PXUIField(DisplayName = "Credit Amount", Enabled = false)]
        public virtual decimal? CreditAmount { get; set; }
        public abstract class creditAmount : PX.Data.BQL.BqlDecimal.Field<creditAmount> { }
        #endregion

        #region CurrentRefNbr
        [PXString]
        [PXDefault(typeof(LUMRemittance.refNbr), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Current Document RefNbr", Enabled = false, Visible = false)]
        public virtual string CurrentRefNbr { get; set; }
        public abstract class currentRefNbr : PX.Data.BQL.BqlString.Field<currentRefNbr> { }
        #endregion

    }

    [Serializable]
    [PXCacheName("LUMCloudBedTransactions2")]
    public class LUMCloudBedTransactions2 : LUMCloudBedTransactions
    {

        #region Selected
        public new abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region IsImported
        public new abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region BatchNbr
        public new abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }
        #endregion

        #region PropertyID
        public new abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region ReservationID
        public new abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region GuestName
        public new abstract class guestName : PX.Data.BQL.BqlString.Field<guestName> { }
        #endregion

        #region Description
        public new abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Category
        public new abstract class category : PX.Data.BQL.BqlString.Field<category> { }
        #endregion

        #region TransactionNotes
        public abstract class transactionNotes : PX.Data.BQL.BqlString.Field<transactionNotes> { }
        #endregion

        #region Amount
        public new abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region UserName
        public abstract class userName : PX.Data.BQL.BqlString.Field<userName> { }
        #endregion

        #region TransactionType
        public new abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region TransactionCategory
        public new abstract class transactionCategory : PX.Data.BQL.BqlString.Field<transactionCategory> { }
        #endregion

        #region ItemCategoryName
        public new abstract class itemCategoryName : PX.Data.BQL.BqlString.Field<itemCategoryName> { }
        #endregion

        #region TransactionID
        public new abstract class transactionID : PX.Data.BQL.BqlString.Field<transactionID> { }
        #endregion
    }

    [Serializable]
    [PXCacheName("LUMCloudBedTransactions3")]
    public class LUMCloudBedTransactions3 : LUMCloudBedTransactions
    {

        #region SortNumber
        [PXInt]
        public virtual int? SortNumber { get; set; }
        public abstract class sortNumber : PX.Data.BQL.BqlInt.Field<sortNumber> { }
        #endregion

        #region Selected
        public new abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region IsImported
        public new abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region BatchNbr
        public new abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }
        #endregion

        #region PropertyID
        public new abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region ReservationID
        public new abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region GuestName
        public new abstract class guestName : PX.Data.BQL.BqlString.Field<guestName> { }
        #endregion

        #region Description
        public new abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Category
        public new abstract class category : PX.Data.BQL.BqlString.Field<category> { }
        #endregion

        #region TransactionNotes
        public abstract class transactionNotes : PX.Data.BQL.BqlString.Field<transactionNotes> { }
        #endregion

        #region Amount
        public new abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region UserName
        public abstract class userName : PX.Data.BQL.BqlString.Field<userName> { }
        #endregion

        #region TransactionType
        public new abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region TransactionCategory
        public new abstract class transactionCategory : PX.Data.BQL.BqlString.Field<transactionCategory> { }
        #endregion

        #region ItemCategoryName
        public new abstract class itemCategoryName : PX.Data.BQL.BqlString.Field<itemCategoryName> { }
        #endregion

        #region TransactionID
        public new abstract class transactionID : PX.Data.BQL.BqlString.Field<transactionID> { }
        #endregion
    }

    [Serializable]
    public class LUMShowSystemPostFilter : PXBqlTable, IBqlTable
    {
        #region ShowPost
        [PXBool]
        [PXUIField(DisplayName = "SHOW SYSTEM TRANS")]
        public virtual bool? ShowPost { get; set; }
        public abstract class showPost : PX.Data.BQL.BqlBool.Field<showPost> { }
        #endregion
    }

    [Serializable]
    public class LUMBatchUpdateAcctFilter : PXBqlTable, IBqlTable
    {
        #region AccountID
        [PXInt()]
        [PXDefault()]
        [PXUIField(DisplayName = "Account ID")]
        [PXSelector(typeof(Search<Account.accountID>),
                    typeof(Account.accountCD),
                    typeof(Account.description),
                    SubstituteKey = typeof(Account.accountCD))]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubAccountID
        [PXInt()]
        [PXDefault()]
        [PXUIField(DisplayName = "Sub Account ID")]
        [PXSelector(typeof(Search<Sub.subID>),
                    typeof(Sub.subCD),
                    typeof(Sub.description),
                    SubstituteKey = typeof(Sub.subCD))]
        public virtual int? SubAccountID { get; set; }
        public abstract class subAccountID : PX.Data.BQL.BqlInt.Field<subAccountID> { }
        #endregion
    }

    #endregion

}
