using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;
using PX.Objects.CA;
using PX.Objects.GL;

namespace PX.Objects.AR
{
    public class ARPaymentEntryHHExt : PXGraphExtension<ARPaymentEntry>
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
        protected virtual void _(Events.RowPersisting<ARPayment> e)
        {
            LinkBranch();
        }
        #endregion

        #region Method
        /// <summary>
        /// ARPayment新增時自動綁定CashAccount.BranchID
        /// </summary>
        /// <returns></returns>
        protected virtual void LinkBranch()
        {
            var doc = Base.Document.Current;
            if (Base.Document.Cache.GetStatus(doc) == PXEntryStatus.Inserted)
            {
                var ca = CashAccount.PK.Find(Base, doc.CashAccountID);
                Base.Document.Cache.SetValueExt<ARInvoice.branchID>(doc, ca?.BranchID);
                Base.Document.UpdateCurrent();
            }
        }

        /// <summary>
        /// 檢核明細BranchID 是否與表頭一致，不一致則跳確認
        /// </summary>
        /// <returns></returns>
        protected virtual WebDialogResult ValidateBranch()
        {
            var doc = Base.Document.Current;
            var ca = CashAccount.PK.Find(Base, doc.CashAccountID);
            Branch hb = Branch.PK.Find(Base, doc.BranchID);
            Branch db = Branch.PK.Find(Base, ca?.BranchID);
            if (doc.BranchID != ca?.BranchID)
            {
                return Base.Document.Ask(
                            $"Cash Account {db?.BranchCD} is differenct from Finance {hb?.BranchCD}. This will lead to inter-branch posting, please confirm whether you want to continue."
                            , MessageButtons.OKCancel);
            }
            return WebDialogResult.OK;
        }
        #endregion
    }
}
