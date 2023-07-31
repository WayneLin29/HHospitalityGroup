using PX.Data;
using PX.Objects.CA;
using PX.Objects.GL;
using System;
using System.Collections;

namespace PX.Objects.AP
{
    public class APPaymentEntryHHExt : PXGraphExtension<APPaymentEntry>
    {
        #region AttributeName
        public const string UD_BANKCODE = "AttributeBANKCODE";
        public const string UD_BANK = "AttributeBANK";
        public const string UD_BANKACCT = "AttributeBANKACCT";
        public const string UD_PAYEECLASS = "AttributePAYEECLASS";
        public const string UD_BANKACCTN = "AttributeBANKACCTN";
        public const string UD_BANKACCTF = "AttributeBANKACCTF";
        public const string UD_BANKACCTM = "AttributeBANKACCTM";
        public const string UD_BANKACCTL = "AttributeBANKACCTL";
        public const string UD_BANKACCTAD = "AttributeBANKACCTAD";
        #endregion

        #region View
        public PXSelectJoin<NoteDoc,
                InnerJoin<NoteDoc2, On<NoteDoc2.fileID, Equal<NoteDoc.fileID>>,
                InnerJoin<APInvoice, On<APInvoice.noteID, Equal<NoteDoc2.noteID>>>>,
                Where<NoteDoc.noteID, Equal<Current<APInvoice.noteID>>>> FileLinksByAP;
        #endregion

        #region Override
        #region Persist
        public delegate void PersistDelegate();
        [PXOverride]
        public virtual void Persist(PersistDelegate baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                //baseMethod();
                APPayment invoice = Base.Document.Current;
                if (invoice != null && Base.Document.Cache.GetStatus(invoice) != PXEntryStatus.Deleted)
                {
                    LinkFileByAdjdAP();
                    LinkBranch();
                }
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

        #region Events
        public virtual void _(Events.FieldUpdated<APPayment, APPayment.vendorID> e)
        {
            if (e.Row == null || e.Row.VendorID == null) return;
            string[] copyList = { UD_BANKCODE, UD_BANK, UD_BANKACCT, UD_PAYEECLASS, UD_BANKACCTN, UD_BANKACCTF, UD_BANKACCTM, UD_BANKACCTL, UD_BANKACCTAD };
            Vendor vendor = Vendor.PK.Find(Base, e.Row.VendorID);
            foreach (var copyName in copyList)
            {
                var value = Base.Caches<Vendor>().GetValueExt(vendor, copyName);
                e.Cache.SetValueExt(e.Row, copyName, value);
                var _value = e.Cache.GetValueExt(e.Row, copyName);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// APInvoice新增時自動綁定明細第一筆BranchID
        /// </summary>
        /// <returns></returns>
        protected virtual void LinkBranch()
        {
            APPayment invoice = Base.Document.Current;
            if (Base.Document.Cache.GetStatus(invoice) == PXEntryStatus.Inserted)
            {
                CashAccount ca = CashAccount.PK.Find(Base, invoice.CashAccountID);
                if (ca?.BranchID != null)
                {
                    Base.Document.Cache.SetValueExt<APPayment.branchID>(invoice, ca.BranchID);
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
            APPayment payment = Base.Document.Current;
            if (payment.CashAccountID == null) return WebDialogResult.OK;
            CashAccount ca = CashAccount.PK.Find(Base, payment.CashAccountID);
            if (payment.BranchID != ca.BranchID)
            {
                Branch hb = Branch.PK.Find(Base, payment.BranchID);
                Branch db = Branch.PK.Find(Base, ca.BranchID);
                return Base.Document.Ask(
                    $"Document Branch - [{hb?.BranchCD?.Trim()}] differs from bank account branch [{db?.BranchCD?.Trim()}]. This will result in inter-company posting. Are you sure you want to proceed?"
                    , MessageButtons.OKCancel);
            }
            return WebDialogResult.OK;
        }
        /// <summary>
        /// 關聯AP檔案
        /// </summary>
        protected virtual void LinkFileByAdjdAP()
        {
            APPayment payment = Base.Document.Current;
            //Clear All Link by header
            foreach (NoteDoc doc in FileLinksByAP.Select())
            {
                FileLinksByAP.Delete(doc);
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
