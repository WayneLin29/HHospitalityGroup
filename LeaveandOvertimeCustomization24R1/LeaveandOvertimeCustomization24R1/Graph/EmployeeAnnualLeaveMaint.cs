using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveAndOvertimeCustomization
{
    public class EmployeeAnnualLeaveMaint : PXGraph<EmployeeAnnualLeaveMaint>
    {
        public PXSave<LumEmployeeAnnualLeave> save;
        public PXCancel<LumEmployeeAnnualLeave> cancel;

        public SelectFrom<LumEmployeeAnnualLeave>.View Document;

        public SelectFrom<LumEmployeeAnnualLeaveLine>
               .Where<LumEmployeeAnnualLeaveLine.employeeID.IsEqual<LumEmployeeAnnualLeave.employeeID.FromCurrent>
                   .And<LumEmployeeAnnualLeaveLine.leaveType.IsEqual<LumEmployeeAnnualLeaveLine.leaveType.FromCurrent>>>
               .View Transaction;

        public SelectFrom<LumEmployeeCompensated>
               .Where<LumEmployeeCompensated.employeeID.IsEqual<LumEmployeeAnnualLeave.employeeID.FromCurrent>>
               .View CompensatedTrans;

        #region Override & Delegate

        #endregion

        #region Events

        public virtual void _(Events.RowSelected<LumEmployeeAnnualLeave> e)
        {
            var doc = e.Row;
            var totalCompensated = SelectFrom<LumEmployeeCompensated>
                                   .Where<LumEmployeeCompensated.employeeID.IsEqual<P.AsInt>>
                                   .View.Select(this, doc.EmployeeID).RowCast<LumEmployeeCompensated>().ToList()
                                   .Where(x => x.AvailableYear == DateTime.Now.Year).Sum(x => x.TransferHours);
            var usedCompensated = SelectFrom<LumLeaveRequest>
                                  .Where<LumLeaveRequest.status.IsEqual<LumLeaveRequestStatus.approved>
                                    .And<LumLeaveRequest.requestEmployeeID.IsEqual<P.AsInt>>>
                                  .View.Select(this, doc.EmployeeID).RowCast<LumLeaveRequest>().ToList()
                                  .Where(x => x.LeaveStart?.Year == DateTime.Now.Year && x.LeaveType == "Compensated").Sum(x => x.Duration);
            this.Document.Cache.SetValueExt<LumEmployeeAnnualLeave.availCompensatedHrs>(doc, totalCompensated - usedCompensated);
            this.Document.Cache.SetValueExt<LumEmployeeAnnualLeave.usedCompensatedHrs>(doc, usedCompensated);
        }

        public virtual void _(Events.RowSelected<LumEmployeeAnnualLeaveLine> e)
            => e.Cache.SetValue<LumEmployeeAnnualLeaveLine.leaveDays>(e.Row, ((e.Row.LeaveHours ?? 0) + (e.Row.CarryForwardHours ?? 0)) / 8);

        public virtual void _(Events.FieldUpdated<LumEmployeeAnnualLeaveLine.leaveHours> e)
        {
            if (e.Row != null && e.Row is LumEmployeeAnnualLeaveLine row)
                e.Cache.SetValueExt<LumEmployeeAnnualLeaveLine.leaveDays>(row, (row.LeaveHours ?? 0) / 8);
        }

        #endregion

    }
}
