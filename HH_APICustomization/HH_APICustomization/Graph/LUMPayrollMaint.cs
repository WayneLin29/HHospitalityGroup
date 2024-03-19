using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.GL;
using PX.Objects.CS;
using PX.Data.BQL;

namespace HH_APICustomization.Graph
{
    public class LUMPayrollMaint : PXGraph<LUMPayrollMaint>
    {
        public PXSave<PayrollFilter> Save;
        public PXCancel<PayrollFilter> Cancel;

        public PXFilter<PayrollFilter> Filter;

        [PXImport(typeof(LUMPayrollHour))]
        public SelectFrom<LUMPayrollHour>
               .Where<Brackets<LUMPayrollHour.branchID.IsEqual<PayrollFilter.branchID.FromCurrent>.Or<PayrollFilter.branchID.FromCurrent.IsNull>>
                 .And<Brackets<LUMPayrollHour.workingDate.IsGreaterEqual<PayrollFilter.dateFrom.FromCurrent>.Or<PayrollFilter.dateFrom.FromCurrent.IsNull>>>
                 .And<Brackets<LUMPayrollHour.workingDate.IsLessEqual<PayrollFilter.dateTo.FromCurrent>.Or<PayrollFilter.dateTo.FromCurrent.IsNull>>>
                 .And<Brackets<LUMPayrollHour.approved.IsEqual<False>.Or<LUMPayrollHour.approved.IsNull>>>>
               .View payrollHour;

        [PXImport(typeof(LUMPayrollAdjustment))]
        public SelectFrom<LUMPayrollAdjustment>
               .Where<Brackets<LUMPayrollAdjustment.branchID.IsEqual<PayrollFilter.branchID.FromCurrent>.Or<PayrollFilter.branchID.FromCurrent.IsNull>>
                 .And<Brackets<LUMPayrollAdjustment.adjustmentDate.IsGreaterEqual<PayrollFilter.dateFrom.FromCurrent>.Or<PayrollFilter.dateFrom.FromCurrent.IsNull>>>
                 .And<Brackets<LUMPayrollAdjustment.adjustmentDate.IsLessEqual<PayrollFilter.dateTo.FromCurrent>.Or<PayrollFilter.dateTo.FromCurrent.IsNull>>>
                 .And<Brackets<LUMPayrollAdjustment.approved.IsEqual<False>.Or<LUMPayrollAdjustment.approved.IsNull>>>>
               .View payrollAdjustment;

        #region Events

        public virtual void _(Events.RowDeleting<LUMPayrollHour> e)
        {
            if (e.Row?.Approved ?? false)
                throw new PXException("Can not delete approved record");
        }

        public virtual void _(Events.RowDeleting<LUMPayrollAdjustment> e)
        {
            if (e.Row?.Approved ?? false)
                throw new PXException("Can not delete approved record");
        }

        #endregion
    }

    [Serializable]
    public class PayrollFilter : IBqlTable
    {
        [PXInt]
        [PXUIField(DisplayName = "Branch")]
        [Branch(useDefaulting: false)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }

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
