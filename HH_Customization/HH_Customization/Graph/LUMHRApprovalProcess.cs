using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HH_APICustomization.DAC;
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
                And<Brackets<HRApprovalFilter.employeeID.FromCurrent.IsNull
                    .Or<LUMHRApprovalV.employeeID.IsEqual<HRApprovalFilter.employeeID.FromCurrent>>>
                   >>>.
            ProcessingView.FilteredBy<HRApprovalFilter> ProcessView;
        #endregion

        #region Delegate
        public IEnumerable processView()
        {
            var filter = this.Filter.Current;
            PXView select = new PXView(this, true, this.ProcessView.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            var result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;
            foreach (LUMHRApprovalV item in result)
            {
                switch (filter.ProcessType)
                {
                    // 當ProcessType = Hold時，篩選 Approved = 1 AND BatchNbr is null AND (Hold =0  OR HOLD = Null)
                    case HRApprovalFilter.HOLD:
                        if ((item?.Approved ?? false) && string.IsNullOrEmpty(item?.BatchNbr) && !(item?.Hold ?? false))
                            yield return item;
                        break;
                    // 當ProcessType = Unhold 時，篩選 Approved = 1 AND BatchNbr is null AND Hold = 1 
                    case HRApprovalFilter.UNBOUND:
                        if ((item?.Approved ?? false) && string.IsNullOrEmpty(item?.BatchNbr) && (item?.Hold ?? false))
                            yield return item;
                        break;
                    // 當ProcessType = Generate JE 時，篩選 Approved = 1 AND BatchNbr is null AND (Hold =0  OR HOLD = Null)
                    case HRApprovalFilter.GENERATEJE:
                        if ((item?.Approved ?? false) && string.IsNullOrEmpty(item?.BatchNbr) && !(item?.Hold ?? false))
                            yield return item;
                        break;
                    // 當ProcessType = Release的時候，篩選 Approved = 0 OR Approved = Null  				
                    case HRApprovalFilter.RELEASE:
                        if (!(item?.Approved ?? false))
                            yield return item;
                        break;
                    // 當ProcessType = Reverse的時候，篩選 Approved = 1  AND (Hold = 0 OR HOLD = NULL) AND (BatchNbr is null)					
                    case HRApprovalFilter.REVERSE:
                        if ((item?.Approved ?? false) && !(item?.Hold ?? false) && string.IsNullOrEmpty(item?.BatchNbr))
                            yield return item;
                        break;
                }
            }
        }
        #endregion

        #region Event
        public virtual void _(Events.RowSelected<HRApprovalFilter> e)
        {
            var ptypeAttr = SelectFrom<PX.Objects.CS.CSAttributeDetail>
                           .Where<PX.Objects.CS.CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                           .View.Select(this, "PTYPE").RowCast<PX.Objects.CS.CSAttributeDetail>();
            PXStringListAttribute.SetList<HRApprovalFilter.pType>(this.Caches[typeof(HRApprovalFilter)], null, ptypeAttr.Select(x => x.ValueID).ToArray(), ptypeAttr.Select(x => x.Description).ToArray());
        }

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
                // Validation and Process (GENERATE JE)
                if (filter.ProcessType == HRApprovalFilter.GENERATEJE)
                {
                    ValidAccountMapping(list, self);
                    if (string.IsNullOrEmpty(filter.PType))
                        throw new Exception("PType can not be empty!");

                    var accountMapping = SelectFrom<LUMHRPayrollAccountMapping>.View.Select(self).RowCast<LUMHRPayrollAccountMapping>();
                    var processData = from t in list
                                      join m in accountMapping on new { A = t.BranchID, B = t.Type.ToUpper() } equals new { A = m.Branch, B = m.PayrollType.ToUpper() }
                                      select new
                                      {
                                          Branch = t.BranchID,
                                          PayrollType = m.PayrollType,
                                          CreditAccount = m.CreditAcount,
                                          CreditSubAccount = m.CreditSub,
                                          DebitAccount = m.DebitAccount,
                                          DebitSub = m.DebitSub,
                                          Amount = t.Amount
                                      };
                    foreach (var GroupData in processData.GroupBy(x => new { x.Branch, x.CreditAccount, x.CreditSubAccount, x.DebitAccount, x.DebitSub }))
                    {
                        #region Header
                        var glGraph = PXGraph.CreateInstance<JournalEntry>();
                        var doc = glGraph.BatchModule.Cache.CreateInstance() as Batch;
                        doc.Module = "GL";
                        doc = glGraph.BatchModule.Cache.Insert(doc) as Batch;
                        doc.BranchID = GroupData?.Key?.Branch;
                        doc.Description = $"{filter.PType} - {filter.CutoffDate}";
                        glGraph.BatchModule.Cache.Update(doc);
                        #endregion

                        #region Details
                        // Set CurrentItem
                        #region Credit
                        var line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
                        line.BranchID = GroupData.Key.Branch;
                        line.AccountID = GroupData.Key.CreditAccount;
                        line.SubID = GroupData.Key.CreditSubAccount;
                        line.CuryCreditAmt = GroupData.Sum(x => x.Amount ?? 0);
                        line.TranDesc = $"{filter.PType} - {filter.CutoffDate}";
                        line = glGraph.GLTranModuleBatNbr.Cache.Insert(line) as GLTran;
                        #endregion

                        #region Debit
                        line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
                        line.BranchID = GroupData.Key.Branch;
                        line.AccountID = GroupData.Key.DebitAccount;
                        line.SubID = GroupData.Key.DebitSub;
                        line.CuryDebitAmt = GroupData.Sum(x => x.Amount ?? 0);
                        line.TranDesc = $"{filter.PType} - {filter.CutoffDate}";
                        line = glGraph.GLTranModuleBatNbr.Cache.Insert(line) as GLTran;
                        #endregion

                        #endregion

                        glGraph.Save.Press();
                        // 回寫資料
                        foreach (var grpItem in GroupData)
                        {
                            foreach (var data in list.Where(x => x.BranchID == GroupData.Key.Branch && x.Type.ToUpper() == grpItem.PayrollType.ToUpper()))
                            {
                                PXProcessing.SetCurrentItem(data);
                                dynamic currentItem = new System.Dynamic.ExpandoObject();
                                if (data.DataType == DATA_TYPE_PH)
                                    currentItem = self.GetLUMPayrollHour(data.BranchID, data.Date, data.EmployeeID, data.Type);
                                else
                                    currentItem = self.GetLUMPayrollAdjustment(data.BranchID, data.Date, data.EmployeeID, data.Type);
                                currentItem.BatchNbr = doc.BatchNbr;
                                PersistUpdatedData(self, currentItem);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var data in list)
                    {
                        dynamic currentItem = new System.Dynamic.ExpandoObject();
                        if (data.DataType == DATA_TYPE_PH)
                            currentItem = self.GetLUMPayrollHour(data.BranchID, data.Date, data.EmployeeID, data.Type);
                        else
                            currentItem = self.GetLUMPayrollAdjustment(data.BranchID, data.Date, data.EmployeeID, data.Type);
                        switch (filter.ProcessType)
                        {
                            case HRApprovalFilter.RELEASE:
                            case HRApprovalFilter.REVERSE:
                                currentItem.Approved = isRelease;
                                currentItem.ApprovedAmount = isRelease ? data.Amount : currentItem.ApprovedAmount;
                                break;
                            case HRApprovalFilter.HOLD:
                                currentItem.Hold = true;
                                break;
                            case HRApprovalFilter.UNBOUND:
                                currentItem.Hold = false;
                                break;
                        }
                        PersistUpdatedData(self, currentItem);
                    }
                }
                ts.Complete();
            }
        }

        public static void PersistUpdatedData<TG>(LUMHRApprovalProcess self, TG currentItem) where TG : class, IBqlTable, new()
        {
            self.Caches<TG>().Update(currentItem);
            self.Caches<TG>().PersistUpdated(currentItem);
        }

        /// <summary> 檢查Payroll Type是否維護對應Cash Account </summary>
        public static void ValidAccountMapping(List<LUMHRApprovalV> list, LUMHRApprovalProcess self)
        {
            var selectedTypes = list.Select(x => x.Type).Distinct();
            var accountMapping = SelectFrom<LUMHRPayrollAccountMapping>.View.Select(self).RowCast<LUMHRPayrollAccountMapping>();
            var ExceptList = selectedTypes.Except(accountMapping.Select(x => x.PayrollType));
            if (ExceptList.Count() > 0)
                throw new Exception($"Can not find account corresponding to {string.Join(",", ExceptList)}");
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
            public const string HOLD = "2";
            public const string UNBOUND = "3";
            public const string GENERATEJE = "4";
            public const string RELEASE_L = "Release";
            public const string REVERSE_L = "Reverse";
            public const string HOLD_L = "HOLD";
            public const string UNBOUND_L = "UNBOUND";
            public const string GENERATEJE_L = "GENERATE JE";
            #endregion

            #region ProcessType
            [PXString()]
            [PXUIField(DisplayName = "Process Type", Required = true)]
            [PXUnboundDefault(RELEASE)]
            [PXStringList(
                new string[] { RELEASE, REVERSE, HOLD, UNBOUND, GENERATEJE },
                new string[] { RELEASE_L, REVERSE_L, HOLD_L, UNBOUND_L, GENERATEJE_L }
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

            #region PTYPE
            [PXStringList]
            [PXString]
            [PXUIField(DisplayName = "PTYPE")]
            public virtual string PType { get; set; }
            public abstract class pType : PX.Data.BQL.BqlString.Field<pType> { }
            #endregion
        }
        #endregion

    }
}