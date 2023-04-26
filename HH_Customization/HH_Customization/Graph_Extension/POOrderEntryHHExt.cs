using PX.Data;
using PX.Objects.AP;
using PX.Objects.GL;
using PX.Objects.RQ;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PX.Objects.PO
{
    public class POOrderEntryHHExt : PXGraphExtension<POOrderEntry>
    {
        #region Message
        public const string WANT_TO_LINK_BRANCH = "Inconsistent to Line Branch and Document Branch. Click Yes if you want to update document branch to {0}";
        #endregion

        #region View
        public PXFilter<EditPOAccrualType> EditPOAccrualTypeForm;
        #endregion

        #region Action
        public PXAction<POOrder> updateBillingBase;

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Update Billing Base", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable UpdateBillingBase(PXAdapter adapter)
        {
            if (EditPOAccrualTypeForm.AskExt() == WebDialogResult.OK)
            {
                foreach (POLine line in Base.Transactions.Select())
                {
                    line.POAccrualType = EditPOAccrualTypeForm.Current.POAccrualType;
                    Base.Transactions.Update(line);
                }
            }
            EditPOAccrualTypeForm.Cache.Clear();
            return adapter.Get();
        }
        #endregion

        #region Event
        protected virtual void _(Events.RowSelected<POOrder> e, PXRowSelected baseMethod)
        {
            baseMethod?.Invoke(e.Cache, e.Args);
            if (e.Row == null) return;
            bool isLinkAP = GetLinkAPTran(e.Row.OrderNbr, e.Row.OrderType).Count > 0;
            bool isLinkReceipt = GetLinkPOReceiptLine(e.Row.OrderNbr, e.Row.OrderType).Count > 0;
            bool isNormal = POOrderType.IsNormalType(e.Row.OrderType);
            bool hasDetail = Base.Transactions.Select().Count > 0;
            updateBillingBase.SetEnabled(isNormal && !isLinkAP && !isLinkReceipt && hasDetail);
        }

        protected virtual void _(Events.RowPersisting<POOrder> e, PXRowPersisting baseMethod)
        {
            baseMethod?.Invoke(e.Cache, e.Args);
            if (e.Row == null) return;
            if (e.Operation == PXDBOperation.Delete) return;
            if (Base.Document.Cache.GetStatus(Base.Document.Current) == PXEntryStatus.Inserted)
            {
                LinkBranch();
            }
            //2023-02-03 每次存檔都檢核且提醒是否更新
            POOrder order = Base.Document.Current;
            POLine line = Base.Transactions.Select();
            if (line?.BranchID != null && order.BranchID != line.BranchID)
            {
                Branch branch = Branch.PK.Find(Base, line.BranchID);
                if (Base.Document.Ask(string.Format(WANT_TO_LINK_BRANCH, branch.BranchCD), MessageButtons.YesNo) == WebDialogResult.Yes)
                {
                    LinkBranch();
                }
            }
        }
        #endregion

        #region Method
        protected virtual void LinkBranch()
        {
            POOrder order = Base.Document.Current;
            POLine line = Base.Transactions.Select();
            if (line?.BranchID != null)
            {
                Base.Document.Cache.SetValueExt<POOrder.branchID>(order, line.BranchID);
                Base.Document.UpdateCurrent();
            }
        }
        #endregion

        #region BQL
        protected virtual List<APTran> GetLinkAPTran(string orderNbr, string orderType)
        {
            return PXSelect<APTran,
                Where<APTran.pONbr, Equal<Required<APTran.pONbr>>,
                And<APTran.pOOrderType, Equal<Required<APTran.pOOrderType>>>>>
                .Select(Base, orderNbr, orderType).RowCast<APTran>().ToList();
        }

        protected virtual List<POReceiptLine> GetLinkPOReceiptLine(string orderNbr, string orderType)
        {
            return PXSelect<POReceiptLine,
                Where<POReceiptLine.pONbr, Equal<Required<POReceiptLine.pONbr>>,
                And<POReceiptLine.pOType, Equal<Required<POReceiptLine.pOType>>>>>
                .Select(Base, orderNbr, orderType).RowCast<POReceiptLine>().ToList();
        }
        #endregion

        #region CacheAttached
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXUIField(DisplayName ="RI RefNbr",IsReadOnly = true)]
        [PXSelector(typeof(Search<RQRequisition.reqNbr>))]
        public virtual void _(Events.CacheAttached<POLine.rQReqNbr> e) { }
        #endregion

        #region Table
        [PXHidden]
        public class EditPOAccrualType : IBqlTable
        {
            #region POAccrualType
            [PXString(1, IsFixed = true)]
            [POAccrualType.List]
            [PXUIField(DisplayName = "Billing Based On")]
            [PXUnboundDefault(typeof(POAccrualType.order))]
            public virtual string POAccrualType { get; set; }
            public abstract class pOAccrualType : PX.Data.BQL.BqlString.Field<pOAccrualType> { }
            #endregion
        }
        #endregion
    }

}
