using HH_APICustomization.APIHelper;
using HH_APICustomization.DAC;
using Newtonsoft.Json.Linq;
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
using HH_APICustomization.Descriptor;

namespace HH_APICustomization.Graph
{
    /// <summary> ScreenID = LM505000.aspx</summary>
    public class LUMCloudBedTransactionProcess : PXGraph<LUMCloudBedTransactionProcess>
    {
        #region Constants
        private const int MAX_REFNBR_LENGTH = 15;
        private const int MAX_TRANDESC_LENGTH = 512;
        private const int MAX_NOTES_LENGTH = 1024;
        private const int MAX_RESPONSE_LENGTH = 2048;
        private const int TIMEZONE_OFFSET_HOURS = 8;
        private const string TRANS_TYPE_CREDIT = "CREDIT";
        private const string TRANS_TYPE_DEBIT = "DEBIT";
        private const string TRANS_CATEGORY_REFUND = "REFUND";
        private const string GL_MODULE = "GL";
        #endregion

        #region Views
        public PXSave<TransactionFilter> Save;
        public PXCancel<TransactionFilter> Cancel;

        public PXFilter<TransactionFilter> TransacionFilter;
        public PXFilter<ReservationFilter> ReservationFilter;

        public SelectFrom<LUMCloudBedTransactions>
              .Where<Brackets<LUMCloudBedTransactions.remitRefNbr.IsNull>.And<LUMCloudBedTransactions.isImported.IsNotEqual<True>>
                .And<Brackets<LUMCloudBedTransactions.propertyID.IsEqual<TransactionFilter.cloudBedPropertyID.FromCurrent>.Or<TransactionFilter.cloudBedPropertyID.FromCurrent.IsNull>>>>
              .ProcessingView.FilteredBy<TransactionFilter> Transaction;

        public SelectFrom<LUMCloudBedTransactions>.View CurrentTransaction;

        public SelectFrom<LUMCloudBedReservations>
              .Where<Brackets<LUMCloudBedReservations.propertyID.IsEqual<ReservationFilter.cloudBedPropertyID.FromCurrent>.Or<ReservationFilter.cloudBedPropertyID.FromCurrent.IsNull>>>
              .View Reservations;

        public SelectFrom<LUMCloudBedRoomAssignment>.View RoomAssignment;

        public SelectFrom<LUMCloudBedRoomRateDetails>.View RoomRateDetails;
        #endregion

        #region Constructor
        public LUMCloudBedTransactionProcess()
        {
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.isImported>(Transaction.Cache, null, true);
            var filter = this.TransacionFilter.Current;
            var reservationFilter = this.ReservationFilter.Current;
            Transaction.SetProcessDelegate(delegate (List<LUMCloudBedTransactions> list)
            {
                GoProcessing(list, filter, reservationFilter);
            });
        }
        #endregion

        #region Actions
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

        #region Events
        public virtual void _(Events.RowSelected<LUMCloudBedTransactions> e)
        {
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.amount>(e.Cache, null, !(e.Row?.IsImported ?? false));
        }

        public virtual void _(Events.FieldDefaulting<TransactionFilter.fromDate> e)
        {
            var row = e.Row as TransactionFilter;
            if (!row.FromDate.HasValue)
                row.FromDate = DateTime.Now.AddDays(-1);
        }

        public virtual void _(Events.FieldDefaulting<TransactionFilter.toDate> e)
        {
            var row = e.Row as TransactionFilter;
            if (!row.ToDate.HasValue)
                row.ToDate = DateTime.Now;
        }

        public virtual void _(Events.FieldDefaulting<ReservationFilter.reservationFromDate> e)
        {
            var row = e.Row as ReservationFilter;
            if (!row.ReservationFromDate.HasValue)
                row.ReservationFromDate = PX.Common.PXTimeZoneInfo.Now;
        }

        public virtual void _(Events.FieldDefaulting<ReservationFilter.reservationToDate> e)
        {
            var row = e.Row as ReservationFilter;
            if (!row.ReservationToDate.HasValue)
                row.ReservationToDate = PX.Common.PXTimeZoneInfo.Now.AddHours(1);
        }
        #endregion

