using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.GL;

namespace HH_APICustomization.Graph
{
    public class LUMPayrollMaint : PXGraph<LUMPayrollMaint>
    {
        public PXSave<PayrollFilter> Save;
        public PXCancel<PayrollFilter> Cancel;

        public PXFilter<PayrollFilter> Filter;

        [PXImport(typeof(LUMPayrollHour))]
        public SelectFrom<LUMPayrollHour>
               .Where<LUMPayrollHour.branch.IsEqual<PayrollFilter.branch.FromCurrent>.Or<PayrollFilter.branch.FromCurrent.IsNull>
                 .And<LUMPayrollHour.workingDate.IsGreaterEqual<PayrollFilter.dateFrom.FromCurrent>.Or<PayrollFilter.dateFrom.FromCurrent.IsNull>>
                 .And<LUMPayrollHour.workingDate.IsLessEqual<PayrollFilter.dateTo.FromCurrent>.Or<PayrollFilter.dateTo.FromCurrent.IsNull>>>
               .View payrollHour;

        [PXImport(typeof(LUMPayrollAdjustment))]
        public SelectFrom<LUMPayrollAdjustment>
               .Where<LUMPayrollAdjustment.branch.IsEqual<PayrollFilter.branch.FromCurrent>.Or<PayrollFilter.branch.FromCurrent.IsNull>
                 .And<LUMPayrollAdjustment.adjustmentDate.IsGreaterEqual<PayrollFilter.dateFrom.FromCurrent>.Or<PayrollFilter.dateFrom.FromCurrent.IsNull>>
                 .And<LUMPayrollAdjustment.adjustmentDate.IsLessEqual<PayrollFilter.dateTo.FromCurrent>.Or<PayrollFilter.dateTo.FromCurrent.IsNull>>>
               .View payrollAdjustment;
    }

    [Serializable]
    public class PayrollFilter : IBqlTable
    {
        [PXInt]
        [PXUIField(DisplayName = "Branch")]
        [Branch(useDefaulting: false)]
        public virtual int? Branch { get; set; }
        public abstract class branch : PX.Data.BQL.BqlInt.Field<branch> { }

        [PXDate]
        [PXUIField(DisplayName = "DateFrom")]
        public virtual DateTime? DateFrom { get; set; }
        public abstract class dateFrom : PX.Data.BQL.BqlDateTime.Field<dateFrom> { }

        [PXDate]
        [PXUIField(DisplayName = "DateTo")]
        public virtual DateTime? DateTo { get; set; }
        public abstract class dateTo : PX.Data.BQL.BqlDateTime.Field<dateTo> { }
    }
}
