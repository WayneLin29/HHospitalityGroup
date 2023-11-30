﻿using HHAPICustomization.DAC;
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

namespace HH_APICustomization.Graph
{
    public class LUMCloudBedRemitTransactionProcess : PXGraph<LUMCloudBedRemitTransactionProcess, LUMRemittance>
    {
        [PXHidden]
        public PXSetup<LUMHHSetup> Setup;

        [PXHidden]
        public SelectFrom<LUMCloudBedPreference>
              .Where<LUMCloudBedPreference.branchID.IsEqual<LUMRemittance.branch.FromCurrent>>
              .View ClodBedPreference;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>.View CloudbedTransactions;

        [PXHidden]
        public SelectFrom<LUMRemitExcludeTransactions>
              .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View ExculdeTransactions;

        [PXHidden]
        public SelectFrom<vHHRemitReservationCheck>
              .Where<vHHRemitReservationCheck.reservationID.IsNotNull>
              .View RemiteReservationCheck;

        [PXHidden]
        public SelectFrom<LUMCloudBedTransactions>
              .Where<LUMCloudBedTransactions.remitRefNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View RemitTransactions;

        [PXHidden]
        public PXSetup<LUMRemitRequestApproval> RemitApproval;

        [PXHidden]
        public SelectFrom<vRemitBlockCheck>.View RemitBlockCheck;

        [PXViewName("Remit Document")]
        public SelectFrom<LUMRemittance>.View Document;

        public SelectFrom<LUMRemitPayment>
              .Where<LUMRemitPayment.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View PaymentTransactions;

        [PXCacheName("PaymentDetails")]
        public SelectFrom<LUMCloudBedTransactions>
              .LeftJoin<LUMCloudBedReservations>.On<LUMCloudBedTransactions.propertyID.IsEqual<LUMCloudBedReservations.propertyID>
                                               .And<LUMCloudBedTransactions.reservationID.IsEqual<LUMCloudBedReservations.reservationID>>>
              .Where<LUMCloudBedTransactions.description.IsEqual<LUMRemitPayment.description.FromCurrent>
                .And<LUMCloudBedTransactions.isImported.IsNotEqual<True>>>
              .View PaymentDetails;

        [PXCacheName("RemitReservation")]
        public SelectFrom<LUMRemitReservation>
              .Where<LUMRemitReservation.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View ReservationTransactions;

        [PXCacheName("ReservationDetails")]
        public SelectFrom<LUMCloudBedTransactions2>
              .Where<LUMCloudBedTransactions2.reservationID.IsEqual<LUMRemitReservation.reservationID.FromCurrent>>
              .View ReservationDetails;

        [PXCacheName("Remit Block")]
        public SelectFrom<LUMRemitBlock>
              .InnerJoin<LUMCloudBedRoomBlock>.On<LUMRemitBlock.blockID.IsEqual<LUMCloudBedRoomBlock.roomBlockID>>
              .LeftJoin<LUMCloudBedRoomBlockDetails>.On<LUMCloudBedRoomBlock.roomBlockID.IsEqual<LUMCloudBedRoomBlockDetails.roomBlockID>
                                                   .And<LUMCloudBedRoomBlockDetails.propertyID.IsEqual<LUMCloudBedRoomBlock.propertyID>>>
              .Where<LUMRemitBlock.refNbr.IsEqual<LUMRemittance.refNbr.FromCurrent>>
              .View RemitBlock;

        [PXViewName("Approval Remit")]
        public EPApprovalAutomation<LUMRemittance, LUMRemittance.approved, LUMRemittance.rejected, LUMRemittance.hold, LUMRemitRequestApproval> Approval;

        #region Delegate Data View

