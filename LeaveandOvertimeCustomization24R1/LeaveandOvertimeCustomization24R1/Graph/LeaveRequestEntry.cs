using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveAndOvertimeCustomization.DAC;
using PX.Data.BQL.Fluent;
using PX.Objects.EP;
using System.Collections;
using PX.Objects.CR;
using PX.Data.BQL;
using PX.Objects.AM;
using PX.Objects.CS;
using LeaveAndOvertimeCustomization.Descriptor;

namespace LeaveAndOvertimeCustomization
{
    public class LeaveRequestEntry : PXGraph<LeaveRequestEntry, LumLeaveRequest>
    {
        public PXSave<LumLeaveRequest> save;
        public PXCancel<LumLeaveRequest> cancel;

        [PXHidden]
        public PXSetup<LumLeaveAndOvertimePreference> setup;

        [PXHidden]
        public PXSetup<LumLeaveRequestApproval> leaveApproval;

        [PXHidden]
        public SelectFrom<EPEmployee>.Where<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>>.View currentEmployee;

        [PXHidden]
        public SelectFrom<LumLeaveType>.Where<LumLeaveType.leaveType.IsEqual<LumLeaveRequest.leaveType.FromCurrent>>.View leaveTypeInfo;

        [PXViewName("Document")]
        public SelectFrom<LumLeaveRequest>.View document;

        [PXViewName("Transaction")]
        public SelectFrom<LumLeaveType>
               .InnerJoin<LumLeaveRequest>.On<LumLeaveType.leaveType.IsEqual<LumLeaveRequest.leaveType>>
               .InnerJoin<EPEmployee>.On<LumLeaveRequest.requestEmployeeID.IsEqual<EPEmployee.bAccountID>>
               .Where<LumLeaveRequest.leaveStart.FromCurrent.IsBetween<LumLeaveRequest.leaveStart, LumLeaveRequest.leaveEnd>
                   .And<LumLeaveRequest.refNbr.IsNotEqual<LumLeaveRequest.refNbr.FromCurrent>>>
               .View transaction;

        [PXViewName("Approval Details")]
        public EPApprovalAutomation<LumLeaveRequest, LumLeaveRequest.approved, LumLeaveRequest.rejected, LumLeaveRequest.hold, LumLeaveRequestApproval> Approval;

        #region Action

        public PXAction<LumLeaveRequest> Action;
        public PXAction<LumLeaveRequest> Approve;
        public PXAction<LumLeaveRequest> Reject;
        public PXAction<LumLeaveRequest> Submit;
        public PXAction<LumLeaveRequest> CancelRequest;