        #region Main Processing Methods
        /// <summary> Main processing entry point </summary>
        public static void GoProcessing(List<LUMCloudBedTransactions> selectedData, TransactionFilter transactionFilter, ReservationFilter reservationFilter)
        {
            var baseGraph = PXGraph.CreateInstance<LUMCloudBedTransactionProcess>();
            switch (transactionFilter.ProcessType)
            {
                case "ImportTransaction":
                    GetCloudBedTransactionData(baseGraph, transactionFilter);
                    break;
                case "ImportReservation":
                    GetCloudBedReservationData(baseGraph, reservationFilter);
                    break;
                case "CreateJournalTransaction":
                    baseGraph.CreateJournalTransaction(baseGraph, selectedData);
                    break;
            }
        }

        /// <summary> Create GL Journal Transactions from CloudBed data </summary>
        public virtual void CreateJournalTransaction(LUMCloudBedTransactionProcess baseGraph, List<LUMCloudBedTransactions> selectedData)
        {
            HHHelper helper = new HHHelper();

            // Group transactions by Date, Property, and Currency
            var groupedData = selectedData
                .Where(x => !(x.IsImported ?? false))
                .GroupBy(x => new { x.TransactionDateTime.Value.Date, x.PropertyID, x.Currency });

            // Load reference data once
            var cloudBedProperties = SelectFrom<LUMCloudBedPreference>.View.Select(baseGraph).RowCast<LUMCloudBedPreference>();
            var reservations = baseGraph.Reservations.Select().RowCast<LUMCloudBedReservations>();
            var ledgerInfo = helper.GetActualLedgerInfo();

            foreach (var transactionGroup in groupedData)
            {
                ProcessTransactionGroup(baseGraph, transactionGroup, cloudBedProperties, reservations, ledgerInfo);
            }
        }

