using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.GL;
using PX.Data.BQL.Fluent;
using HH_APICustomization.DAC;
using System.Collections;
using PX.Objects.CM;
using PX.Objects.CS;
using PX.SM;

namespace HH_APICustomization.Graph
{
    public class LUMReconcilidProcess : PXGraph<LUMReconcilidProcess>
    {
        public PXFilter<ReconcilidFilter> Filter;

        [PXVirtualDAC]
        public SelectFrom<GLTranDebit>.
               LeftJoin<Users>.On<GLTranDebit.usrReconciledBy.IsEqual<Users.pKID>>.
               Where<GLTranDebit.accountID.IsEqual<ReconcilidFilter.accountID.FromCurrent>.
                 And<GLTranDebit.subID.IsEqual<ReconcilidFilter.subID.FromCurrent>>.
                 And<GLTranDebit.usrReconciled.IfNullThen<False>.IsEqual<ReconcilidFilter.showReconciledTrans.FromCurrent>>.
                 And<GLTranDebit.tranDate.IsBetween<ReconcilidFilter.dateFrom.FromCurrent, ReconcilidFilter.dateTo.FromCurrent>>>.View DebitTransactions;

        public SelectFrom<GLTran>.
               LeftJoin<LUMCloudBedReservations>.On<GLTran.refNbr.IsEqual<LUMCloudBedReservations.reservationID>>.
               LeftJoin<Users>.On<GLTranExtension.usrReconciledBy.IsEqual<Users.pKID>>.
               Where<GLTran.curyCreditAmt.IsNotEqual<Zero>.
                 And<GLTran.accountID.IsEqual<ReconcilidFilter.accountID.FromCurrent>>.
                 And<GLTran.subID.IsEqual<ReconcilidFilter.subID.FromCurrent>>.
                 And<GLTranExtension.usrReconciled.IfNullThen<False>.IsEqual<ReconcilidFilter.showReconciledTrans.FromCurrent>>.
                 And<GLTran.tranDate.IsBetween<ReconcilidFilter.dateFrom.FromCurrent, ReconcilidFilter.dateTo.FromCurrent>>>.View CreditTransactions;

