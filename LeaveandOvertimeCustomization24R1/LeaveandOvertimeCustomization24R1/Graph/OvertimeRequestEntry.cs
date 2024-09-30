using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.EP;
using PX.Objects.CR;
using LeaveAndOvertimeCustomization.Descriptor;

namespace LeaveAndOvertimeCustomization.Graph
{
    public class OvertimeRequestEntry : PXGraph<OvertimeRequestEntry, LumOvertimeRequest>
    {
        public PXSave<LumOvertimeRequest> save;
        public PXCancel<LumOvertimeRequest> cancel;

        [PXHidden]
        public PXSetup<LumLeaveAndOvertimePreference> setup;

        [PXHidden]
        public PXSetup<LumOvertimeApproval> OvertimeApproval;

        [PXHidden]
        public SelectFrom<EPEmployee>.Where<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>>.View currentEmployee;

        [PXViewName("Document")]
        public SelectFrom<LumOvertimeRequest>.View document;

        [PXHidden]
        public PXSetup<LumOvertimeApproval> overtimeApproval;

        [PXViewName("Approval Details")]
        public EPApprovalAutomation<LumOvertimeRequest, LumOvertimeRequest.approved, LumOvertimeRequest.rejected, LumOvertimeRequest.hold, LumOvertimeApproval> Approval;

        [PXHidden]
        public SelectFrom<LumEmployeeCompensated>.View compensatedTrans;

        #region Action