        /// <summary> Process a group of transactions to create a single GL batch </summary>
        private void ProcessTransactionGroup(
            LUMCloudBedTransactionProcess baseGraph,
            IGrouping<dynamic, LUMCloudBedTransactions> transactionGroup,
            IEnumerable<LUMCloudBedPreference> cloudBedProperties,
            IEnumerable<LUMCloudBedReservations> reservations,
            Ledger ledgerInfo)
        {
            var glBatchNbr = string.Empty;
            string errorMsg = string.Empty;
            var property = cloudBedProperties.FirstOrDefault(x => x.CloudBedPropertyID == transactionGroup.Key.PropertyID);
            var lineNbrDictionary = new Dictionary<string, int?>();

            try
            {
                // Create GL Batch
                var glGraph = PXGraph.CreateInstance<JournalEntry>();
                var batch = CreateGLBatch(glGraph, transactionGroup.Key, property, ledgerInfo);

                // Process each transaction
                foreach (var transaction in transactionGroup)
                {
                    errorMsg = string.Empty;
                    PXProcessing.SetCurrentItem(transaction);

                    try
                    {
                        ProcessSingleTransaction(glGraph, transaction, property, reservations, lineNbrDictionary);
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
                        UpdateTransactionStatus(baseGraph, transaction, errorMsg);
                    }
                }

                // Save GL Batch if at least one transaction succeeded
                if (transactionGroup.Any(x => x.IsImported ?? false))
                {
                    errorMsg = string.Empty;
                    glGraph.releaseFromHold.Press();
                    glGraph.Save.Press();
                    glBatchNbr = batch.BatchNbr;
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
                UpdateGroupProcessingStatus(baseGraph, transactionGroup, errorMsg, glBatchNbr, lineNbrDictionary);
                baseGraph.Actions.PressSave();
            }
        }

        /// <summary> Process a single transaction and create GL lines </summary>
        private void ProcessSingleTransaction(
            JournalEntry glGraph,
            LUMCloudBedTransactions transaction,
            LUMCloudBedPreference property,
            IEnumerable<LUMCloudBedReservations> reservations,
            Dictionary<string, int?> lineNbrDictionary)
        {
            // Validate reservation exists if ReservationID is valid
            long validReservation;
            var reservation = reservations.FirstOrDefault(x => x.PropertyID == transaction.PropertyID && x.ReservationID == transaction.ReservationID);
            if (reservation == null && long.TryParse(transaction.ReservationID, out validReservation))
                throw new PXException("Can not Mapping Reservation Data!!");

            // Get account mapping
            var accountMapping = CloudBedHelper.GetCloudbedAccountMappingWithScore(this, transaction, true);

            // Prepare common line data
            var refNbr = TruncateString(string.IsNullOrEmpty(transaction?.ReservationID) ? transaction.HouseAccountID.ToString() : transaction.ReservationID, MAX_REFNBR_LENGTH);
            var tranDesc = BuildTransactionDescription(transaction, reservation);

            // Create Rule A: Debit/Credit account from mapping
            var lineA = CreateGLTranLine(
                glGraph,
                accountMapping?.AccountID,
                accountMapping?.SubAccountID,
                accountMapping?.BranchID,
                refNbr,
                transaction?.Quantity,
                tranDesc,
                debitAmount: transaction.TransactionType?.ToUpper() == TRANS_TYPE_CREDIT ? transaction.Amount : 0,
                creditAmount: transaction.TransactionType?.ToUpper() == TRANS_TYPE_DEBIT ? transaction.Amount : 0
            );
            lineNbrDictionary.Add(transaction.TransactionID, lineA.LineNbr);

            // Create Rule B: Opposite entry to clearing account
            CreateGLTranLine(
                glGraph,
                property?.ClearingAcct,
                property?.ClearingSub,
                null, // No BranchID override for clearing account
                refNbr,
                transaction?.Quantity,
                tranDesc,
                debitAmount: transaction.TransactionType?.ToUpper() == TRANS_TYPE_DEBIT ? transaction.Amount : 0,
                creditAmount: transaction.TransactionType?.ToUpper() == TRANS_TYPE_CREDIT ? transaction.Amount : 0
            );
        }

        /// <summary> Get Cloud Bed Transaction Data from API </summary>
        public static void GetCloudBedTransactionData(LUMCloudBedTransactionProcess baseGraph, TransactionFilter transactionFilter = null)
        {
            baseGraph.Transaction.Cache.Clear();
            var filter = transactionFilter ?? baseGraph.TransacionFilter.Current;

            if (!filter.FromDate.HasValue || !filter.ToDate.HasValue)
                throw new PXException("Datetime is required!!");

            var transactionData = CloudBedHelper.GetTransactionData(filter);
            if (transactionData == null)
                throw new PXException("Get Transaction Failed!! (Transaction data is null)");

            using (PXTransactionScope sc = new PXTransactionScope())
            {
                foreach (var apiTransaction in transactionData)
                {
                    // Skip if already imported
                    var existingTransaction = LUMCloudBedTransactions.PK.Find(baseGraph, apiTransaction?.propertyID, apiTransaction?.accountingID);
                    if (existingTransaction != null)
                        continue;

                    var transaction = baseGraph.CurrentTransaction.Cache.CreateInstance() as LUMCloudBedTransactions;
                    MapTransactionFields(transaction, apiTransaction);
                    baseGraph.CurrentTransaction.Cache.Insert(transaction);
                }
                baseGraph.Save.Press();
                sc.Complete();
            }
        }

        /// <summary> Get Cloud Bed Reservation Data from API </summary>
        public static void GetCloudBedReservationData(LUMCloudBedTransactionProcess baseGraph, ReservationFilter reservationFilter = null)
        {
            var filter = reservationFilter ?? baseGraph.ReservationFilter.Current;

            if (!filter.ReservationFromDate.HasValue || !filter.ReservationToDate.HasValue)
                throw new PXException("Datetime is required!!");

            var reservationData = CloudBedHelper.GetReservationData(filter);
            if (reservationData == null)
                throw new PXException("Get Reservation Failed!!");

            var existingReservations = baseGraph.Reservations.Select().RowCast<LUMCloudBedReservations>();

            using (PXTransactionScope sc = new PXTransactionScope())
            {
                try
                {
                    // Import Reservations
                    ImportReservations(baseGraph, reservationData, existingReservations);

                    // Import Reservation Rates and Room Assignments
                    ImportReservationRates(baseGraph, reservationData, filter);

                    baseGraph.Save.Press();
                    sc.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sc.Dispose();
                }
            }
        }
        #endregion

        #region Helper Methods - GL Transaction Creation
        /// <summary> Create GL Batch header </summary>
        private Batch CreateGLBatch(JournalEntry glGraph, dynamic groupKey, LUMCloudBedPreference property, Ledger ledgerInfo)
        {
            var batch = glGraph.BatchModule.Cache.CreateInstance() as Batch;
            batch.Module = GL_MODULE;
            batch = glGraph.BatchModule.Cache.Insert(batch) as Batch;
            batch.DateEntered = groupKey.Date;
            batch.LedgerID = ledgerInfo?.LedgerID;
            batch.BranchID = property?.BranchID;
            batch.Description = $"CloudBed Transaction {groupKey.Date.ToString("yyyy-MM-dd")}";
            batch.CuryID = groupKey.Currency;
            glGraph.BatchModule.Cache.Update(batch);
            return batch;
        }

        /// <summary> Create a single GL Transaction Line </summary>
        private GLTran CreateGLTranLine(
            JournalEntry glGraph,
            int? accountID,
            int? subAccountID,
            int? branchID,
            string refNbr,
            int? quantity,
            string description,
            decimal? debitAmount,
            decimal? creditAmount)
        {
            var line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
            line.AccountID = accountID;
            line.SubID = subAccountID;
            line.BranchID = branchID;
            line.RefNbr = refNbr;
            line.Qty = quantity;
            line.TranDesc = description;
            line.CuryDebitAmt = debitAmount;
            line.CuryCreditAmt = creditAmount;
            line = glGraph.GLTranModuleBatNbr.Cache.Insert(line) as GLTran;
            return line;
        }

        /// <summary> Build transaction description from CloudBed data </summary>
        private string BuildTransactionDescription(LUMCloudBedTransactions transaction, LUMCloudBedReservations reservation)
        {
            var description = $"C/O {reservation?.EndDate?.ToString("yyyy-MM-dd")} - {reservation?.ThirdPartyIdentifier} - " +
                            $"{transaction.HouseAccountID} - {transaction.ReservationID} - {transaction.Description} - " +
                            $"{transaction.TransactionDateTime?.ToString("HH:mm:ss")}";
            return TruncateString(description, MAX_TRANDESC_LENGTH);
        }

        /// <summary> Update single transaction processing status </summary>
        private void UpdateTransactionStatus(LUMCloudBedTransactionProcess baseGraph, LUMCloudBedTransactions transaction, string errorMsg)
        {
            transaction.IsImported = string.IsNullOrEmpty(errorMsg);
            transaction.ErrorMessage = errorMsg;

            if (!(transaction.IsImported ?? false))
                PXProcessing.SetError<LUMCloudBedTransactions>(errorMsg);

            baseGraph.Transaction.Update(transaction);
        }

        /// <summary> Update processing status for entire transaction group </summary>
        private void UpdateGroupProcessingStatus(
            LUMCloudBedTransactionProcess baseGraph,
            IGrouping<dynamic, LUMCloudBedTransactions> transactionGroup,
            string errorMsg,
            string glBatchNbr,
            Dictionary<string, int?> lineNbrDictionary)
        {
            foreach (var transaction in transactionGroup)
            {
                PXProcessing.SetCurrentItem(transaction);

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    // Entire group failed
                    PXProcessing.SetError<LUMCloudBedTransactions>(errorMsg);
                    transaction.IsImported = false;
                    transaction.ErrorMessage = errorMsg;
                }
                else
                {
                    // Set individual status and GL reference
                    if (transaction.IsImported ?? false)
                        PXProcessing.SetProcessed<LUMCloudBedTransactions>();
                    else
                        PXProcessing.SetError<LUMCloudBedTransactions>(transaction.ErrorMessage);

                    transaction.BatchNbr = (transaction.IsImported ?? false) ? glBatchNbr : string.Empty;
                    transaction.LineNbr = (transaction?.IsImported ?? false) ? lineNbrDictionary[transaction.TransactionID] : null;
                }

                baseGraph.Transaction.Update(transaction);
            }
        }
        #endregion

