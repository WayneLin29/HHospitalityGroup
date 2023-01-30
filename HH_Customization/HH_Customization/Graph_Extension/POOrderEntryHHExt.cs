using PX.Data;

namespace PX.Objects.PO
{
    public class POOrderEntryHHExt:PXGraphExtension<POOrderEntry>
    {
        #region Event
        protected virtual void _(Events.RowPersisting<POOrder> e, PXRowPersisting baseMethod) {
            baseMethod?.Invoke(e.Cache,e.Args);
            if (e.Row == null) return;
            LinkBranch();
        }
        #endregion

        #region Method
        protected virtual void LinkBranch()
        {
            POOrder order = Base.Document.Current;
            if (Base.Document.Cache.GetStatus(order) == PXEntryStatus.Inserted)
            {
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
}
