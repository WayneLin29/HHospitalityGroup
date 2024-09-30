using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveAndOvertimeCustomization.Graph
{
    public class EmployeeHolidaySchedule : PXGraph<EmployeeHolidaySchedule>
    {
        public SelectFrom<v_EmployeeHolidaySehedule>.View Document;
    }
}