        #region Helper Methods - Data Mapping
        /// <summary> Map API transaction data to DAC </summary>
        private static void MapTransactionFields(LUMCloudBedTransactions transaction, HH_APICustomization.Entity.Transaction apiData)
        {
            // Initialize status fields
            transaction.IsImported = false;
            transaction.BatchNbr = null;
            transaction.LineNbr = null;

            // Map basic fields
            transaction.PropertyID = apiData.propertyID;
            transaction.ReservationID = apiData.reservationID;
            transaction.SubReservationID = apiData.subReservationID;
            transaction.HouseAccountID = apiData.houseAccountID;
            transaction.HouseAccountName = apiData.houseAccountName;
            transaction.GuestID = apiData.guestID;
            transaction.PropertyName = apiData.propertyName;
            transaction.IsDeleted = apiData.isDeleted;

            // Map DateTime fields with timezone offset
            transaction.TransactionDateTime = TryParseAndAddHours(apiData.transactionDateTime, TIMEZONE_OFFSET_HOURS);
            transaction.TransactionDateTimeUTC = TryParseDateTime(apiData.transactionDateTimeUTC);
            transaction.TransactionLastModifiedDateTime = TryParseDateTime(apiData.transactionModifiedDateTime);
            transaction.TransactionLastModifiedDateTimeUTC = TryParseDateTime(apiData.transactionModifiedDateTimeUTC);
            transaction.GuestCheckIn = TryParseDateTime(apiData.guestCheckIn);
            transaction.GuestCheckOut = TryParseDateTime(apiData.guestCheckOut);

            // Map room and guest info
            transaction.RoomTypeID = apiData.roomTypeID;
            transaction.RoomTypeName = apiData.roomTypeName;
            transaction.RoomName = apiData.roomName;
            transaction.GuestName = apiData.guestName;

            // Map transaction details
            transaction.Description = apiData.description;
            transaction.Category = apiData.category;
            transaction.TransactionCode = apiData.transactionCode;
            transaction.TransactionNotes = TruncateString(apiData.notes, MAX_NOTES_LENGTH);
            transaction.Quantity = int.Parse(apiData.quantity);
            transaction.TransactionCategory = apiData.transactionCategory;

            // Handle amount (reverse if refund)
            if (transaction.TransactionCategory?.Trim().ToUpper() == TRANS_CATEGORY_REFUND && apiData.amount > 0)
                transaction.Amount = (decimal?)apiData.amount * -1;
            else
                transaction.Amount = (decimal?)apiData.amount;

            transaction.Currency = apiData.currency;
            transaction.UserName = apiData.userName;
            transaction.TransactionType = apiData.transactionType;
            transaction.ItemCategoryName = apiData.itemCategoryName;
            transaction.TransactionID = apiData.accountingID;
            transaction.ParentTransactionID = apiData.parentTransactionID;
            transaction.CardType = apiData.cardType;
        }

