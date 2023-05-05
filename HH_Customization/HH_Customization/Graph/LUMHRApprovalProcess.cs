using System;
using System.Collections.Generic;
using HH_Customization.DAC;
using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.EP;
using PX.Objects.GL;

namespace HH_Customization.Graph
{
    public class LUMHRApprovalProcess : PXGraph<LUMHRApprovalProcess>
    {
        public LUMHRApprovalProcess()
        {
            var self = this;
            ProcessView.SetProcessAllEnabled(true);
            ProcessView.SetProcessEnabled(true);
            ProcessView.SetProcessDelegate(list => DoProcess(list, self));
        }

        #region Const
        /// <summary>
        /// LUMPayrollHour
        /// </summary>
        public const string DATA_TYPE_PH = "PH";
        /// <summary>
        /// LUMPayrollAdjustment
        /// </summary>
        public const string DATA_TYPE_PA = "PA";
        #endregion

        #region View
        public PXFilter<HRApprovalFilter> Filter;
        public SelectFrom<LUMHRApprovalV>.
            Where<LUMHRApprovalV.cutoffDate.IsEqual<HRApprovalFilter.cutoffDate.FromCurrent>.
                And<LUMHRApprovalV.branchID.IsEqual<HRApprovalFilter.branchID.FromCurrent>.
                 And<LUMHRApprovalV.approved.IsEqual<HRApprovalFilter.approved.FromCurrent>.
                And<Brackets<HRApprovalFilter.employeeID.FromCurrent.IsNull
                    .Or<LUMHRApprovalV.employeeID.IsEqual<HRApprovalFilter.employeeID.FromCurrent>>>
                   >>>>.
            ProcessingView.FilteredBy<HRApprovalFilter> ProcessView;
        #endregion

        #region 
        public virtual void _(Events.FieldUpdated<HRApprovalFilter, HRApprovalFilter.processType> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<HRApprovalFilter.approved>(e.Row);
        }

        public virtual void _(Events.FieldDefaulting<HRApprovalFilter, HRApprovalFilter.approved> e)
        {
            if (e.Row == null) return;
            if (e.Row.ProcessType == HRApprovalFilter.RELEASE)
            {
                e.NewValue = false;
            }
            else if (e.Row.ProcessType == HRApprovalFilter.REVERSE)
            {
                e.NewValue = true;
            }
        }

        #endregion

        #region Method
        public static void DoProcess(List<LUMHRApprovalV> list, LUMHRApprovalProcess self)
        {
            var filter = self.Filter.Current;
            bool isRelease = filter.ProcessType == HRApprovalFilter.RELEASE;
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                foreach (var data in list)
                {
                    if (data.DataType == DATA_TYPE_PH)
                    {
                        var item = self.GetLUMPayrollHour(data.BranchID, data.Date, data.EmployeeID, data.Type);
                        item.Approved = isRelease;
                        item.ApprovedAmount = isRelease ? data.Amount : item.ApprovedAmount;
                        self.Caches<LUMPayrollHour>().Update(item);
                        self.Caches<LUMPayrollHour>().PersistUpdated(item);
                    }
                    else if (data.DataType == DATA_TYPE_PA)
                    {
                        var item = self.GetLUMPayrollAdjustment(data.BranchID, data.Date, data.EmployeeID, data.Type);
                        item.Approved = isRelease;
                        item.ApprovedAmount = isRelease ? data.Amount : item.ApprovedAmount;
                        self.Caches<LUMPayrollAdjustment>().Update(item);
                        self.Caches<LUMPayrollAdjustment>().PersistUpdated(item);
                    }
                }
                ts.Complete();
            }
        }
        #endregion

        #region BQL
        public LUMPayrollHour GetLUMPayrollHour(int? branchID, DateTime? date, int? employeeID, string type)
        {
            return SelectFrom<LUMPayrollHour>.
                Where<LUMPayrollHour.branchID.IsEqual<@P.AsInt>.
                And<LUMPayrollHour.workingDate.IsEqual<@P.AsDateTime>.
                And<LUMPayrollHour.employeeID.IsEqual<@P.AsInt>.
                And<LUMPayrollHour.earningType.IsEqual<@P.AsString>>>>>
                .View.Select(this, branchID, date, employeeID, type);
        }

        public LUMPayrollAdjustment GetLUMPayrollAdjustment(int? branchID, DateTime? date, int? employeeID, string type)
        {
            return SelectFrom<LUMPayrollAdjustment>.
                Where<LUMPayrollAdjustment.branchID.IsEqual<@P.AsInt>.
                And<LUMPayrollAdjustment.adjustmentDate.IsEqual<@P.AsDateTime>.
                And<LUMPayrollAdjustment.employeeID.IsEqual<@P.AsInt>.
                And<LUMPayrollAdjustment.adjustmentType.IsEqual<@P.AsString>>>>>
                .View.Select(this, branchID, date, employeeID, type);
        }
        #endregion

        #region Table
        [Serializable]
        [PXHidden]
        public class HRApprovalFilter : IBqlTable
        {
            #region const
            public const string RELEASE = "1";
            public const string REVERSE = "0";
            public const string RELEASE_L = "Release";
            public const string REVERSE_L = "Reverse";
            #endregion

            #region ProcessType
            [PXString()]
            [PXUIField(DisplayName = "Process Type", Required = true)]
            [PXUnboundDefault(RELEASE)]
            [PXStringList(
                new string[] { RELEASE, REVERSE },
                new string[] { RELEASE_L, REVERSE_L }
                )]
            public virtual string ProcessType { get; set; }
            public abstract class processType : PX.Data.BQL.BqlString.Field<processType> { }
            #endregion

            #region CutoffDate
            [PXDate()]
            [PXUIField(DisplayName = "Cutoff Date", Required = true)]
            public virtual DateTime? CutoffDate { get; set; }
            public abstract class cutoffDate : PX.Data.BQL.BqlDateTime.Field<cutoffDate> { }
            #endregion

            #region BranchID
            [PXInt()]
            [PXUIField(DisplayName = "Branch", Required = true)]
            [PXSelector(typeof(Search<Branch.branchID>),
                typeof(Branch.branchCD),
                typeof(Branch.acctName),
                SubstituteKey = typeof(Branch.branchCD),
                DescriptionField = typeof(Branch.acctName)
                )]
            public virtual int? BranchID { get; set; }
            public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
            #endregion

            #region EmployeeID
            [PXInt()]
            [PXUIField(DisplayName = "Employee")]
            [PXSelector(typeof(Search<EPEmployee.bAccountID>),
                typeof(EPEmployee.acctCD),
                typeof(EPEmployee.acctName),
                SubstituteKey = typeof(EPEmployee.acctCD),
                DescriptionField = typeof(EPEmployee.acctName)
                )]
            public virtual int? EmployeeID { get; set; }
            public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
            #endregion

            #region Approved
            [PXBool]
            [PXUIField(DisplayName = "Approved")]
            [PXUnboundDefault(false)]
            public virtual bool? Approved { get; set; }
            public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
            #endregion
        }
        #endregion

    }
}