        [PXButton(MenuAutoOpen = true)]
        [PXUIField(DisplayName = "Actions", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        public virtual IEnumerable action(PXAdapter adapter)
        {
            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Approve", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable approve(PXAdapter adapter)
        {
            LumLeaveRequest doc = this.document.Current;
            if (doc != null)
            {
                doc.Approved = true;
                doc.Status = LumLeaveRequestStatus.Approved;

                this.document.Update(doc);
                this.save.Press();
                UpdateStatusMaunal(doc);
            }

            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Reject", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable reject(PXAdapter adapter)
        {
            LumLeaveRequest doc = this.document.Current;
            if (doc != null)
            {
                doc.Rejected = true;
                doc.Status = LumLeaveRequestStatus.Rejected;

                this.document.Update(doc);
                this.save.Press();
            }

            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Cancel", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable cancelRequest(PXAdapter adapter)
        {
            var doc = this.document.Current;
            // Delete Pending Approval
            var pendingList = SelectFrom<EPApproval>
                               .Where<EPApproval.refNoteID.IsEqual<P.AsGuid>
                                 .And<EPApproval.status.IsEqual<LumLeaveRequestStatus.pendingApproval>>>
                               .View.Select(this, doc.NoteID).RowCast<EPApproval>().ToList();
            pendingList.ForEach(x => { this.Approval.Delete(x); });
            doc.Status = LumLeaveRequestStatus.Cancel;
            this.document.Update(doc);
            this.save.Press();

            // Get notification template
            PX.SM.Notification notification = SelectFrom<PX.SM.Notification>
                                              .Where<PX.SM.Notification.notificationID.IsEqual<P.AsInt>>
                                              .View.Select(this, this.leaveApproval.Current?.CancelNotificationID)
                                              .RowCast<PX.SM.Notification>().FirstOrDefault();
            if (notification == null)
                PXTrace.WriteError("Can not find notification template 'Leave Request Cancel Notification' ");
            else
            {
                // Send Cancel email for Approved 
                var approvedList = SelectFrom<EPApproval>
                                   .Where<EPApproval.refNoteID.IsEqual<P.AsGuid>
                                     .And<EPApproval.status.IsEqual<LumLeaveRequestStatus.approved>>>
                                   .View.Select(this, doc.NoteID).RowCast<EPApproval>().ToList();
                foreach (var row in approvedList)
                {
                    try
                    {
                        var contactData = Contact.PK.Find(this, row.ApprovedByID, PKFindOptions.None);
                        if (contactData == null)
                            continue;
                        TemplateNotificationGenerator sender = TemplateNotificationGenerator.Create(this.document.Current, notification.NotificationID.Value);
                        sender.MailAccountId = (notification.NFrom.HasValue)
                                               ? notification.NFrom.Value
                                               : PX.Data.EP.MailAccountManager.DefaultMailAccountID;
                        sender.RefNoteID = this.document.Current.NoteID;
                        sender.Owner = this.document.Current.OwnerID;
                        sender.To = contactData.EMail;
                        var isSuccess = sender.Send().Any();
                        if (!isSuccess)
                            throw new Exception("Failed to send E-mail.");
                    }
                    catch (Exception ex)
                    {
                        throw new PXException(ex.Message);
                    }
                }
            }
            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Submit", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable submit(PXAdapter adapter)
        {
            LumLeaveRequest doc = this.document.Current;
            // valid
            Validsupplemental();
            if (doc != null)
            {
                this.document.Cache.SetValueExt<LumLeaveRequest.hold>(doc, false);
                this.document.Update(doc);

                // 如果沒有進入簽核則直接將狀態改為Approve
                if (SelectFrom<EPApproval>.Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>.View.Select(this, doc.NoteID).Count == 0)
                {
                    this.document.Cache.SetValue<LumLeaveRequest.approved>(doc, true);
                    this.document.Cache.SetValue<LumLeaveRequest.status>(doc, LumLeaveRequestStatus.Approved);
                    this.document.Update(doc);
                }

                this.save.Press();
            }
            return adapter.Get();
        }

        #endregion

        #region CacheAttached
        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(LumLeaveRequest.description), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void _(Events.CacheAttached<EPApproval.descr> e) { }
        #endregion

        #region Override & Delegate

        public IEnumerable Transaction()
        {

            PXView select = new PXView(this, true, transaction.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            List<object> result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;


            return CheckIsApprovalOwner(this.document.Current) ? result : Enumerable.Empty<object>();
        }

        public override void Persist()
        {
            Valid();
            base.Persist();
        }

        #endregion

        #region Event

        public virtual void _(Events.RowSelecting<LumLeaveRequest> e)
        {
            if (e.Row is LumLeaveRequest row)
            {
                using (new PXConnectionScope())
                {
                    //If Approvals are enabled, check if the current user is the set approver
                    if (setup.Current.LeaveRequestApproval ?? false)
                        row.IsApprover = CheckIsApprovalOwner(e.Row);
                }
            }
        }

        public virtual void _(Events.RowSelected<LumLeaveRequest> e)
        {
            if (e.Row != null && e.Row is LumLeaveRequest row)
            {
                if (this.setup.Current.LeaveRequestApproval ?? false)
                {
                    this.Approve.SetVisible(row.Status == LumLeaveRequestStatus.PendingApproval && (row.IsApprover ?? false));
                    this.Reject.SetVisible(row.Status == LumLeaveRequestStatus.PendingApproval && (row.IsApprover ?? false));
                }
                else
                {
                    this.Approve.SetVisible(row.Status == LumLeaveRequestStatus.PendingApproval);
                    this.Reject.SetVisible(row.Status == LumLeaveRequestStatus.PendingApproval);
                }
                // setting submit
                this.Submit.SetVisible(row.Status == LumLeaveRequestStatus.OnHold && row.RefNbr != "<NEW>");
                // setting cancel
                this.CancelRequest.SetVisible(row.Status != LumLeaveRequestStatus.Cancel);
                // setting field Enable
                PXUIFieldAttribute.SetEnabled<LumLeaveRequest.leaveStart>(e.Cache, null, row.Status == LumLeaveRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumLeaveRequest.leaveEnd>(e.Cache, null, row.Status == LumLeaveRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumLeaveRequest.delegateEmployeeID>(e.Cache, null, row.Status == LumLeaveRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumLeaveRequest.workgroupID>(e.Cache, null, row.Status == LumLeaveRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumLeaveRequest.leaveType>(e.Cache, null, row.Status == LumLeaveRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumLeaveRequest.description>(e.Cache, null, row.Status == LumLeaveRequestStatus.OnHold);

                // setting EntitledAnnualLeave
                PXUIFieldAttribute.SetVisible<LumLeaveRequest.entitledAnnualLeaveTimes>(e.Cache, null, this.leaveTypeInfo.Select().RowCast<LumLeaveType>().FirstOrDefault()?.IsAnnualLeave ?? false);
                PXUIFieldAttribute.SetVisible<LumLeaveRequest.remainingAvailableHours>(e.Cache, null, !(this.leaveTypeInfo.Select().RowCast<LumLeaveType>().FirstOrDefault()?.IsAnnualLeave ?? false));

                // 計算所有時數
                CalculateAllTimes(row);
            }

        }

        public virtual void _(Events.FieldUpdated<LumLeaveRequest.hold> e)
        {
            if (e.Row != null && e.Row is LumLeaveRequest row)
            {
                if (!row.Hold ?? false)
                    row.Status = LumLeaveRequestStatus.PendingApproval;
                else
                {
                    row.Status = LumLeaveRequestStatus.OnHold;
                    row.Rejected = false;
                    row.Approved = false;
                }
            }
        }

        public virtual void _(Events.FieldUpdated<LumLeaveRequest.delegateEmployeeID> e)
        {
            if (e.Row != null && e.Row is LumLeaveRequest row && e.NewValue != null)
            {
                var approvedRequest = SelectFrom<LumLeaveRequest>
                                      .Where<LumLeaveRequest.requestEmployeeID.IsEqual<P.AsInt>
                                        .And<LumLeaveRequest.status.IsEqual<P.AsString>
                                        .Or<LumLeaveRequest.status.IsEqual<P.AsString>>>>
                                      .View.Select(this, (int)e.NewValue, LumLeaveRequestStatus.Approved, LumLeaveRequestStatus.PendingApproval)
                                      .RowCast<LumLeaveRequest>().FirstOrDefault(x => x.LeaveStart >= row?.LeaveStart && x.LeaveStart <= row?.LeaveEnd);
                if (approvedRequest != null)
                    e.Cache.RaiseExceptionHandling<LumLeaveRequest.delegateEmployeeID>(e.Row, row.DelegateEmployeeID,
                            new PXSetPropertyException<LumLeaveRequest.delegateEmployeeID>("Your deputy employee is on leave, please choose other employee.", PXErrorLevel.Warning));
            }
        }

        public virtual void _(Events.FieldUpdated<LumLeaveRequest.leaveStart> e)
        {
            if (e.Row != null && e.Row is LumLeaveRequest row && e.NewValue != null)
            {
                var request = SelectFrom<LumLeaveRequest>
                                      .Where<LumLeaveRequest.delegateEmployeeID.IsEqual<P.AsInt>
                                        .And<LumLeaveRequest.status.IsEqual<P.AsString>
                                        .Or<LumLeaveRequest.status.IsEqual<P.AsString>>>>
                                      .View.Select(this, row.RequestEmployeeID, LumLeaveRequestStatus.Approved, LumLeaveRequestStatus.PendingApproval)
                                      .RowCast<LumLeaveRequest>().FirstOrDefault(x => (DateTime)e.NewValue >= x.LeaveStart && (DateTime)e.NewValue <= x.LeaveEnd);
                if (request != null)
                {
                    var emp = SelectFrom<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<P.AsInt>>.View.Select(this, request.RequestEmployeeID).TopFirst;
                    e.Cache.RaiseExceptionHandling<LumLeaveRequest.leaveStart>(e.Row, e.NewValue,
                            new PXSetPropertyException<LumLeaveRequest.leaveStart>($"You are the deputy of {emp.AcctName} during this period.", PXErrorLevel.Warning));
                }
            }
            CalculateAllTimes((LumLeaveRequest)e.Row);
        }

        public virtual void _(Events.FieldUpdated<LumLeaveRequest.leaveEnd> e)
            => CalculateAllTimes((LumLeaveRequest)e.Row);

        public virtual void _(Events.FieldUpdated<LumLeaveRequest.leaveType> e)
        {
            // setting EntitledAnnualLeave
            PXUIFieldAttribute.SetVisible<LumLeaveRequest.entitledAnnualLeaveTimes>(e.Cache, null, this.leaveTypeInfo.Select().RowCast<LumLeaveType>().FirstOrDefault()?.IsAnnualLeave ?? false);
            CalculateAllTimes((LumLeaveRequest)e.Row);
        }

        public virtual void _(Events.RowDeleting<LumLeaveRequest> e)
        {
            if (e.Row != null && e.Row.Status != LumLeaveRequestStatus.OnHold)
                throw new PXException("Can not delete this Document");
        }

        public virtual void _(Events.FieldVerifying<LumLeaveRequest.leaveStart> e)
        {
            if (e.Row != null && e.Row is LumLeaveRequest row)
            {
                if (((DateTime)e.NewValue) > row.LeaveEnd)
                    e.Cache.RaiseExceptionHandling<LumLeaveRequest.leaveStart>(
                        row,
                        null,
                        new PXSetPropertyException<LumLeaveRequest.leaveStart>("Leave Start time can not grather then Leave End time"));
            }
        }

        public virtual void _(Events.FieldVerifying<LumLeaveRequest.leaveEnd> e)
        {
            if (e.Row != null && e.Row is LumLeaveRequest row)
            {
                if (row.LeaveStart > ((DateTime)e.NewValue))
                    e.Cache.RaiseExceptionHandling<LumLeaveRequest.leaveEnd>(
                        row,
                        null,
                        new PXSetPropertyException<LumLeaveRequest.leaveEnd>("Leave End time can not small then Leave Start time"));
            }
        }

        protected virtual void _(Events.RowInserting<EPApproval> e)
        {
            if (e.Row is EPApproval)
            {
                LumLeaveRequest doc = document.Current;

                if (doc != null)
                {
                    // 寫入Approval 
                    e.Cache.SetValue<EPApproval.refNoteID>(e.Row, doc.NoteID);

                    EPEmployee employee = GetRequestEmployeeObject();
                    //EPEmployee employee = PXSelect<EPEmployee, Where<EPEmployee.userID, Equal<Required<EPEmployee.userID>>>>.Select(this, Accessinfo.UserID);
                    BAccount2 acct = SelectFrom<BAccount2>.Where<BAccount2.bAccountID.IsEqual<P.AsInt>>.View.Select(this, employee.BAccountID);
                    if (employee != null)
                    {
                        e.Cache.SetValue<EPApproval.documentOwnerID>(e.Row, acct.DefContactID);
                        e.Cache.SetValue<EPApproval.bAccountID>(e.Row, employee.BAccountID);
                    }
                    e.Cache.SetValue<EPApproval.docDate>(e.Row, doc.RequestDate);
                    e.Cache.SetValue<EPApproval.descr>(e.Row, doc.Description);

                    string details = $"Request Duration Hours: {doc.Duration}";

                    e.Cache.SetValue<EPApproval.details>(e.Row, details);
                }
            }
        }

        #endregion

        #region Method

        /// <summary> 檢查是否需要附件 </summary>
        public void Validsupplemental()
        {
            LumLeaveType info = this.leaveTypeInfo.Select();
            var currentFileIDs = PXNoteAttribute.GetFileNotes(this.document.Cache, this.document.Current);
            if ((info.IsAttachedRequired ?? false) && currentFileIDs.Count() == 0)
                throw new PXException("Please upload the supplemental documents for leave request");
        }

        /// <summary> 計算所有時間 </summary>
        public void CalculateAllTimes(LumLeaveRequest row)
        {
            CalculateDuartion(row);
            CalculateTotalApprovedTimes(row);
            CalculateTotalPendingTimes(row);
            CalculateAvailableAnnualTimes(row);
            CalculateRemainingAvailableTimes(row);
        }

        /// <summary> 計算請假時間 </summary>
        public void CalculateDuartion(LumLeaveRequest row)
        {
            if (row != null && row.LeaveStart.HasValue && row.LeaveEnd.HasValue)
            {
                EPEmployee employee = GetRequestEmployeeObject();
                if (employee == null)
                    return;
                this.document.SetValueExt<LumLeaveRequest.duration>(row, new HRHelper().GetLeaveDuration(employee, row.LeaveStart, row.LeaveEnd, new HRHelper().GetLeaveTypeInfo(row.LeaveType)?.IsOnlyWorkDay));
            }
        }

        /// <summary> 計算已核准請假時間(年) </summary>
        public void CalculateTotalApprovedTimes(LumLeaveRequest row)
        {
            if (row != null && !string.IsNullOrEmpty(row.LeaveType) && row.LeaveStart.HasValue && row.LeaveEnd.HasValue)
            {
                var helper = new HRHelper();
                EPEmployee employee = GetRequestEmployeeObject();
                if (employee == null)
                    return;
                var approvedRequest = SelectFrom<LumLeaveRequest>
                                      .Where<LumLeaveRequest.requestEmployeeID.IsEqual<P.AsInt>
                                        .And<LumLeaveRequest.status.IsEqual<P.AsString>>
                                        .And<LumLeaveRequest.leaveType.IsEqual<P.AsString>>>
                                      .View.Select(this, employee.BAccountID, LumLeaveRequestStatus.Approved, row.LeaveType, row.RefNbr).RowCast<LumLeaveRequest>().ToList();
                var result = (decimal)0;
                foreach (var item in approvedRequest.Where(x => x.LeaveStart?.Year == row.LeaveStart?.Year))
                    result += helper.GetLeaveDuration(employee, item.LeaveStart, item.LeaveEnd, helper.GetLeaveTypeInfo(row.LeaveType)?.IsOnlyWorkDay);
                this.document.Cache.SetValueExt<LumLeaveRequest.approvedLeaveTimes>(row, result);
            }
        }

        /// <summary> 計算待核准請假時間(年) </summary>
        public void CalculateTotalPendingTimes(LumLeaveRequest row)
        {
            if (row != null && !string.IsNullOrEmpty(row.LeaveType) && row.LeaveStart.HasValue && row.LeaveEnd.HasValue)
            {
                var helper = new HRHelper();
                EPEmployee employee = GetRequestEmployeeObject();
                if (employee == null)
                    return;
                var pendingRequest = SelectFrom<LumLeaveRequest>
                                      .Where<LumLeaveRequest.requestEmployeeID.IsEqual<P.AsInt>
                                        .And<LumLeaveRequest.status.IsEqual<P.AsString>>
                                        .And<LumLeaveRequest.leaveType.IsEqual<P.AsString>>>
                                      .View.Select(this, employee.BAccountID, LumLeaveRequestStatus.PendingApproval, row.LeaveType).RowCast<LumLeaveRequest>().ToList();
                var result = (decimal)0;
                foreach (var item in pendingRequest.Where(x => x.LeaveStart?.Year == row.LeaveStart?.Year))
                    result += helper.GetLeaveDuration(employee, item.LeaveStart, item.LeaveEnd, helper.GetLeaveTypeInfo(row.LeaveType)?.IsOnlyWorkDay);
                this.document.Cache.SetValueExt<LumLeaveRequest.pendingApprovalTimes>(row, result);
            }
        }

        /// <summary> 計算該Employee可使用的年假 </summary>
        public void CalculateAvailableAnnualTimes(LumLeaveRequest row)
        {
            if (row != null)
            {
                var result = new HRHelper().GetAvailableAnnualLeaveTime(
                    GetRequestEmployeeObject(),
                    row.LeaveType,
                    row.RefNbr,
                    row?.LeaveStart,
                    row?.LeaveEnd);
                this.document.SetValueExt<LumLeaveRequest.entitledAnnualLeaveTimes>(row, result);

            }
        }

        /// <summary> 計算該Employee剩餘請假時數 </summary>
        public void CalculateRemainingAvailableTimes(LumLeaveRequest row)
        {
            if (row != null)
            {
                EPEmployee employee = GetRequestEmployeeObject();
                var availableTime = new HRHelper().GetAvailableLeaveTime(employee, row.LeaveType, row.RefNbr, row.LeaveStart, row.LeaveEnd, new HRHelper().GetLeaveTypeInfo(row.LeaveType)?.IsOnlyWorkDay);
                this.document.SetValueExt<LumLeaveRequest.remainingAvailableHours>(row, availableTime);
            }
        }

        /// <summary> 手動更新Status </summary>
        public void UpdateStatusMaunal(LumLeaveRequest doc)
        {
            if (SelectFrom<EPApproval>
               .Where<EPApproval.status.IsEqual<P.AsString>
                    .And<EPApproval.refNoteID.IsEqual<P.AsGuid>>>
               .View.Select(this, LumLeaveRequestStatus.PendingApproval, doc.NoteID).Count > 0 && doc.Status == LumLeaveRequestStatus.Approved)
            {
                PXUpdate<Set<LumLeaveRequest.approved, Required<LumLeaveRequest.approved>,
                         Set<LumLeaveRequest.status, Required<LumLeaveRequest.status>>>,
                         LumLeaveRequest,
                         Where<LumLeaveRequest.refNbr, Equal<Required<LumLeaveRequest.refNbr>>>>.Update(this, false, LumLeaveRequestStatus.PendingApproval, doc.RefNbr);
            }
        }

        /// <summary> 檢查是不是審核人 </summary>
        public bool CheckIsApprovalOwner(LumLeaveRequest doc)
        {
            var approvalRecord = SelectFrom<EPApproval>
                                 .Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>
                                 .View.Select(this, doc.NoteID).RowCast<EPApproval>().FirstOrDefault(x => x.Status == LumLeaveRequestStatus.PendingApproval);
            if (approvalRecord != null)
            {
                BAccount acct = SelectFrom<BAccount>
                           .InnerJoin<EPEmployee>.On<BAccount.bAccountID.IsEqual<EPEmployee.bAccountID>>
                           .Where<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>>.View.Select(this);
                if (acct.DefContactID == approvalRecord.OwnerID)
                    return true;
            }
            return false;
        }

        public void Valid()
        {
            // Valid Available time
            if (this.document.Current != null)
            {
                var row = this.document.Current;

                // Rejected不用做檢核
                if (row.Status == LumLeaveRequestStatus.Rejected)
                    return;

                EPEmployee employee = GetRequestEmployeeObject();
                LumLeaveType typeInfo = this.leaveTypeInfo.Select();

                // 檢查是否有重複的請假時段
                var requests = SelectFrom<LumLeaveRequest>
                               .Where<LumLeaveRequest.refNbr.IsNotEqual<P.AsString>
                                 .And<LumLeaveRequest.requestEmployeeID.IsEqual<P.AsInt>>
                                 .And<LumLeaveRequest.status.IsEqual<P.AsString>.Or<LumLeaveRequest.status.IsEqual<P.AsString>>>>
                              .View.Select(this, row.RefNbr, employee.BAccountID, LumLeaveRequestStatus.Approved, LumLeaveRequestStatus.PendingApproval).RowCast<LumLeaveRequest>().ToList();
                var overLapData = requests.Where(x => x.LeaveStart == row.LeaveStart && x.LeaveEnd == row.LeaveEnd);
                if (overLapData.Count() > 0)
                    throw new PXException($"There is time overlap for this leave request {overLapData.FirstOrDefault().RefNbr}");

                // 檢查是否符合最少時數
                if (row.Duration < typeInfo?.MinLeaveHour)
                    throw new PXException("The leave request hours is less than allowed minimum hours per leave");

                // 檢查是否超休
                if (typeInfo?.IsAnnualLeave ?? false)
                {
                    if (row?.Duration > row?.EntitledAnnualLeaveTimes)
                        throw new PXException("The leave request hours will exceed the total annual leave entitled");
                }
                else
                {
                    var availableTime = new HRHelper().GetAvailableLeaveTime(employee, row.LeaveType, row.RefNbr, row.LeaveStart, row.LeaveEnd, new HRHelper().GetLeaveTypeInfo(row.LeaveType)?.IsOnlyWorkDay);
                    if (row?.Duration > availableTime)
                        throw new PXException("The leave request hours will exceed the remaining available hours");
                }

            }
        }

        /// <summary> 取得Leqve Request 申請人的Employee的物件 </summary>
        public EPEmployee GetRequestEmployeeObject()
            => this.document.Current != null ? SelectFrom<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<P.AsInt>>.View.Select(this, this.document.Current.RequestEmployeeID) : null;

        #endregion

    }
}