        /// <summary> Import reservations from API data </summary>
        private static void ImportReservations(
            LUMCloudBedTransactionProcess baseGraph,
            List<HH_APICustomization.Entity.Reservation> apiReservations,
            IEnumerable<LUMCloudBedReservations> existingReservations)
        {
            foreach (var apiReservation in apiReservations)
            {
                var existing = existingReservations.FirstOrDefault(x => x.PropertyID == apiReservation.propertyID && x.ReservationID == apiReservation.reservationID);
                var reservation = existing ?? baseGraph.Reservations.Cache.CreateInstance() as LUMCloudBedReservations;

                MapReservationFields(reservation, apiReservation);

                if (existing != null)
                    baseGraph.Reservations.Cache.Update(reservation);
                else
                    baseGraph.Reservations.Cache.Insert(reservation);
            }
        }

        /// <summary> Map API reservation data to DAC </summary>
        private static void MapReservationFields(LUMCloudBedReservations reservation, HH_APICustomization.Entity.Reservation apiData)
        {
            reservation.PropertyID = apiData.propertyID;
            reservation.ReservationID = apiData.reservationID;
            reservation.DateCreated = DateTime.Parse(apiData.dateCreated).AddHours(TIMEZONE_OFFSET_HOURS);
            reservation.DateModified = DateTime.Parse(apiData.dateModified).AddHours(TIMEZONE_OFFSET_HOURS);
            reservation.Source = apiData.sourceName;
            reservation.ThirdPartyIdentifier = apiData.thirdPartyIdentifier;
            reservation.Status = apiData.status;
            reservation.GuestName = apiData.guestName;
            reservation.StartDate = TryParseAndAddHours(apiData.startDate, TIMEZONE_OFFSET_HOURS);
            reservation.EndDate = TryParseAndAddHours(apiData.endDate, TIMEZONE_OFFSET_HOURS);
            reservation.Balance = (decimal?)apiData.balance;
        }

