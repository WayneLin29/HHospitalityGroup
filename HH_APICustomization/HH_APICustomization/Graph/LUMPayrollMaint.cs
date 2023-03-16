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

namespace HH_APICustomization.Graph
{
    public class LUMPayrollMaint : PXGraph<LUMPayrollMaint>
    {
        public PXSave<PayrollFilter> Save;
        public PXCancel<PayrollFilter> Cancel;

        public PXFilter<PayrollFilter> Filter;

        [PXImport(typeof(LUMPayrollHour))]
        public SelectFrom<LUMPayrollHour>
               .Where<LUMPayrollHour.branchID.IsEqual<PayrollFilter.branchID.FromCurrent>.Or<PayrollFilter.branchID.FromCurrent.IsNull>
                 .And<LUMPayrollHour.workingDate.IsGreaterEqual<PayrollFilter.dateFrom.FromCurrent>.Or<PayrollFilter.dateFrom.FromCurrent.IsNull>>
                 .And<LUMPayrollHour.workingDate.IsLessEqual<PayrollFilter.dateTo.FromCurrent>.Or<PayrollFilter.dateTo.FromCurrent.IsNull>>>
               .View payrollHour;

        [PXImport(typeof(LUMPayrollAdjustment))]
        public SelectFrom<LUMPayrollAdjustment>
               .Where<LUMPayrollAdjustment.branchID.IsEqual<PayrollFilter.branchID.FromCurrent>.Or<PayrollFilter.branchID.FromCurrent.IsNull>
                 .And<LUMPayrollAdjustment.adjustmentDate.IsGreaterEqual<PayrollFilter.dateFrom.FromCurrent>.Or<PayrollFilter.dateFrom.FromCurrent.IsNull>>
                 .And<LUMPayrollAdjustment.adjustmentDate.IsLessEqual<PayrollFilter.dateTo.FromCurrent>.Or<PayrollFilter.dateTo.FromCurrent.IsNull>>>
               .View payrollAdjustment;

        #region Events

        public virtual void _(Events.RowSelected<LUMPayrollHour> e)
        {
            if (e.Row?.Approved ?? false)
            {
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.branchID>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.workingDate>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.employeeID>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.earningType>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.hour>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.remark>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollHour.cutOffDate>(e.Cache, e.Row, false);
            }
        }

        public virtual void _(Events.RowSelected<LUMPayrollAdjustment> e)
        {
            if (e.Row?.Approved ?? false)
            {
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.branchID>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.adjustmentDate>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.employeeID>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.adjustmentType>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.amount>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.remark>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetEnabled<LUMPayrollAdjustment.cutOffDate>(e.Cache, e.Row, false);
            }
        }

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