        public IEnumerable paymentDetails()
        {
            PXView select = new PXView(this, true, this.PaymentDetails.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;

            var _propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
            foreach (PXResult<LUMCloudBedTransactions> item in result)
            {
                var row = item.GetItem<LUMCloudBedTransactions>();
                if (row?.PropertyID == _propertyID && row?.TransactionType == "credit" && row?.IsImported != true && row?.UserName != "SYSTEM")
                    yield return item;
            }
        }

        public IEnumerable reservationDetails()
        {
            PXView select = new PXView(this, true, this.ReservationDetails.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;

            var _propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
            foreach (LUMCloudBedTransactions2 item in result)
                if (item?.PropertyID == _propertyID)
                    yield return item;
        }

        public IEnumerable remitBlock()
        {
            PXView select = new PXView(this, true, this.RemitBlock.View.BqlSelect);
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
        [PXUIField(DisplayName = "REFRESH", MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable refresh(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                this.Save.Press();
                var _refNbr = this.Document.Current?.RefNbr;
                //ReloadCloudBedData();
                var propertyID = this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID;
                var allTrans = SelectFrom<LUMCloudBedTransactions>
                              .Where<LUMCloudBedTransactions.propertyID.IsEqual<P.AsString>>
                              .View.Select(this, propertyID)
                              .RowCast<LUMCloudBedTransactions>()
                              .Where(x => (string.IsNullOrEmpty(x.RemitRefNbr) || x.RemitRefNbr == _refNbr) && !(x.IsImported ?? false));
                var excludeTrans = this.ExculdeTransactions.View.SelectMulti().RowCast<LUMRemitExcludeTransactions>();
                // expected ExcludeTransaction
                allTrans = allTrans.Where(x => !excludeTrans.Any(y => y.TransactionID == x.TransactionID));

                // Update Cloudbed Transaction Remit Refnbr / AccountID / Sub-AccountID
                foreach (var item in allTrans)
                {
                    // 沒有Account/SubAccount 才重新執行
                    if (!item.AccountID.HasValue || !item.SubAccountID.HasValue)
                    {
                        var mapAccountInfo = CloudBedHelper.GetCloudbedTransactionMappingAccount(this, item);
                        item.AccountID = mapAccountInfo.AccountID;
                        item.SubAccountID = mapAccountInfo.SubAccountID;
                    }
                    item.RemitRefNbr = _refNbr;
                    this.CloudbedTransactions.Update(item);
                }

                var existsPaymentTrans = this.PaymentTransactions.View.SelectMulti().RowCast<LUMRemitPayment>();
                #region Insert/Update LUMRemitPayment
                // Insert/Update LUMRemitPayment
                foreach (var item in allTrans.Where(x => x.TransactionType == "credit" && !(x?.IsImported ?? false) && x.UserName != "SYSTEM").GroupBy(x => x.Description))
                {
                    var paymentLine = existsPaymentTrans.FirstOrDefault(x => x.Description == item.Key);
                    if (paymentLine != null)
                    {
                        paymentLine.RecordedAmt = item.Sum(x => x?.Amount);
                        paymentLine.RemitAmt = 0;
                        paymentLine = this.PaymentTransactions.Update(paymentLine);
                    }
                    else
                    {
                        paymentLine = this.PaymentTransactions.Cache.CreateInstance() as LUMRemitPayment;
                        paymentLine.Description = item.Key;
                        paymentLine.RecordedAmt = item.Sum(x => x?.Amount);
                        paymentLine.RemitAmt = 0;
                        paymentLine = this.PaymentTransactions.Insert(paymentLine);
                    }
                }
                #endregion

                #region Insert/Update LUMRemitReservation
                // Insert/Update LUMRemitReservation
                var reservationCheckData = this.RemiteReservationCheck.View.SelectMulti().RowCast<vHHRemitReservationCheck>();
                var existsReservationTrs = this.ReservationTransactions.View.SelectMulti().RowCast<LUMRemitReservation>();
                foreach (var checkObj in reservationCheckData.Where(x => x.PropertyID == propertyID))
                {
                    var resLine = new LUMRemitReservation();
                    var mapReservation = LUMCloudBedReservations.PK.Find(this, checkObj.PropertyID, checkObj.ReservationID);
                    var isExists = existsReservationTrs.FirstOrDefault(x => x.RefNbr == _refNbr && x.ReservationID == checkObj.ReservationID) != null;
                    resLine = existsReservationTrs.FirstOrDefault(x => x.RefNbr == _refNbr && x.ReservationID == checkObj.ReservationID) ?? resLine;
                    resLine.RefNbr = _refNbr;
                    resLine.ReservationID = checkObj.ReservationID;
                    if (resLine?.IsOutOfScope ?? false)
                        continue;
                    resLine.CheckMessage = checkObj.Message;
                    resLine.GuestName = mapReservation?.GuestName;
                    resLine.Status = mapReservation?.Status;
                    resLine.CheckIn = mapReservation?.StartDate;
                    resLine.CheckOut = mapReservation?.EndDate;
                    resLine.Balance = mapReservation?.Balance ?? 0;
                    resLine.Total = mapReservation?.Total ?? 0;
                    CalculateReservationAmount(this, _refNbr, checkObj, resLine);
                    resLine = isExists ? this.ReservationTransactions.Update(resLine) : this.ReservationTransactions.Insert(resLine);
                }
                #endregion

                #region Insert/Update LUMRemitBlock
                var exisBlocks = this.RemitBlock.View.SelectMulti().RowCast<LUMRemitBlock>();
                var availabeBlocks = this.RemitBlockCheck.View.SelectMulti().RowCast<vRemitBlockCheck>();
                foreach (var checkBlock in availabeBlocks.Where(x => x.PropertyID == propertyID).GroupBy(x => new { x.PropertyID, x.RoomBlockID }))
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

                this.Save.Press();
            });
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
                this.Document.Cache.SetValueExt<LUMRemittance.hold>(doc, false);
                this.Document.Update(doc);
                // 如果沒有進入簽核則直接將狀態改為Approve
                //if (SelectFrom<EPApproval>.Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>.View.Select(this, doc.Noteid).Count == 0)
                //{
                //    this.Document.Cache.SetValue<LUMRemittance.approved>(doc, true);
                //    this.Document.Cache.SetValue<LUMRemittance.status>(doc, LUMRemitStatus.Balanced);
                //    this.Document.Update(doc);
                //}
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
                var selectedItems = this.PaymentDetails.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && (x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
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
                var selectedItems = this.PaymentDetails.Cache.Updated.RowCast<LUMCloudBedTransactions>().Where(x => x.Selected ?? false && !(x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
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
                var selectedItems = this.ReservationDetails.Cache.Updated.RowCast<LUMCloudBedTransactions2>().Where(x => x.Selected ?? false && (x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
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
                var selectedItems = this.ReservationDetails.Cache.Updated.RowCast<LUMCloudBedTransactions2>().Where(x => x.Selected ?? false && !(x.GetExtension<LUMCloudBedTransactionsExt>().ToRemit ?? false));
                ToggleInTransactions(selectedItems);
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
                var selectedRes = this.ReservationTransactions.Cache.Updated.RowCast<LUMRemitReservation>().Where(x => x.Selected ?? false && !(x?.IsOutOfScope ?? false));
                foreach (var res in selectedRes)
                {
                    var selectedItems = GetCloudBedTransByReservation(res?.ReservationID);
                    ToggleOutTransactions(selectedItems, true);
                }
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
                var selectedRes = this.ReservationTransactions.Cache.Updated.RowCast<LUMRemitReservation>().Where(x => x.Selected ?? false && (x?.IsOutOfScope ?? false));
                foreach (var res in selectedRes)
                {
                    var selectedItems = GetCloudBedTransByReservation(res?.ReservationID);
                    ToggleInTransactions(selectedItems, false);
                }
            });
            return adapter.Get();
        }

        /// <summary> Release Remit and Create GL </summary>
        public PXAction<LUMRemittance> Release;
        [PXButton]
        [PXUIField(DisplayName = "RELEASE", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable release(PXAdapter adapter)
        {
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
            if (Document.AskExt(true) != WebDialogResult.OK)
                return adapter.Get();
            var reason = this.Document.Cache.Updated.RowCast<LUMRemittance>().FirstOrDefault()?.VoidReason;
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
        [PXUIField(DisplayName = "AUDITTOGGLE IN", Enabled = false, MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable auditReservationToggleIn(PXAdapter adapter)
            => this.reservationToggleIn(adapter);

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

                FieldControl();
            }
        }

        public virtual void _(Events.RowSelected<LUMCloudBedTransactions> e)
        {
            var row = e.Row;
            if (row != null)
            {
                var excludeTrans = this.ExculdeTransactions.Select().RowCast<LUMRemitExcludeTransactions>();
                row.GetExtension<LUMCloudBedTransactionsExt>().ToRemit = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr) == null;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleByID = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedByID;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleDateTime = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedDateTime;
            }
        }

        public virtual void _(Events.RowSelected<LUMCloudBedTransactions2> e)
        {
            var row = e.Row;
            if (row != null)
            {
                var excludeTrans = this.ExculdeTransactions.Select().RowCast<LUMRemitExcludeTransactions>();
                row.GetExtension<LUMCloudBedTransactionsExt>().ToRemit = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr) == null;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleByID = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedByID;
                row.GetExtension<LUMCloudBedTransactionsExt>().ToggleDateTime = excludeTrans.FirstOrDefault(x => row?.TransactionID == x?.TransactionID && x.RefNbr == this.Document.Current?.RefNbr)?.CreatedDateTime;
                row.GetExtension<LUMCloudBedTransactionsExt>().CreditAmount = row?.TransactionType?.ToUpper() == "CREDIT" ? row?.Amount : 0;
                row.GetExtension<LUMCloudBedTransactionsExt>().DebitAmount = row?.TransactionType?.ToUpper() == "DEBIT" ? row?.Amount : 0;
            }
        }

        public virtual void _(Events.FieldDefaulting<LUMRemitPayment.lineNbr> e)
        {
            var currentList = this.PaymentTransactions.Select().RowCast<LUMRemitPayment>();
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
                switch (row.Status)
                {
                    case LUMRemitStatus.PendingApproval:
                        PXUIFieldAttribute.SetEnabled<LUMRemitPayment.oPRemark>(this.PaymentTransactions.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMRemitReservation.oPRemark>(this.PaymentTransactions.Cache, null, false);
                        break;
                    case LUMRemitStatus.Open:
                        PXUIFieldAttribute.SetEnabled<LUMRemitPayment.oPRemark>(this.PaymentTransactions.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMRemitReservation.oPRemark>(this.PaymentTransactions.Cache, null, false);
                        break;
                    case LUMRemitStatus.Released:
                    case LUMRemitStatus.Voided:
                        PXUIFieldAttribute.SetEnabled<LUMRemitPayment.oPRemark>(this.PaymentTransactions.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMRemitReservation.oPRemark>(this.PaymentTransactions.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.accountID>(this.PaymentDetails.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.subAccountID>(this.PaymentDetails.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.accountID>(this.ReservationDetails.Cache, null, false);
                        PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.subAccountID>(this.ReservationDetails.Cache, null, false);
                        break;
                }
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

        #endregion

        #region Method
        /// <summary> 重新載入 Cloudbed API 資料</summary>
        public virtual void ReloadCloudBedData()
        {
            var cloudbedGraph = PXGraph.CreateInstance<LUMCloudBedTransactionProcess>();
            // Load Cloudbed transaction data
            cloudbedGraph.TransacionFilter.Current = cloudbedGraph.TransacionFilter.Cache.CreateInstance() as TransactionFilter;
            cloudbedGraph.TransacionFilter.Current.FromDate = Accessinfo.BusinessDate.Value.Date;
            cloudbedGraph.TransacionFilter.Current.ToDate = PX.Common.PXTimeZoneInfo.Now.AddDays(1).Date;
            cloudbedGraph.importTransactionData.Press();
            // Load Cloudbed reservation data
            cloudbedGraph.ReservationFilter.Current = cloudbedGraph.ReservationFilter.Cache.CreateInstance() as ReservationFilter;
            cloudbedGraph.ReservationFilter.Current.ReservationFromDate = Accessinfo.BusinessDate.Value.Date;
            cloudbedGraph.ReservationFilter.Current.ReservationToDate = PX.Common.PXTimeZoneInfo.Now.AddDays(1).Date;
            cloudbedGraph.importReservationData.Press();
        }

        /// <summary> TOGGLE OUT Cloudbed Transactions Data </summary>
        public virtual void ToggleOutTransactions(IEnumerable<LUMCloudBedTransactions> selectedItems, bool? _isScopeOut = null)
        {
            this.CloudbedTransactions.View.SelectMulti();
            this.ReservationTransactions.View.SelectMulti();
            foreach (var item in selectedItems)
            {
                if (item?.RemitRefNbr != this.Document.Current?.RefNbr)
                    continue;
                var excludeData = this.ExculdeTransactions.Cache.CreateInstance() as LUMRemitExcludeTransactions;
                excludeData.RefNbr = this.Document.Current?.RefNbr;
                excludeData.TransactionID = item?.TransactionID;
                excludeData = this.ExculdeTransactions.Insert(excludeData);
                this.ExculdeTransactions.Cache.PersistInserted(excludeData);
                // Remove Remit RefNbr.
                UpdateTransactionRefNbr(item?.TransactionID, null);
                // Recalcuate Remit Reservation
                ReCalculateRemitReservation(this.Document.Current?.RefNbr, item?.ReservationID, _isScopeOut);
            }
            InvokeCachePersist<LUMRemitReservation>(PXDBOperation.Update);
            //this.ReservationTransactions.Cache.Persist(PXDBOperation.Update);
        }

        /// <summary> TOGGLE IN Cloudbed Transactions Data </summary>
        public virtual void ToggleInTransactions(IEnumerable<LUMCloudBedTransactions> selectedItems, bool? _isScopeOut = null)
        {
            this.CloudbedTransactions.View.SelectMulti();
            this.ReservationTransactions.View.SelectMulti();
            foreach (var item in selectedItems)
            {
                var excludeData = new LUMRemitExcludeTransactions();
                excludeData.RefNbr = this.Document.Current?.RefNbr;
                excludeData.TransactionID = item?.TransactionID;
                excludeData = (LUMRemitExcludeTransactions)this.ExculdeTransactions.Cache.Locate(excludeData);
                this.ExculdeTransactions.Delete(excludeData);
                this.ExculdeTransactions.Cache.PersistDeleted(excludeData);
                // Add Remit RefNbr.
                UpdateTransactionRefNbr(item?.TransactionID, this.Document.Current?.RefNbr);
                // Recalcuate Remit Reservation
                ReCalculateRemitReservation(this.Document.Current?.RefNbr, item?.ReservationID, _isScopeOut);
            }
            InvokeCachePersist<LUMRemitReservation>(PXDBOperation.Update);
            //this.ReservationTransactions.Cache.Persist(PXDBOperation.Update);
        }

        /// <summary> Add/Remote Cloudbed Transaction Remit RefNbr. & Recorded Amount </summary>
        private void UpdateTransactionRefNbr(string _transactionID, string _refNbr)
        {
            // Update/Remova Remit RefNbr.
            var trans = new LUMCloudBedTransactions();
            trans.TransactionID = _transactionID;
            trans = (LUMCloudBedTransactions)this.CloudbedTransactions.Cache.Locate(trans);
            trans.RemitRefNbr = _refNbr;
            trans = this.CloudbedTransactions.Update(trans);
            this.CloudbedTransactions.Cache.PersistUpdated(trans);

            // Update Payment-Check Recorded Amount
            var paymentTrans = this.PaymentTransactions.View.SelectMulti().RowCast<LUMRemitPayment>();
            var currentPayment = paymentTrans.Where(x => x.Description == trans?.Description).FirstOrDefault();
            if (currentPayment != null)
            {
                currentPayment.RecordedAmt += string.IsNullOrEmpty(_refNbr) ? ((trans?.Amount ?? 0) * -1) : (trans?.Amount ?? 0);
                currentPayment = this.PaymentTransactions.Update(currentPayment);
                this.PaymentTransactions.Cache.PersistUpdated(currentPayment);
            }
        }

        /// <summary> ReCalcuate Remit Reservation when transation toggle on/in </summary>
        private void ReCalculateRemitReservation(string _refNbr, string _reservationID, bool? _isScopeOut)
        {
            vHHRemitReservationCheck checkObj = new vHHRemitReservationCheck()
            {
                ReservationID = _reservationID,
                PropertyID = GetCurrentPropertyID(),
                Type = _reservationID.Contains("-") ? "H" : "R"
            };
            var resLine = new LUMRemitReservation();
            resLine.RefNbr = _refNbr;
            resLine.ReservationID = _reservationID;
            resLine = this.ReservationTransactions.Cache.Locate(resLine) as LUMRemitReservation;
            if (_isScopeOut != null)
                resLine.IsOutOfScope = _isScopeOut;
            CalculateReservationAmount(this, _refNbr, checkObj, resLine);
            resLine = this.ReservationTransactions.Update(resLine);
        }

        /// <summary>
        /// Calculate Reservation Amount
        /// </summary>
        /// <param name="baseGraph"> Grpah </param>
        /// <param name="_refNbr"> Remit Nbr.</param>
        /// <param name="checkObj"> vHHRemitReservationCheck </param>
        /// <param name="reservationobj"> LUMRemitReservation </param>
        private void CalculateReservationAmount(PXGraph baseGraph, string _refNbr, vHHRemitReservationCheck checkObj, LUMRemitReservation reservationobj)
        {
            var transByReservation = SelectFrom<LUMCloudBedTransactions>
                                           .Where<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>
                                             .And<LUMCloudBedTransactions.propertyID.IsEqual<P.AsString>>>
                                           .View.Select(baseGraph, checkObj?.ReservationID, checkObj?.PropertyID).RowCast<LUMCloudBedTransactions>();
            switch (checkObj?.Type)
            {
                // Reservation
                case "R":
                    transByReservation = transByReservation.Where(x => (x?.IsImported ?? false) || x?.RemitRefNbr == _refNbr);
                    // Room Revenue - TransactionType = 'Debit' AND Category = 'Room Revenue'
                    reservationobj.RoomRevenue = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && x?.Category?.ToUpper() == "ROOM REVENUE").Sum(x => x?.Amount ?? 0);
                    // Taxes - TransactionType = 'Debit' AND Category = 'Taxes'
                    reservationobj.Taxes = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && x?.Category?.ToUpper() == "TAXES").Sum(x => x?.Amount ?? 0);
                    // Fees - TransactionType = 'Debit' AND Category = 'Fees'
                    reservationobj.Fees = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && x?.Category?.ToUpper() == "FEES").Sum(x => x?.Amount ?? 0);
                    // Others - TransactionType = 'Debit' AND Category NOT IN ('Room Revenue', 'Taxes', 'Fees')
                    reservationobj.Others = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && !new string[] { "ROOM REVENUE", "TAXES", "FEES" }.Contains(x.Category)).Sum(x => x?.Amount ?? 0);
                    // Payment - TransactionType = 'Credit'　
                    reservationobj.Payment = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "CREDIT").Sum(x => x?.Amount);
                    // Pending Post Count of LUMRemitExcludeTransactions where (RefNbr = Current Remit AND Reservation = Current Reservation)
                    reservationobj.PendingCount = SelectFrom<LUMRemitExcludeTransactions>
                                                 .InnerJoin<LUMCloudBedTransactions>.On<LUMRemitExcludeTransactions.transactionID.IsEqual<LUMCloudBedTransactions.transactionID>
                                                       .And<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>>>
                                                 .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>
                                                 .View.Select(baseGraph, checkObj.ReservationID, _refNbr).Count;
                    break;
                // House Account
                case "H":
                    transByReservation = transByReservation.Where(x => !(x?.IsImported ?? false) && x?.RemitRefNbr == _refNbr);
                    // Room Revenue - TransactionType = 'Debit' AND Category = 'Room Revenue'
                    reservationobj.RoomRevenue = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && x?.Category?.ToUpper() == "ROOM REVENUE").Sum(x => x?.Amount ?? 0);
                    // Taxes - TransactionType = 'Debit' AND Category = 'Taxes'
                    reservationobj.Taxes = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && x?.Category?.ToUpper() == "TAXES").Sum(x => x?.Amount ?? 0);
                    // Fees - TransactionType = 'Debit' AND Category = 'Fees'
                    reservationobj.Fees = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && x?.Category?.ToUpper() == "FEES").Sum(x => x?.Amount ?? 0);
                    // Others - TransactionType = 'Debit' AND Category NOT IN ('Room Revenue', 'Taxes', 'Fees')
                    reservationobj.Others = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "DEBIT" && !new string[] { "ROOM REVENUE", "TAXES", "FEES" }.Contains(x.Category)).Sum(x => x?.Amount ?? 0);
                    // Payment - TransactionType = 'Credit'　
                    reservationobj.Payment = transByReservation.Where(x => x?.TransactionType?.ToUpper() == "CREDIT").Sum(x => x?.Amount);
                    // Pending Post Count of LUMRemitExcludeTransactions where (RefNbr = Current Remit AND Reservation = Current Reservation)
                    reservationobj.PendingCount = SelectFrom<LUMRemitExcludeTransactions>
                                                 .InnerJoin<LUMCloudBedTransactions>.On<LUMRemitExcludeTransactions.transactionID.IsEqual<LUMCloudBedTransactions.transactionID>
                                                       .And<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>>>
                                                 .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>>
                                                 .View.Select(baseGraph, checkObj.ReservationID, _refNbr).Count;
                    break;
            }
        }

        /// <summary> Get CloudBedTransaction by ReservationID </summary>
        public IEnumerable<LUMCloudBedTransactions> GetCloudBedTransByReservation(string _reservationID)
        {
            return SelectFrom<LUMCloudBedTransactions>
                  .Where<LUMCloudBedTransactions.reservationID.IsEqual<P.AsString>
                    .And<LUMCloudBedTransactions.propertyID.IsEqual<P.AsString>>>
                  .View.Select(this, _reservationID, this.ClodBedPreference.Select().TopFirst?.CloudBedPropertyID)
                  .RowCast<LUMCloudBedTransactions>();
        }

        /// <summary> Create Journal Transaction </summary>
        public virtual void CreateJournalTransaction(LUMCloudBedRemitTransactionProcess baseGraph, List<LUMCloudBedTransactions> selectedData)
        {
            var groupData = selectedData.Where(x => !(x.IsImported ?? false)).GroupBy(x => new { x.RemitRefNbr, x.Currency });
            var cloudbedProperty = SelectFrom<LUMCloudBedPreference>.View.Select(baseGraph).RowCast<LUMCloudBedPreference>();
            //var reservationData = baseGraph.Reservations.Select().RowCast<LUMCloudBedReservations>();
            //var AcctMapAData = SelectFrom<LUMCloudBedAccountMapping>.View.Select(baseGraph).RowCast<LUMCloudBedAccountMapping>();
            var ledgerInfo = SelectFrom<Ledger>.Where<Ledger.ledgerCD.IsEqual<P.AsString>>.View.Select(baseGraph, "ACTUAL").TopFirst;
            foreach (var cloudBedGroupRow in groupData)
            {
                var glBatchNbr = string.Empty;
                string errorMsg = string.Empty;              // 整筆資料錯誤訊息
                var lineErrorDic = new Dictionary<string, string>();    // 每筆資料的錯誤訊息
                var lineNbrDic = new Dictionary<string, int?>();        // 每筆資料所對應到的GL Line

                // 整筆資料錯誤Handler
                try
                {
                    #region Header
                    var glGraph = PXGraph.CreateInstance<JournalEntry>();
                    var doc = glGraph.BatchModule.Cache.CreateInstance() as Batch;
                    doc.Module = "GL";
                    doc = glGraph.BatchModule.Cache.Insert(doc) as Batch;
                    doc.DateEntered = baseGraph.Document.Current?.Date;
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
                        }
                        catch (Exception ex)
                        {
                            errorMsg = $"Error: {ex.Message}";
                        }
                        finally
                        {
                            row.IsImported = string.IsNullOrEmpty(errorMsg) ? true : false;
                            row.ErrorMessage = errorMsg;
                            baseGraph.CloudbedTransactions.Update(row);
                        }
                    }
                    #endregion

                    errorMsg = string.Empty;
                    glGraph.releaseFromHold.Press();
                    glGraph.Save.Press();
                    glGraph.release.Press();
                    glBatchNbr = doc.BatchNbr;
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
                            x.IsImported = false;
                            x.ErrorMessage = errorMsg;
                            baseGraph.CloudbedTransactions.Update(x);
                        });
                    // 回寫BatchNbr & LineNbr
                    else
                        cloudBedGroupRow.ToList().ForEach(x =>
                        {
                            x.BatchNbr = (x.IsImported ?? false) ? glBatchNbr : string.Empty;
                            x.LineNbr = (x?.IsImported ?? false) ? lineNbrDic[x.TransactionID] : null;
                            baseGraph.CloudbedTransactions.Update(x);
                        });
                    // 回寫 Remit Document
                    var remitDoc = baseGraph.Document.Current;
                    remitDoc.BatchNbr = glBatchNbr;
                    remitDoc.Status = LUMRemitStatus.Released;
                    baseGraph.Document.UpdateCurrent();
                }
                // Save Process Result
                baseGraph.Actions.PressSave();
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
            this.ReleaseFromHold.SetVisible(false);
            this.Refresh.SetVisible(false);
            this.Release.SetVisible(false);
            this.GoVoid.SetVisible(false);
            this.InScope.SetEnabled(false);
            this.OutOfScope.SetEnabled(false);
            this.Approve.SetVisible(false);
            this.Reject.SetVisible(false);
            #endregion

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
                    }
                    break;
                case LUMRemitStatus.PendingApproval:
                    if (this.Setup.Current?.RemitRequestApproval ?? false)
                    {
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

                    this.Release.SetVisible(true);
                    this.Release.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Success);
                    break;
                case LUMRemitStatus.Released:
                    this.GoVoid.SetVisible(true);
                    this.GoVoid.SetConnotation(PX.Data.WorkflowAPI.ActionConnotation.Warning);
                    break;
            }
        }