        /// <summary> Import reservation rates and room assignments </summary>
        private static void ImportReservationRates(
            LUMCloudBedTransactionProcess baseGraph,
            List<HH_APICustomization.Entity.Reservation> apiReservations,
            ReservationFilter filter)
        {
            var propertyIDs = apiReservations.Select(x => x.propertyID).Distinct().ToList();
            var rateDetails = CloudBedHelper.GetReservationWithRate(propertyIDs, filter.ReservationFromDate.Value, filter.ReservationToDate.Value);

            if (rateDetails == null)
                throw new PXException("Get Reservation Rate Detail failed!!");

            var existingRoomAssignments = baseGraph.RoomAssignment.Select().RowCast<LUMCloudBedRoomAssignment>();
            var existingRoomRates = baseGraph.RoomRateDetails.Select().RowCast<LUMCloudBedRoomRateDetails>();

            foreach (var rateDetail in rateDetails)
            {
                var reservationID = rateDetail?.reservationID;

                // Delete existing data for this reservation
                DeleteExistingRoomData(baseGraph, reservationID, existingRoomAssignments, existingRoomRates);

                // Import new data
                ImportRoomAssignmentsAndRates(baseGraph, rateDetail);
            }
        }

        /// <summary> Delete existing room assignment and rate data </summary>
        private static void DeleteExistingRoomData(
            LUMCloudBedTransactionProcess baseGraph,
            string reservationID,
            IEnumerable<LUMCloudBedRoomAssignment> existingAssignments,
            IEnumerable<LUMCloudBedRoomRateDetails> existingRates)
        {
            // Delete room assignments
            existingAssignments
                .Where(x => x.ReservationID == reservationID)
                .ToList()
                .ForEach(x => baseGraph.RoomAssignment.Delete(x));

            // Delete room rates
            existingRates
                .Where(x => x.ReservationID == reservationID)
                .ToList()
                .ForEach(x => baseGraph.RoomRateDetails.Delete(x));
        }

        /// <summary> Import room assignments and detailed rates </summary>
        private static void ImportRoomAssignmentsAndRates(
            LUMCloudBedTransactionProcess baseGraph,
            HH_APICustomization.Entity.ReservationRateDetail rateDetail)
        {
            foreach (var room in rateDetail.rooms)
            {
                if (string.IsNullOrEmpty(room?.roomID))
                    continue;

                // Create room assignment
                var roomAssignment = baseGraph.RoomAssignment.Cache.CreateInstance() as LUMCloudBedRoomAssignment;
                MapRoomAssignmentFields(roomAssignment, room, rateDetail);
                baseGraph.RoomAssignment.Insert(roomAssignment);

                // Create room rates
                foreach (var rate in room.detailedRoomRates)
                {
                    var roomRate = baseGraph.RoomRateDetails.Cache.CreateInstance() as LUMCloudBedRoomRateDetails;
                    MapRoomRateFields(roomRate, rateDetail.reservationID, room.roomID, rate);
                    baseGraph.RoomRateDetails.Insert(roomRate);
                }
            }
        }

        /// <summary> Map room assignment fields </summary>
        private static void MapRoomAssignmentFields(LUMCloudBedRoomAssignment assignment, HH_APICustomization.Entity.Room room, HH_APICustomization.Entity.ReservationRateDetail rateDetail)
        {
            assignment.ReservationID = rateDetail.reservationID;
            assignment.Roomid = room?.roomID;
            assignment.RoomName = room?.roomName;
            assignment.RoomType = room?.roomTypeID;
            assignment.RoomTypeName = room?.roomTypeName;
            assignment.IsDeleted = rateDetail?.isDeleted;
            assignment.Status = rateDetail?.status;
            assignment.Checkin = TryParseDateTime(room?.roomCheckIn);
            assignment.Checkout = TryParseDateTime(room?.roomCheckOut);
        }

