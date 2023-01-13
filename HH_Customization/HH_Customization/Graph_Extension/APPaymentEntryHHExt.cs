using PX.Data;
using PX.Objects.PO;
using System;

namespace PX.Objects.AP
{
    public class APPaymentEntryHHExt : PXGraphExtension<APPaymentEntry>
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
                LinkFileByAdjdAP();
                baseMethod();
                ts.Complete();
            }
        }
        #endregion

        #region Method
        protected virtual void LinkFileByAdjdAP()
        {
            APPayment payment = Base.Document.Current;
            //Clear All Link by header
            foreach (NoteDoc doc in FileLinks.Select()) {
                FileLinks.Delete(doc);
            }

            foreach (APAdjust tran in Base.Adjustments.Select())
            {
                //Copy Header
                APInvoice invoice = APInvoice.PK.Find(Base, tran.AdjdDocType, tran.AdjdRefNbr);
                PXCache<APInvoice> invCache = Base.Caches<APInvoice>();
                PXNoteAttribute.CopyNoteAndFiles(invCache, invoice, Base.Document.Cache, payment, false, true);
            }
        }
        #endregion

    }
}
