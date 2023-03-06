using PX.Data;
using PX.Objects.PO;
using PX.Objects.GL;
using System.Collections;

namespace PX.Objects.AP
{
    public class APInvoiceEntryHHExt : PXGraphExtension<APInvoiceEntry>
    {
        #region View
        public PXSelectJoin<NoteDoc,
                InnerJoin<NoteDoc2, On<NoteDoc2.fileID, Equal<NoteDoc.fileID>>,
                InnerJoin<POOrder, On<POOrder.noteID, Equal<NoteDoc2.noteID>>>>,
                Where<NoteDoc.noteID, Equal<Current<APInvoice.noteID>>>> FileLinksByPO;
        #endregion

        #region Override
        #region Persist
        public delegate void PersistDelegate();
        [PXOverride]
        public virtual void Persist(PersistDelegate baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                LinkFileByPO();
                baseMethod();
                ts.Complete();
            }
        }
        #endregion

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
        protected virtual void _(Events.RowPersisting<APInvoice> e) {
            LinkBranch();
        }

        #endregion

        #region Method
        /// <summary>
        /// APInvoice新增時自動綁定明細第一筆BranchID
        /// </summary>
        /// <returns></returns>
        protected virtual void LinkBranch()
        {
            APInvoice invoice = Base.Document.Current;
            if (Base.Document.Cache.GetStatus(invoice) == PXEntryStatus.Inserted)
            {
                APTran tran = Base.Transactions.Select();
                if (tran?.BranchID != null) { 
                    Base.Document.Cache.SetValueExt<APInvoice.branchID>(invoice, tran.BranchID);
                    Base.Document.UpdateCurrent();
                }
            }
        }

        /// <summary>
        /// 檢核明細BranchID 是否與表頭一致，不一致則跳確認
        /// </summary>
        /// <returns></returns>
        protected virtual WebDialogResult ValidateBranch()
        {
            APInvoice invoice = Base.Document.Current;
            foreach (APTran tran in Base.Transactions.Select())
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

        /// <summary>
        /// 關聯PO檔案
        /// </summary>
        protected virtual void LinkFileByPO()
        {
            APInvoice invoice = Base.Document.Current;
            //Clear All Link by header
            foreach (NoteDoc doc in FileLinksByPO.Select())
            {
                FileLinksByPO.Delete(doc);
            }

            foreach (APTran tran in Base.Transactions.Select())
            {
                //Copy Header
                POOrder order = POOrder.PK.Find(Base, tran.POOrderType, tran.PONbr);
                PXCache<POOrder> poCache = Base.Caches<POOrder>();
                PXNoteAttribute.CopyNoteAndFiles(poCache, order, Base.Document.Cache, invoice, false, true);
                //Copy Detail
                POLine line = POLine.PK.Find(Base, tran.POOrderType, tran.PONbr, tran.POLineNbr);
                PXCache<POLine> lineCache = Base.Caches<POLine>();
                PXNoteAttribute.CopyNoteAndFiles(lineCache, line, Base.Transactions.Cache, tran, false, true);
            }
        }
        #endregion

    }
}