        /// <summary> Map room rate fields </summary>
        private static void MapRoomRateFields(LUMCloudBedRoomRateDetails roomRate, string reservationID, string roomID, KeyValuePair<string, JToken> rate)
        {
            roomRate.ReservationID = reservationID;
            roomRate.Roomid = roomID;
            roomRate.RateDate = DateTime.Parse(rate.Key);
            roomRate.Rate = Decimal.Parse(rate.Value.ToString());
        }
        #endregion

        #region Helper Methods - Utilities
        /// <summary> Parse DateTime and add hours offset </summary>
        private static DateTime? TryParseAndAddHours(string dateTimeString, int hoursToAdd)
        {
            DateTime result;
            if (DateTime.TryParse(dateTimeString, out result))
                return result.AddHours(hoursToAdd);
            return null;
        }

        /// <summary> Parse DateTime without offset </summary>
        private static DateTime? TryParseDateTime(string dateTimeString)
        {
            DateTime result;
            if (DateTime.TryParse(dateTimeString, out result))
                return result;
            return null;
        }

        /// <summary> Truncate string to maximum length </summary>
        private static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length > maxLength ? value.Substring(0, maxLength) : value;
        }
        #endregion
    }

    #region Filter Classes
    [Serializable]
    public class TransactionFilter : PXBqlTable, IBqlTable
    {
        [PXDate]
        [PXDefault]
        [PXUIField(DisplayName = "Transaction From")]
        public virtual DateTime? FromDate { get; set; }
        public abstract class fromDate : PX.Data.BQL.BqlDateTime.Field<fromDate> { }

        [PXDate]
        [PXDefault]
        [PXUIField(DisplayName = "Transaction To")]
        public virtual DateTime? ToDate { get; set; }
        public abstract class toDate : PX.Data.BQL.BqlDateTime.Field<toDate> { }

        [PXString(IsUnicode = true)]
        [PXDefault("ImportTransaction")]
        [PXUIField(DisplayName = "Process type")]
        [PXStringList(new string[] { "ImportTransaction", "ImportReservation" }, new string[] { "Import Transaction", "Import Reservation" })]
        public virtual string ProcessType { get; set; }
        public abstract class processType : PX.Data.BQL.BqlString.Field<processType> { }

        [PXBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Display Imported Transaction")]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }

        [PXString(50, IsUnicode = true, InputMask = "")]
        [PXSelector(typeof(Search<LUMCloudBedPreference.cloudBedPropertyID>),
                    typeof(LUMCloudBedPreference.branchID))]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string CloudBedPropertyID { get; set; }
        public abstract class cloudBedPropertyID : PX.Data.BQL.BqlString.Field<cloudBedPropertyID> { }

        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }

        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Transaction ID")]
        public virtual string TransactionID { get; set; }
        public abstract class transactionID : PX.Data.BQL.BqlString.Field<transactionID> { }
    }

    [Serializable]
    public class ReservationFilter : PXBqlTable, IBqlTable
    {
        [PXDateAndTime(DisplayMask = "g", InputMask = "g")]
        [PXDefault]
        [PXUIField(DisplayName = "ReservationUpdate From")]
        public virtual DateTime? ReservationFromDate { get; set; }
        public abstract class reservationFromDate : PX.Data.BQL.BqlDateTime.Field<reservationFromDate> { }

        [PXDateAndTime(DisplayMask = "g", InputMask = "g")]
        [PXDefault]
        [PXUIField(DisplayName = "ReservationUpdate To")]
        public virtual DateTime? ReservationToDate { get; set; }
        public abstract class reservationToDate : PX.Data.BQL.BqlDateTime.Field<reservationToDate> { }

        [PXString(50, IsUnicode = true, InputMask = "")]
        [PXSelector(typeof(Search<LUMCloudBedPreference.cloudBedPropertyID>),
                    typeof(LUMCloudBedPreference.branchID))]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string CloudBedPropertyID { get; set; }
        public abstract class cloudBedPropertyID : PX.Data.BQL.BqlString.Field<cloudBedPropertyID> { }

        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "ReservationID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
    }
    #endregion
}
