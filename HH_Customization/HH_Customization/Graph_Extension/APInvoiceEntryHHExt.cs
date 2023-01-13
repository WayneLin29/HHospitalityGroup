using PX.Data;
using PX.Objects.PO;
using System;

namespace PX.Objects.AP
{
    public class APInvoiceEntryHHExt : PXGraphExtension<APInvoiceEntry>
    {
        #region View
        public PXSelect<NoteDoc,
                Where<NoteDoc.noteID, Equal<Current<APInvoice.noteID>>>> FileLinks;
        #endregion

        #region Override
        public delegate void PersistDelegate();
        [PXOverride]
        public virtual void Persist(PersistDelegate baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                //baseMethod();
                LinkFileByPO();
                baseMethod();
                ts.Complete();
            }
        }
        #endregion

        #region Method
        protected virtual void LinkFileByPO()
        {
            APInvoice invoice = Base.Document.Current;
            //Clear All Link by header
            foreach (NoteDoc doc in FileLinks.Select()) {
                FileLinks.Delete(doc);
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
