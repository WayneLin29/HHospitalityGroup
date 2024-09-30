using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveAndOvertimeCustomization
{
    public class LeaveAndOvertimeSetupMaint : PXGraph<LeaveAndOvertimeSetupMaint>
    {
        public PXSave<LumLeaveAndOvertimePreference> save;
        public PXCancel<LumLeaveAndOvertimePreference> cancel;
        public SelectFrom<LumLeaveAndOvertimePreference>.View setup;
        public SelectFrom<LumLeaveRequestApproval>.View setupApproval;
        public SelectFrom<LumOvertimeApproval>.View OTSetupApproval;


        protected virtual void _(Events.FieldUpdated<LumLeaveAndOvertimePreference, LumLeaveAndOvertimePreference.leaveRequestApproval> e)
        {
            LumLeaveAndOvertimePreference row = e.Row;

            if (row != null)
            {
                foreach (LumLeaveRequestApproval setup in setupApproval.Select())
                {
                    setupApproval.SetValueExt<LumLeaveRequestApproval.isActive>(setup, row.LeaveRequestApproval);
                }

                foreach (LumOvertimeApproval setup in OTSetupApproval.Select())
                {
                    OTSetupApproval.SetValueExt<LumOvertimeApproval.isActive>(setup, row.OvertimeApproval);
                }
            }
        }

    }
}