        #region Action
        public PXAction<ReconcilidFilter> selectAll;
        [PXUIField(DisplayName = "SELECT ALL", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public virtual IEnumerable SelectAll(PXAdapter adapter)
        {

            foreach (var item in this.DebitTransactions.View.SelectMulti().RowCast<GLTranDebit>())
            {
                this.DebitTransactions.SetValueExt<GLTranDebit.selected>(item, true);
                this.DebitTransactions.Update(item);
            }

            foreach (var item in this.CreditTransactions.View.SelectMulti().RowCast<GLTran>())
            {
                this.CreditTransactions.SetValueExt<GLTran.selected>(item, true);
                this.CreditTransactions.Update(item);
            }
            return adapter.Get();
        }

        public PXAction<ReconcilidFilter> autoMatch;
        [PXUIField(DisplayName = "AUTO-MATCH", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public virtual IEnumerable AutoMatch(PXAdapter adapter)
        {
            var creditData = this.CreditTransactions.Select().ToList();
            var debitData = this.DebitTransactions.Select().ToList();

            // unselect all
            creditData.ForEach(x => { x.GetItem<GLTran>().Selected = false; });
            debitData.ForEach(x => { x.GetItem<GLTranDebit>().Selected = false; });

            var intersection = from c in creditData
                               join d in debitData on new { A = c.GetItem<GLTran>().RefNbr, B = c.GetItem<GLTran>().CreditAmt, C = c.GetItem<LUMCloudBedReservations>()?.ThirdPartyIdentifier } equals new { A = d.GetItem<GLTranDebit>().RefNbr, B = d.GetItem<GLTranDebit>().DebitAmt, C = d.GetItem<GLTranDebit>()?.ThirdPartyIdentifier }
                               select new { credit = c, debit = d };
            foreach (var item in intersection)
            {
                this.Caches[typeof(GLTran)].SetValueExt<GLTran.selected>(item.credit, true);
                this.Caches[typeof(GLTranDebit)].SetValueExt<GLTran.selected>(item.debit, true);
                this.CreditTransactions.Update(item.credit);
                this.DebitTransactions.Update(item.debit);
            }
            return adapter.Get();
        }

        public PXAction<ReconcilidFilter> release;
        [PXUIField(DisplayName = "RELEASE", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXProcessButton]
        public virtual IEnumerable Release(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                ReleaseProcessing(this);
            });
            return adapter.Get();
        }

        public PXAction<ReconcilidFilter> reverse;
        [PXUIField(DisplayName = "Reverse", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXProcessButton]
        public virtual IEnumerable Reverse(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                ReverseProcessing(this);
            });
            return adapter.Get();
        }
        #endregion

        #region Events

        [PXUIField(DisplayName = "Reconciled By")]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        public virtual void _(Events.CacheAttached<Users.username> e) { }

        public virtual void _(Events.RowSelected<ReconcilidFilter> e)
            => CheckButtnAvailable();

        public virtual void _(Events.FieldUpdated<GLTranDebit.selected> e)
           => RecalculateBalance();

        public virtual void _(Events.FieldUpdated<GLTran.selected> e)
            => RecalculateBalance();

        public virtual void _(Events.FieldUpdated<ReconcilidFilter.showReconciledTrans> e)
            => CheckButtnAvailable();

        #endregion

        #region Method

        /// <summary> 設置Button是否可用 </summary>
        public void CheckButtnAvailable()
        {
            this.Actions["SelectAll"].SetEnabled(!(this.Filter.Current?.ShowReconciledTrans ?? false));
            this.Actions["AutoMatch"].SetEnabled(!(this.Filter.Current?.ShowReconciledTrans ?? false));
            // Diference amount == 0
            this.Actions["Release"].SetEnabled(!(this.Filter.Current?.ShowReconciledTrans ?? false) && this.Filter.Current?.DifferenceAmt == 0);
            this.Actions["Reverse"].SetEnabled((this.Filter.Current?.ShowReconciledTrans ?? false));
        }

        /// <summary> 計算畫面上的Balance amount </summary>
        public void RecalculateBalance()
        {
            var filter = this.Filter.Current;
            filter.PostingSelectedBalance = this.DebitTransactions.Cache.Cached.RowCast<GLTranDebit>().ToList().Where(x => x.Selected ?? false).Sum(x => x.DebitAmt);
            filter.ReconciledSelectedBalance = this.CreditTransactions.Cache.Cached.RowCast<GLTran>().ToList().Where(x => x.Selected ?? false).Sum(x => x.CreditAmt);
            filter.DifferenceAmt = filter.PostingSelectedBalance - filter.ReconciledSelectedBalance;
            this.Filter.UpdateCurrent();
            CheckButtnAvailable();
        }

        /// <summary> 更新GLTran </summary>
        public void UpdateGLTran(string module, string batchNbr, int? lineNbr, GLTranExtension extension)
        {
            PXUpdate<Set<GLTranExtension.usrReconciled, Required<GLTranExtension.usrReconciled>,
                     Set<GLTranExtension.usrReconciledDate, Required<GLTranExtension.usrReconciledDate>,
                     Set<GLTranExtension.usrReconciledBatch, Required<GLTranExtension.usrReconciledBatch>,
                     Set<GLTranExtension.usrReconciledBy, Required<GLTranExtension.usrReconciledBy>>>>>,
                GLTran,
                Where<GLTran.module, Equal<Required<GLTran.module>>,
                  And<GLTran.batchNbr, Equal<Required<GLTran.batchNbr>>,
                  And<GLTran.lineNbr, Equal<Required<GLTran.lineNbr>>>>>>.
                Update(
                    this,
                    extension.UsrReconciled,
                    extension.UsrReconciledDate,
                    extension.UsrReconciledBatch,
                    extension.UsrReconciledBy,
                    module,
                    batchNbr,
                    lineNbr);
        }

        /// <summary> 執行Release </summary>
        public static void ReleaseProcessing(LUMReconcilidProcess baseGraph)
        {
            string newSequence = string.Empty;
            var selectedDebit = baseGraph.DebitTransactions.View.SelectMulti().RowCast<GLTranDebit>().Where(x => x.Selected ?? false);
            var selectedCredit = baseGraph.CreditTransactions.View.SelectMulti().RowCast<GLTran>().Where(x => x.Selected ?? false);
            if (selectedDebit.Count() > 0 || selectedCredit.Count() > 0)
                newSequence = AutoNumberAttribute.GetNextNumber(baseGraph.DebitTransactions.Cache, null, "GLRECON", baseGraph.Accessinfo.BusinessDate);
            var extension = new GLTranExtension()
            {
                UsrReconciledDate = baseGraph.Accessinfo.BusinessDate,
                UsrReconciled = true,
                UsrReconciledBy = baseGraph.Accessinfo.UserID,
                UsrReconciledBatch = newSequence
            };
            // Update Debit 
            foreach (var item in selectedDebit)
                baseGraph.UpdateGLTran(item.Module, item.BatchNbr, item.LineNbr, extension);

            // Update Credit
            foreach (var item in selectedCredit)
                baseGraph.UpdateGLTran(item.Module, item.BatchNbr, item.LineNbr, extension);
        }

        /// <summary> 執行Reverse </summary>
        public static void ReverseProcessing(LUMReconcilidProcess baseGraph)
        {
            var selectedDebit = baseGraph.DebitTransactions.View.SelectMulti().RowCast<GLTranDebit>().Where(x => x.Selected ?? false);
            var selectedCredit = baseGraph.CreditTransactions.View.SelectMulti().RowCast<GLTran>().Where(x => x.Selected ?? false);
            var extension = new GLTranExtension()
            {
                UsrReconciledDate = null,
                UsrReconciled = false,
                UsrReconciledBy = baseGraph.Accessinfo.UserID,
                UsrReconciledBatch = null
            };
            // Update Debit 
            foreach (var item in selectedDebit)
                baseGraph.UpdateGLTran(item.Module, item.BatchNbr, item.LineNbr, extension);

            // Update Credit
            foreach (var item in selectedCredit)
                baseGraph.UpdateGLTran(item.Module, item.BatchNbr, item.LineNbr, extension);
        }

        #endregion

    }

    [Serializable]
    public class ReconcilidFilter : IBqlTable
    {
        [PXInt]
        [PXUIField(DisplayName = "Branch", Required = true)]
        [Branch]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }

        [Account(typeof(GLTran.branchID), LedgerID = typeof(GLTran.ledgerID), DescriptionField = typeof(Account.description))]
        [PXDefault]
        [PXUIField(Required = true)]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }

        [SubAccount(typeof(ReconcilidFilter.accountID), typeof(ReconcilidFilter.branchID), true)]
        [PXDefault]
        [PXUIField(Required = true)]
        public virtual int? SubID { get; set; }
        public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }

        [PXDate]
        [PXUIField(DisplayName = "DateFrom")]
        public virtual DateTime? DateFrom { get; set; }
        public abstract class dateFrom : PX.Data.BQL.BqlDateTime.Field<dateFrom> { }

        [PXDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "DateTo")]
        public virtual DateTime? DateTo { get; set; }
        public abstract class dateTo : PX.Data.BQL.BqlDateTime.Field<dateTo> { }

        [PXBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Show Reconciled Transactions")]
        public virtual bool? ShowReconciledTrans { get; set; }
        public abstract class showReconciledTrans : PX.Data.BQL.BqlBool.Field<showReconciledTrans> { }

        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Posting Selected Balance", Enabled = false)]
        public virtual decimal? PostingSelectedBalance { get; set; }
        public abstract class postingSelectedBalance : PX.Data.BQL.BqlDecimal.Field<postingSelectedBalance> { }

        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Reconciled Selected Balance", Enabled = false)]
        public virtual decimal? ReconciledSelectedBalance { get; set; }
        public abstract class reconciledSelectedBalance : PX.Data.BQL.BqlDecimal.Field<reconciledSelectedBalance> { }

        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Difference", Enabled = false)]
        public virtual decimal? DifferenceAmt { get; set; }
        public abstract class differenceAmt : PX.Data.BQL.BqlDecimal.Field<differenceAmt> { }
    }


    [PXProjection(typeof(SelectFrom<GLTran>.
                         LeftJoin<LUMCloudBedReservations>.On<GLTran.refNbr.IsEqual<LUMCloudBedReservations.reservationID>>.
                         Where<GLTran.curyDebitAmt.IsNotEqual<Zero>>))]
    public class GLTranDebit : IBqlTable
    {
        #region Selected
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }

        /// <summary>
        /// Used for selection on screens.
        /// </summary>
        [PXBool]
        [PXUIField(DisplayName = "Selected", Visible = false)]
        public virtual bool? Selected { get; set; }
        #endregion

        #region BatchNbr
        [PXDBString(15, IsUnicode = true, BqlField = typeof(GLTran.batchNbr), IsKey = true)]
        [PXUIField(DisplayName = "Batch Number", Visibility = PXUIVisibility.Visible, Visible = false)]
        public virtual String BatchNbr { get; set; }
        public abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }

        #endregion

        #region Module
        [PXDBString(2, IsKey = true, BqlField = typeof(GLTran.module))]
        public virtual String Module { get; set; }
        public abstract class module : PX.Data.BQL.BqlString.Field<module> { }

        #endregion

        #region AccountID

        [PXDBInt(BqlField = typeof(GLTran.accountID))]
        [PXDefault]
        public virtual Int32? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }

        #endregion

        #region SubID
        [PXDBInt(BqlField = typeof(GLTran.subID))]
        [PXDefault]
        public virtual Int32? SubID { get; set; }
        public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }

        #endregion

        #region LineNbr
        [PXDBInt(BqlField = typeof(GLTran.lineNbr), IsKey = true)]
        [PXDefault()]
        [PXUIField(DisplayName = "Line Nbr.", Visibility = PXUIVisibility.Visible, Visible = false, Enabled = false)]
        public virtual Int32? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }

        #endregion

        #region TranDesc
        [PXDBString(IsUnicode = true, BqlField = typeof(GLTran.tranDesc))]
        [PXUIField(DisplayName = "Transaction Description", Visibility = PXUIVisibility.Visible)]
        public virtual String TranDesc { get; set; }
        public abstract class tranDesc : PX.Data.BQL.BqlString.Field<tranDesc> { }

        #endregion

        #region TranDate
        [PXDBDate(BqlField = typeof(GLTran.tranDesc))]
        [PXUIField(DisplayName = "Transaction Date", Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual DateTime? TranDate { get; set; }
        public abstract class tranDate : PX.Data.BQL.BqlDateTime.Field<tranDate> { }

        #endregion

        #region DebitAmt
        [PXDBBaseCury(typeof(GLTran.ledgerID), BqlField = typeof(GLTran.debitAmt))]
        public virtual Decimal? DebitAmt { get; set; }
        public abstract class debitAmt : PX.Data.BQL.BqlDecimal.Field<debitAmt> { }

        #endregion

        #region RefNbr

        [PXDBString(15, IsUnicode = true, BqlField = typeof(GLTran.refNbr))]
        [PXUIField(DisplayName = "Ref. Number", Visibility = PXUIVisibility.Visible)]
        public virtual String RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }

        #endregion

        #region ReferenceID
        [PXDBInt(BqlField = typeof(GLTran.referenceID))]
        [PXUIField(DisplayName = "Customer/Vendor", Enabled = false, Visible = false)]
        public virtual Int32? ReferenceID { get; set; }
        public abstract class referenceID : PX.Data.BQL.BqlInt.Field<referenceID> { }

        #endregion

        #region Source
        [PXDBString(256, IsUnicode = true, InputMask = "", BqlField = typeof(LUMCloudBedReservations.source))]
        [PXUIField(DisplayName = "Source")]
        public virtual string Source { get; set; }
        public abstract class source : PX.Data.BQL.BqlString.Field<source> { }
        #endregion

        #region ThirdPartyIdentifier
        [PXDBString(256, IsUnicode = true, InputMask = "", BqlField = typeof(LUMCloudBedReservations.thirdPartyIdentifier))]
        [PXUIField(DisplayName = "Third Party Identifier")]
        public virtual string ThirdPartyIdentifier { get; set; }
        public abstract class thirdPartyIdentifier : PX.Data.BQL.BqlString.Field<thirdPartyIdentifier> { }
        #endregion

        #region StartDate
        [PXDBDate(UseTimeZone = false, BqlField = typeof(LUMCloudBedReservations.startDate))]
        [PXUIField(DisplayName = "Start Date")]
        public virtual DateTime? StartDate { get; set; }
        public abstract class startDate : PX.Data.BQL.BqlDateTime.Field<startDate> { }
        #endregion

        #region EndDate
        [PXDBDate(UseTimeZone = false, BqlField = typeof(LUMCloudBedReservations.endDate))]
        [PXUIField(DisplayName = "End Date")]
        public virtual DateTime? EndDate { get; set; }
        public abstract class endDate : PX.Data.BQL.BqlDateTime.Field<endDate> { }
        #endregion

        #region UsrReconciled
        [PXDBBool(BqlField = typeof(GLTranExtension.usrReconciled))]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Reconciled", Enabled = false)]
        public virtual bool? UsrReconciled { get; set; }
        public abstract class usrReconciled : PX.Data.BQL.BqlBool.Field<usrReconciled> { }
        #endregion

        #region UsrReconciledDate
        [PXDBDate(UseTimeZone = false, BqlField = typeof(GLTranExtension.usrReconciledDate))]
        [PXUIField(DisplayName = "Reconciled Date", Enabled = false)]
        public virtual DateTime? UsrReconciledDate { get; set; }
        public abstract class usrReconciledDate : PX.Data.BQL.BqlDateTime.Field<usrReconciledDate> { }
        #endregion

        #region UsrReconciledBatch
        [PXDBString(15, BqlField = typeof(GLTranExtension.usrReconciledBatch))]
        [PXUIField(DisplayName = "Reconciled Batch", Enabled = false)]
        public virtual string UsrReconciledBatch { get; set; }
        public abstract class usrReconciledBatch : PX.Data.BQL.BqlString.Field<usrReconciledBatch> { }
        #endregion

        #region UsrReconciledBy
        [PXDBGuid(BqlField = typeof(GLTranExtension.usrReconciledBy))]
        [PXUIField(DisplayName = "Reconciled By", Enabled = false)]
        public virtual Guid? UsrReconciledBy { get; set; }
        public abstract class usrReconciledBy : PX.Data.BQL.BqlGuid.Field<usrReconciledBy> { }
        #endregion
    }
}