        public PXAction<LumOvertimeRequest> Action;
        public PXAction<LumOvertimeRequest> Approve;
        public PXAction<LumOvertimeRequest> Reject;
        public PXAction<LumOvertimeRequest> Submit;
        public PXAction<LumOvertimeRequest> CancelRequest;

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
            LumOvertimeRequest doc = this.document.Current;
            if (doc != null)
            {
                doc.Approved = true;
                doc.Status = LumOvertimeRequestStatus.Approved;

                this.document.Update(doc);
                this.save.Press();

                // 如果還有待審核則手動將Status改成Pending;沒有且是補休則將資料寫入補休表
                if (SelectFrom<EPApproval>
                    .Where<EPApproval.status.IsEqual<P.AsString>
                         .And<EPApproval.refNoteID.IsEqual<P.AsGuid>>>
                    .View.Select(this, LumOvertimeRequestStatus.PendingApproval, doc.NoteID).Count > 0 && doc.Status == LumOvertimeRequestStatus.Approved)
                {
                    PXUpdate<Set<LumOvertimeRequest.approved, Required<LumOvertimeRequest.approved>,
                             Set<LumOvertimeRequest.status, Required<LumOvertimeRequest.status>>>,
                             LumOvertimeRequest,
                             Where<LumOvertimeRequest.refNbr, Equal<Required<LumOvertimeRequest.refNbr>>>>.Update(this, false, LumOvertimeRequestStatus.PendingApproval, doc.RefNbr);
                }
                else if (doc.OvertimeType == "Compensated")
                    TransferCompensated(doc);
            }

            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Reject", Visible = false, MapEnableRights = PXCacheRights.Select)]
        public IEnumerable reject(PXAdapter adapter)
        {
            LumOvertimeRequest doc = this.document.Current;
            if (doc != null)
            {
                doc.Rejected = true;
                doc.Status = LumOvertimeRequestStatus.Rejected;

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
                                 .And<EPApproval.status.IsEqual<LumOvertimeRequestStatus.pendingApproval>>>
                               .View.Select(this, doc.NoteID).RowCast<EPApproval>().ToList();
            pendingList.ForEach(x => { this.Approval.Delete(x); });
            doc.Status = LumOvertimeRequestStatus.Cancel;
            this.document.Update(doc);
            this.save.Press();

            PX.SM.Notification notification = SelectFrom<PX.SM.Notification>
                                              .Where<PX.SM.Notification.notificationID.IsEqual<P.AsInt>>
                                              .View.Select(this, this.overtimeApproval.Current?.CancelNotificationID)
                                              .RowCast<PX.SM.Notification>().FirstOrDefault();
            if (notification == null)
                PXTrace.WriteError("Can not find notification template 'Overtime Request Cancel Notification' ");
            else
            {
                // Send Cancel email for Approved 
                var approvedList = SelectFrom<EPApproval>
                                   .Where<EPApproval.refNoteID.IsEqual<P.AsGuid>
                                     .And<EPApproval.status.IsEqual<LumOvertimeRequestStatus.approved>>>
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
            LumOvertimeRequest doc = this.document.Current;
            if (doc != null)
            {
                this.document.Cache.SetValueExt<LumOvertimeRequest.hold>(doc, false);
                this.document.Update(doc);

                // 如果沒有進入簽核則直接將狀態改為Approve
                if (SelectFrom<EPApproval>.Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>.View.Select(this, doc.NoteID).Count == 0)
                {
                    this.document.Cache.SetValue<LumOvertimeRequest.approved>(doc, true);
                    this.document.Cache.SetValue<LumOvertimeRequest.status>(doc, LumOvertimeRequestStatus.Approved);
                    this.document.Update(doc);
                    if (doc.OvertimeType == "Compensated")
                    {
                        TransferCompensated(doc);
                    }
                }

                this.save.Press();
            }
            return adapter.Get();
        }

        #endregion

        #region CacheAttached
        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(LumOvertimeRequest.description), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void _(Events.CacheAttached<EPApproval.descr> e) { }
        #endregion

        #region Delegate & Override

        public override void Persist()
        {
            Valid();
            base.Persist();
        }

        #endregion

        #region Events

        public virtual void _(Events.RowSelecting<LumOvertimeRequest> e)
        {
            if (e.Row is LumOvertimeRequest row)
            {
                using (new PXConnectionScope())
                {
                    //If Approvals are enabled, check if the current user is the set approver
                    if (setup.Current.LeaveRequestApproval ?? false)
                    {
                        EPEmployee approver = SelectFrom<EPEmployee>
                                              .InnerJoin<BAccount2>.On<EPEmployee.bAccountID.IsEqual<BAccount2.bAccountID>>
                                              .InnerJoin<EPRule>.On<BAccount2.defContactID.IsEqual<EPRule.ownerID>>
                                              .InnerJoin<EPAssignmentMap>.On<EPRule.assignmentMapID.IsEqual<EPAssignmentMap.assignmentMapID>>
                                              .Where<EPAssignmentMap.assignmentMapID.IsEqual<P.AsInt>>.View.Select(this, overtimeApproval.Current.AssignmentMapID);
                        if (CheckIsApprovalOwner(e.Row))
                            row.IsApprover = (approver.UserID == Accessinfo.UserID);
                    }
                }
            }
        }

        public virtual void _(Events.RowSelected<LumOvertimeRequest> e)
        {
            if (e.Row != null && e.Row is LumOvertimeRequest row)
            {
                if (this.setup.Current.LeaveRequestApproval ?? false)
                {
                    this.Approve.SetVisible(row.Status == LumOvertimeRequestStatus.PendingApproval && (row.IsApprover ?? false));
                    this.Reject.SetVisible(row.Status == LumOvertimeRequestStatus.PendingApproval && (row.IsApprover ?? false));
                }
                else
                {
                    this.Approve.SetVisible(row.Status == LumOvertimeRequestStatus.PendingApproval);
                    this.Reject.SetVisible(row.Status == LumOvertimeRequestStatus.PendingApproval);
                }
                // setting submit
                this.Submit.SetVisible(row.Status == LumOvertimeRequestStatus.OnHold && row.RefNbr != "<NEW>");
                // setting cancel
                this.CancelRequest.SetVisible(row.Status != LumOvertimeRequestStatus.Cancel);
                // setting Onhold check box
                PXUIFieldAttribute.SetEnabled<LumOvertimeRequest.overtimeStart>(e.Cache, null, row.Status == LumOvertimeRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumOvertimeRequest.overTimeEnd>(e.Cache, null, row.Status == LumOvertimeRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumOvertimeRequest.overtimeType>(e.Cache, null, row.Status == LumOvertimeRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumOvertimeRequest.description>(e.Cache, null, row.Status == LumOvertimeRequestStatus.OnHold);
                PXUIFieldAttribute.SetEnabled<LumOvertimeRequest.workgroupID>(e.Cache, null, row.Status == LumOvertimeRequestStatus.OnHold);

                // 計算所有時數
                CalculateAllTimes(row);
            }

        }

        public virtual void _(Events.FieldVerifying<LumOvertimeRequest.overtimeStart> e)
        {
            if (e.Row != null && e.Row is LumOvertimeRequest row)
            {
                if (((DateTime)e.NewValue) > row.OverTimeEnd)
                    e.Cache.RaiseExceptionHandling<LumOvertimeRequest.overtimeStart>(
                        row,
                        e.NewValue,
                        new PXSetPropertyException<LumOvertimeRequest.overtimeStart>("Over Start time can not grather then Leave End time"));
            }
        }

        public virtual void _(Events.FieldVerifying<LumOvertimeRequest.overTimeEnd> e)
        {
            if (e.Row != null && e.Row is LumOvertimeRequest row)
            {
                if (row.OvertimeStart > ((DateTime)e.NewValue))
                    e.Cache.RaiseExceptionHandling<LumOvertimeRequest.overTimeEnd>(
                        row,
                        e.NewValue,
                        new PXSetPropertyException<LumOvertimeRequest.overTimeEnd>("Over End time can not small then Leave Start time"));
            }
        }

        public virtual void _(Events.FieldUpdated<LumOvertimeRequest.hold> e)
        {
            if (e.Row != null && e.Row is LumOvertimeRequest row)
            {
                if (!row.Hold ?? false)
                    row.Status = LumOvertimeRequestStatus.PendingApproval;
                else
                {
                    row.Status = LumOvertimeRequestStatus.OnHold;
                    row.Rejected = false;
                    row.Approved = false;
                }
            }
        }

        protected virtual void _(Events.RowInserting<EPApproval> e)
        {
            if (e.Row is EPApproval)
            {
                LumOvertimeRequest doc = document.Current;

                if (doc != null)
                {
                    // 寫入Approval 
                    e.Cache.SetValue<EPApproval.refNoteID>(e.Row, doc.NoteID);

                    EPEmployee employee = GetRequestEmployeeObject();
                    BAccount2 acct = SelectFrom<BAccount2>.Where<BAccount2.bAccountID.IsEqual<P.AsInt>>.View.Select(this, employee.BAccountID);
                    if (employee != null)
                    {
                        e.Cache.SetValue<EPApproval.documentOwnerID>(e.Row, acct.DefContactID);
                        e.Cache.SetValue<EPApproval.bAccountID>(e.Row, employee.BAccountID);
                    }
                    e.Cache.SetValue<EPApproval.docDate>(e.Row, doc.RequestDate);
                    e.Cache.SetValue<EPApproval.descr>(e.Row, doc.Description);

                    string details = $"Request Holiday Days: {doc.HolidayDuration + doc.NationalHolidayDuration + doc.WorkDayDuration}";

                    e.Cache.SetValue<EPApproval.details>(e.Row, details);
                }
            }
        }

        public virtual void _(Events.FieldUpdated<LumOvertimeRequest.overtimeStart> e)
          => CalculateAllTimes((LumOvertimeRequest)e.Row);

        public virtual void _(Events.FieldUpdated<LumOvertimeRequest.overTimeEnd> e)
            => CalculateAllTimes((LumOvertimeRequest)e.Row);

        public virtual void _(Events.RowDeleting<LumOvertimeRequest> e)
        {
            if (e.Row != null && (e.Row.Status != LumOvertimeRequestStatus.OnHold))
                throw new PXException("Can not delete this Document");
        }

        #endregion

        #region Method

        public void Valid()
        {
            var row = this.document.Current;
            var preference = this.setup.Current;
            var approvedHours = new HRHelper().GetApprovedOvertimeByMonth(GetRequestEmployeeObject(), row.OvertimeStart, row.RefNbr);
            if (row.WorkDayDuration > preference?.MaxOTinWorkDay)
                throw new PXException($"The Overtime hours exceed the Maximum allowed OT hours({preference?.MaxOTinWorkDay})");
            if (row.HolidayDuration + row.NationalHolidayDuration > preference?.MaxOTinHoliday)
                throw new PXException($"The Overtime hours exceed the Maximum allowed OT hours(Holiday:{preference?.MaxOTinHoliday})");
            if (approvedHours + row.WorkDayDuration + row.NationalHolidayDuration + row.HolidayDuration > preference?.MaxOTPerMonth)
                throw new PXException($"The Overtime hours exceed the Maximum allowed OT hours per month(Approved:{approvedHours})");
        }

        /// <summary> 計算所有時間 </summary>
        public void CalculateAllTimes(LumOvertimeRequest row)
        {
            if (row != null && row.OvertimeStart.HasValue && row.OverTimeEnd.HasValue)
            {
                EPEmployee employee = GetRequestEmployeeObject();
                if (employee == null)
                    return;
                var calculateResult = new HRHelper().GetOvertimeDuration(employee, row.OvertimeStart, row.OverTimeEnd);
                this.document.Cache.SetValueExt<LumOvertimeRequest.workDayDuration>(row, calculateResult.wkTimes);
                this.document.Cache.SetValueExt<LumOvertimeRequest.holidayDuration>(row, calculateResult.HolidayTimes);
                this.document.Cache.SetValueExt<LumOvertimeRequest.nationalHolidayDuration>(row, calculateResult.NationalTimes);
            }
        }

        /// <summary> 檢查是不是審核人 </summary>
        public bool CheckIsApprovalOwner(LumOvertimeRequest doc)
        {
            var approvalRecord = SelectFrom<EPApproval>
                                 .Where<EPApproval.refNoteID.IsEqual<P.AsGuid>>
                                 .View.Select(this, doc.NoteID).RowCast<EPApproval>().FirstOrDefault(x => x.Status == LumOvertimeRequestStatus.PendingApproval);
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

        /// <summary> 取得Leqve Request 申請人的Employee的物件 </summary>
        public EPEmployee GetRequestEmployeeObject()
            => this.document.Current != null ? SelectFrom<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<P.AsInt>>.View.Select(this, this.document.Current.RequestEmployeeID) : null;

        public void TransferCompensated(LumOvertimeRequest doc)
        {
            var preference = this.setup.Current;
            LumEmployeeCompensated trans = this.compensatedTrans.Insert((LumEmployeeCompensated)this.compensatedTrans.Cache.CreateInstance());
            trans.RefNbr = doc.RefNbr;
            trans.EmployeeID = doc.RequestEmployeeID;
            trans.AvailableYear = doc.OvertimeStart?.Year;
            trans.TransferHours = doc.WorkDayDuration + doc.NationalHolidayDuration + doc.HolidayDuration;
            trans.AllowCarryForward = false;
            this.save.Press();
        }

        #endregion
    }
}
