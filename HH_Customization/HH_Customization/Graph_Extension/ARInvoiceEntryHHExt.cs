using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;
using PX.Objects.GL;

namespace PX.Objects.AR
{
    public class ARInvoiceEntryHHExt : PXGraphExtension<ARInvoiceEntry>
    {
        #region Override
        #region Release
        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            if (ValidateBranch() == WebDialogResult.OK)
                return baseMethod(adapter);
            else
                return adapter.Get();
        }
        #endregion
        #endregion

        #region Event
        protected virtual void _(Events.RowPersisting<ARInvoice> e)
        {
            LinkBranch();
        }
        #endregion

        #region Method
        /// <summary>
        /// ARInvoice新增時自動綁定明細第一筆BranchID
        /// + ARTaxTran 也要更新BranchID
        /// </summary>
        /// <returns></returns>
        protected virtual void LinkBranch()
        {
            ARInvoice invoice = Base.Document.Current;
            if (Base.Document.Cache.GetStatus(invoice) == PXEntryStatus.Inserted)
            {
                ARTran tran = Base.Transactions.Select();
                if (tran?.BranchID != null)
                {
                    Base.Document.Cache.SetValueExt<ARInvoice.branchID>(invoice, tran.BranchID);
                    //Base.Document.UpdateCurrent();
                }
            }
        }

        /// <summary>
        /// 檢核明細BranchID 是否與表頭一致，不一致則跳確認
        /// </summary>
        /// <returns></returns>
        protected virtual WebDialogResult ValidateBranch()
        {
            ARInvoice invoice = Base.Document.Current;
            foreach (ARTran tran in Base.Transactions.Select())
            {
                if (invoice.BranchID != tran.BranchID)
                {
                    Branch hb = Branch.PK.Find(Base, invoice.BranchID);
                    Branch db = Branch.PK.Find(Base, tran.BranchID);
                    return Base.Document.Ask(
                        $"Document Branch - [{hb?.BranchCD?.Trim()}] differs from[{db?.BranchCD?.Trim()}].This will result in inter - company posting.Are you sure you want to proceed ?"
                        , MessageButtons.OKCancel);
                }
            }
            return WebDialogResult.OK;
        }
        #endregion
    }
}
