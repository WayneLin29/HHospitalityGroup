using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveAndOvertimeCustomization.Descriptor
{
    public static class AssignmentMapType
    {
        public class AssignmentMapTypeLumLeave : PX.Data.BQL.BqlString.Constant<AssignmentMapTypeLumLeave>
        {
            public AssignmentMapTypeLumLeave() : base(typeof(LumLeaveRequest).FullName) { }
        }

        public class AssignmentMapTypeLumOvertime : PX.Data.BQL.BqlString.Constant<AssignmentMapTypeLumOvertime>
        {
            public AssignmentMapTypeLumOvertime() : base(typeof(LumOvertimeRequest).FullName) { }
        }
    }
}
