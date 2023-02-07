using PX.Data;
using PX.Objects.GL;

namespace PX.Objects.PO
{
    public class POOrderEntryHHExt : PXGraphExtension<POOrderEntry>
    {
        #region Message
        public const string WANT_TO_LINK_BRANCH = " Inconsistent to Line Branch and Document Branch. Click Yes if you want to update document branch to {0}";
        #endregion

        #region Event
        protected virtual void _(Events.RowPersisting<POOrder> e, PXRowPersisting baseMethod)
        {
            baseMethod?.Invoke(e.Cache, e.Args);
            if (e.Row == null) return;
            if (Base.Document.Cache.GetStatus(Base.Document.Current) == PXEntryStatus.Inserted)
            {
                LinkBranch();
            }
            //2023-02-03 每次存檔都檢核且提醒是否更新
            POOrder order = Base.Document.Current;
            POLine line = Base.Transactions.Select();
            if (line.BranchID != null && order.BranchID != line.BranchID)
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
    }
    #endregion
}