        /// <summary> Control All Field according </summary>
        private void FieldControl()
        {
            #region Set Field Enabled
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.transactionID>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.reservationID>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.houseAccountName>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.guestName>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.amount>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.userName>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.transactionNotes>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.createdDateTime>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.remitRefNbr>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactionsExt.toggleByID>(this.PaymentDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactionsExt.toggleDateTime>(this.PaymentDetails.Cache, null, false);

            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.transactionID>(this.ReservationDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.remitRefNbr>(this.ReservationDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.description>(this.ReservationDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.transactionNotes>(this.ReservationDetails.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions2.userName>(this.ReservationDetails.Cache, null, false);
            #endregion
        }

        public Contact GetContactObject()
            => this.Document.Current != null ? SelectFrom<Contact>.Where<Contact.contactID.IsEqual<P.AsInt>>.View.Select(this, this.Document.Current?.OwnerID) : null;

        public EPEmployee GetRequestEmployeeObject()
            => this.Document.Current != null ? SelectFrom<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<P.AsInt>>.View.Select(this, GetContactObject()?.BAccountID) : null;

        #endregion

    }

    public class LUMCloudBedTransactionsExt : PXCacheExtension<LUMCloudBedTransactions>
    {
        #region ToRemit
        [PXBool]
        [PXUIField(DisplayName = "To Remit", Enabled = false)]
        public virtual bool? ToRemit { get; set; }
        public abstract class toRemit : PX.Data.BQL.BqlBool.Field<toRemit> { }
        #endregion

        #region ToggleByID
        [PXGuid]
        [PXSelector(typeof(Search<Users.pKID>),
                    SubstituteKey = typeof(Users.username))]
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

}