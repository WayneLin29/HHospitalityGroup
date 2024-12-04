using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.PO;
using PX.Objects.GL;
using System.Collections;
using PX.Objects.CA;
using PX.Objects.SO;
using System.Collections.Generic;
using HH_Customization.DAC;
using PX.Common;
using static PX.Objects.AP.APInvoiceEntryHHExt;

namespace PX.Objects.AP
{
    public class APInvoiceEntryHHExt : PXGraphExtension<APInvoiceEntry>
    {
        #region AttributeName
        public const string UD_MAINBANK = "AttributeMAINBANK";
        #endregion

        #region View
        public PXSelectJoin<NoteDoc,
                InnerJoin<NoteDoc2, On<NoteDoc2.fileID, Equal<NoteDoc.fileID>>,
                InnerJoin<POOrder, On<POOrder.noteID, Equal<NoteDoc2.noteID>>>>,
                Where<NoteDoc.noteID, Equal<Current<APInvoice.noteID>>>> FileLinksByPO;

        public PXFilter<AssignToSoFilter> AssignToSOFilters;
        #endregion

        #region Override
        #region Persist
        public delegate void PersistDelegate();
        [PXOverride]
        public virtual void Persist(PersistDelegate baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                APInvoice invoice = Base.Document.Current;
                if (invoice != null && Base.Document.Cache.GetStatus(invoice) != PXEntryStatus.Deleted)
                {
                    LinkFileByPO();
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

        #region Action
        public PXAction<APInvoice> assignToSO;
        [PXButton(CommitChanges = true, DisplayOnMainToolbar = false),
        PXUIField(DisplayName = "Assign To SO", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public virtual void AssignToSO()
        {
            if (AssignToSOFilters.AskExt() == WebDialogResult.OK)
            {
                CreateSOItem();
            }
        }
        #endregion

        #region Event
        public virtual void _(Events.RowSelected<APInvoice> e)
        {
            assignToSO.SetEnabled(true);
            if (e.Cache.GetStatus(e.Row) == PXEntryStatus.Inserted)
                assignToSO.SetEnabled(false);
        }
        protected virtual void _(Events.RowPersisting<APInvoice> e)
        {
            LinkBranch();
            SetPayAccount4CashAccount();
        }
        #region APTran
        public virtual void _(Events.RowDeleting<APTran> e)
        {
            var row = e.Row;
            if (row == null) return;
            var soItem = GetLUMTourItem(Base, row.RefNbr, row.TranType, row.LineNbr);
            if (soItem != null)
            {
                var so = SOOrder.PK.Find(Base, soItem.SOOrderType, soItem.SOOrderNbr);
                if (so.Status.IsNotIn(SOOrderStatus.Hold, SOOrderStatus.Open, SOOrderStatus.Cancelled))
                {
                    string statusLabel = SOOrderStatus.ListWithoutOrdersAttribute.GetLocalizedLabel<SOOrder.status>(e.Cache, e.Row);
                    throw new PXException($" Item is linke to a Sales Order [{so.OrderNbr}], status [{statusLabel}] does not allow deletion.");
                }
            }
        }

        public virtual void _(Events.RowPersisted<APTran> e)
        {
            var row = e.Row;
            if (row == null) return;
            if (e.Cache.GetStatus(e.Row) == PXEntryStatus.Deleted)
            {
                //移除SO Item
                DeleteSOItem(e.Row);
            }
        }
        #endregion

        #endregion

        #region Method
        public virtual void CreateSOItem()
        {
            AssignToSoFilter filter = AssignToSOFilters.Current;
            if (filter?.SOOrderNbr == null) return;
            SOOrderEntry entry = PXGraph.CreateInstance<SOOrderEntry>();
            var entryExt = entry.GetExtension<SOOrderEntryHHExt>();
            entry.Document.Current = GetSOByOrderNbr(filter.SOOrderNbr);
            foreach (APTran tran in Base.Transactions.Select())
            {
                if (tran.Selected != true) continue;
                var item = GetLUMTourItem(entry, tran.RefNbr, tran.TranType, tran.LineNbr);
                if (item == null)
                {
                    item = entryExt.Items.Insert(new LUMTourItem());
                    item.APRefNbr = tran.RefNbr;
                    item.APDocType = tran.TranType;
                    item.APLineNbr = tran.LineNbr;
                }
                item.InventoryID = tran.InventoryID;
                item.Description = tran.TranDesc;
                item.VendorID = tran.VendorID;
                item.Pax = 0;
                item.UnitPrice = tran.CuryUnitCost;
                item.ExtCost = (tran.CuryLineAmt ?? 0) * (tran.TranType.IsIn(APDocType.Invoice, APDocType.DebitAdj) ? 1 : -1);
                item.AccountID = tran.AccountID;
                item.SubID = tran.SubID;

                entryExt.Items.Update(item);
            }
            entry.Save.Press();

        }
        public virtual void DeleteSOItem(APTran row)
        {
            SOOrderEntry entry = PXGraph.CreateInstance<SOOrderEntry>();
            var soItem = GetLUMTourItem(entry, row.RefNbr, row.TranType, row.LineNbr);
            if (soItem == null) return;
            entry.Document.Current = SOOrder.PK.Find(entry, soItem.SOOrderType, soItem.SOOrderNbr);
            var entryExt = entry.GetExtension<SOOrderEntryHHExt>();
            entryExt.Items.Delete(soItem);
            entry.Save.Press();
        }

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
                if (tran?.BranchID != null)
                {
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

        /// <summary>
        /// 更新PayAccountID為該Branch對應的CashAccount(Is Main Bank = true) 之AccountID
        /// , ps. 抓到多筆或沒抓到就不更新
        /// </summary>
        public virtual void SetPayAccount4CashAccount()
        {
            var doc = Base.Document.Current;

            if (Base.Document.Cache.GetStatus(doc) == PXEntryStatus.Inserted)
            {
                var cashAccounts = GetCashAccountByBranch(doc.BranchID);
                List<int?> accountIDs = new List<int?>();
                foreach (CashAccount ca in cashAccounts)
                {
                    PXFieldState isMainBank = (PXFieldState)Base.Caches<CashAccount>().GetValueExt(ca, UD_MAINBANK);
                    if (((bool?)isMainBank?.Value) == true) accountIDs.Add(ca.CashAccountID);
                }
                if (accountIDs.Count == 1)
                {
                    int? payAccountID = accountIDs[0];
                    Base.Document.Cache.SetValueExt<APInvoice.payAccountID>(doc, payAccountID);
                    Base.Document.UpdateCurrent();
                }
            }
        }
        #endregion

        #region BQL
        public virtual PXResultset<CashAccount> GetCashAccountByBranch(int? branchID)
        {
            return SelectFrom<CashAccount>
                .Where<CashAccount.branchID.IsEqual<@P.AsInt>
                .And<CashAccount.active.IsEqual<True>>>
                .View.Select(Base, branchID);
        }

        public virtual LUMTourItem GetLUMTourItem(PXGraph graph, string refNbr, string docType, int? lineNbr)
        {
            return SelectFrom<LUMTourItem>
                .Where<LUMTourItem.aPRefNbr.IsEqual<P.AsString>
                .And<LUMTourItem.aPDocType.IsEqual<P.AsString>
                .And<LUMTourItem.aPLineNbr.IsEqual<P.AsInt>>>>
                .View.Select(graph, refNbr, docType, lineNbr);
        }

        public virtual SOOrder GetSOByOrderNbr(string orderNbr)
        {
            return SelectFrom<SOOrder>.Where<SOOrder.orderNbr.IsEqual<P.AsString>>.View.Select(Base, orderNbr);
        }
        #endregion

        #region Table
        [PXHidden]
        public class AssignToSoFilter : PXBqlTable, IBqlTable
        {
            #region SOOrderNbr
            [PXString()]
            [PXUIField(DisplayName = "Order Nbr")]
            [PXSelector(typeof(Search<SOOrder.orderNbr, Where<SOOrder.status, In3<SOOrderStatus.open, SOOrderStatus.hold>>>),
                typeof(SOOrder.orderNbr),
                typeof(SOOrder.orderType),
                typeof(SOOrder.orderDesc),
                typeof(SOOrder.orderDate),
                typeof(SOOrder.curyOrderTotal),
                DescriptionField = typeof(SOOrder.orderDesc)
                )]
            public virtual string SOOrderNbr { get; set; }
            public abstract class soOrderNbr : PX.Data.BQL.BqlString.Field<soOrderNbr> { }
            #endregion
        }
        #endregion

    }
}
