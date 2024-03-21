using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HH_Customization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;


namespace PX.Objects.EP
{
    public class EmployeeMaintSHExt : PXGraphExtension<EmployeeMaint>
    {
        public SelectFrom<LUMAccountability>
            .Where<LUMAccountability.bAccountID.IsEqual<EPEmployee.bAccountID.FromCurrent>>
            .View Accountabilitys;
    }
}
