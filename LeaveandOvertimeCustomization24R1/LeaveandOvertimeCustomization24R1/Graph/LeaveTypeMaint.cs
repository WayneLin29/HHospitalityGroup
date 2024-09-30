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
    public class LeaveTypeMaint : PXGraph<LeaveTypeMaint>
    {
        public PXSave<LumLeaveType> save;
        public PXCancel<LumLeaveType> cancel;
        public SelectFrom<LumLeaveType>.View leaveTypeList;
    }
}
