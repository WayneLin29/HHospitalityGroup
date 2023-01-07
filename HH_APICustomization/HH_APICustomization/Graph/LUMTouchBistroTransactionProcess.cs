using System;
using HH_APICustomization.DAC;
using PX.Data;
using TSDataType = HH_APICustomization.Descriptor.LUMTBTransactionSummaryType;
using System.Collections;
using System.Collections.Generic;
using HH_APICustomization.Graph;
using PX.Objects.GL;
using System.Linq;

namespace HH_APICustomization.Grpah
{
    /**
     * DataType : Account Refresh : 只刷新isImported = false的資料
     * Mark As: 取消已被Import的資料
     * 
     * **/
    public class LUMTouchBistroTransactionProcess : PXGraph<LUMTouchBistroTransactionProcess>
    {

        public LUMTouchBistroTransactionProcess()
        {
            LUMTouchBistroTransactionProcess self = this;
            Transactions.SetProcessDelegate(list => DoProcess(self, list));
        }

        #region View
        public PXFilter<ProcessFilter> Filter;
        public PXFilteredProcessing<LUMTBTransactionSummary, ProcessFilter,
            Where<LUMTBTransactionSummary.isImported, Equal<Current<ProcessFilter.isImported>>,
                And<Where<LUMTBTransactionSummary.dataType, Equal<Current<ProcessFilter.dataType>>,
                    Or<Current<ProcessFilter.dataType>, Equal<ProcessFilter.accountRefresh>>>>>,
            OrderBy<
                Asc<LUMTBTransactionSummary.dataType,
                Asc<LUMTBTransactionSummary.restaurantID,
                Asc<LUMTBTransactionSummary.restaurantID,
                Asc<LUMTBTransactionSummary.accountID,
                Asc<LUMTBTransactionSummary.subID,
                Asc<LUMTBTransactionSummary.salesCategory,
                Asc<LUMTBTransactionSummary.menuGroup,
                Asc<LUMTBTransactionSummary.menuItem,
                Asc<LUMTBTransactionSummary.reason,
                Asc<LUMTBTransactionSummary.accountName>>>>>>>>>>
                >>
            Transactions;
        #endregion

        #region Action
        public PXAction<ProcessFilter> markImported;
        [PXUIField(DisplayName = "Mark Imported", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXProcessButton]
        public virtual IEnumerable MarkImported(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                foreach (LUMTBTransactionSummary item in Transactions.Select())
                {
                    if (item.Selected == true)
                    {
                        item.IsImported = false;
                        this.Caches<LUMTBTransactionSummary>().Update(item);
                    }
                }
                this.Persist();
            });
            return adapter.Get();
        }
        #endregion

        #region Event
        protected virtual void _(Events.RowSelected<ProcessFilter> e)
        {
            if (e.Row == null) return;
            bool isAR = e.Row.DataType == ProcessFilter.AccountRefresh;
            PXUIFieldAttribute.SetEnabled<ProcessFilter.isImported>(e.Cache, e.Row, !isAR);
            this.markImported.SetVisible(e.Row.IsImported ?? false);
            bool isSalesByMenuItem = e.Row.DataType == TSDataType.SALES_BY_MENUITEM;
            bool isAccountSum = e.Row.DataType == TSDataType.ACCOUNTS_SUMMARY;
            bool isInOut = e.Row.DataType == TSDataType.PAY_INS || e.Row.DataType == TSDataType.PAY_OUTS;
            #region SalesByMenuItem
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.menuItem>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.salesCategory>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.menuGroup>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.menuItemVoidQty>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.voids>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.menuItemQty>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.grossSales>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.discounts>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.netSales>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.tax1>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.tax2>(Transactions.Cache, null, isSalesByMenuItem);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.tax3>(Transactions.Cache, null, isSalesByMenuItem);
            #endregion
            #region AccountsSummary
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.accountName>(Transactions.Cache, null, isAccountSum);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.payments>(Transactions.Cache, null, isAccountSum);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.deposits>(Transactions.Cache, null, isAccountSum);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.chargedToAccount>(Transactions.Cache, null, isAccountSum);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.subtotal>(Transactions.Cache, null, isAccountSum);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.tips>(Transactions.Cache, null, isAccountSum);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.total>(Transactions.Cache, null, isAccountSum);
            #endregion
            #region PayOutsIns
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.server>(Transactions.Cache, null, isInOut);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.dateTimestamp>(Transactions.Cache, null, isInOut);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.reason>(Transactions.Cache, null, isInOut);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.amount>(Transactions.Cache, null, isInOut);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.register>(Transactions.Cache, null, isInOut);
            PXUIFieldAttribute.SetVisible<LUMTBTransactionSummary.comment>(Transactions.Cache, null, isInOut);
            #endregion
        }

        protected virtual void _(Events.FieldUpdated<ProcessFilter, ProcessFilter.dataType> e)
        {
            if (e.Row == null) return;
            if (e.Row.DataType == ProcessFilter.AccountRefresh)
                e.Cache.SetValueExt<ProcessFilter.isImported>(e.Row, false);
            Transactions.Cache.Clear();
        }

        protected virtual void _(Events.FieldUpdated<ProcessFilter, ProcessFilter.isImported> e)
        {
            if (e.Row == null) return;
            Transactions.Cache.Clear();
        }
        #endregion

        #region Method
        public static void DoProcess(LUMTouchBistroTransactionProcess self, List<LUMTBTransactionSummary> list)
        {
            if (self.Filter.Current?.DataType == ProcessFilter.AccountRefresh)
            {
                self.AccountRefresh(list);
            }
            else
            {
                self.CreateBatch(list);
            }
            self.Persist();
        }

        public void AccountRefresh(List<LUMTBTransactionSummary> list)
        {
            foreach (LUMTBTransactionSummary item in list)
            {
                if (item.DataType == TSDataType.SALES_BY_MENUITEM)
                {
                    var mapping = LUMTouchBistroPreferenceMaint.GetSalesByMenuItemAcct(this, item);
                    item.AccountID = mapping?.AccountID;
                    item.SubID = mapping?.SubAcctID;
                }
                else if (item.DataType == TSDataType.ACCOUNTS_SUMMARY)
                {
                    var mapping = LUMTouchBistroPreferenceMaint.GetAccountsSummaryAcct(this, item);
                    item.AccountID = mapping?.AccountID;
                    item.SubID = mapping?.SubAcctID;
                }
                else if (item.DataType == TSDataType.PAY_INS || item.DataType == TSDataType.PAY_OUTS)
                {
                    var mapping = LUMTouchBistroPreferenceMaint.GetPayOutsInsAcct(this, item);
                    item.AccountID = mapping?.AccountID;
                    item.SubID = mapping?.SubAcctID;
                }
                this.Caches<LUMTBTransactionSummary>().Update(item);
            }
        }

        public void CreateBatch(List<LUMTBTransactionSummary> list)
        {
            var dataType = this.Filter.Current.DataType;
            Ledger ledger = Ledger.UK.Find(this, "ACTUAL");
            List<LUMTBTransactionSummary> processData = ValidateAccountAndSub(list);
            var groupBy = processData.GroupBy(item => new { item.RestaurantID, item.DateTimestamp });
            foreach (var groupData in groupBy)
            {
                List<LUMTBTransactionSummary> groupList = groupData.ToList();
                LUMTouchBistroPreference preference = LUMTouchBistroPreference.PK.Find(this, groupData.Key.RestaurantID);
                string errorMsg = null;
                string batchNbr = null;
                try
                {
                    if (dataType == TSDataType.SALES_BY_MENUITEM) batchNbr = CreateBatchByMenuItem(preference, groupList, ledger?.LedgerID);
                    else if (dataType == TSDataType.ACCOUNTS_SUMMARY) batchNbr = CreateBatchByAccountsSummary(preference, groupList, ledger?.LedgerID);
                    else if (dataType == TSDataType.PAY_INS) batchNbr = CreateBatchByOutsIns(preference, groupList, ledger?.LedgerID, false);
                    else if (dataType == TSDataType.PAY_OUTS) batchNbr = CreateBatchByOutsIns(preference, groupList, ledger?.LedgerID, true);
                }
                catch (PXOuterException e)
                {
                    for (int j = 0; j < e.InnerFields.Length; j++)
                    {
                        errorMsg += $"[{e.InnerFields[j]} - {e.InnerMessages[j]}] \r\n";
                    }
                    errorMsg = e.Message;
                }
                catch (Exception e)
                {
                    errorMsg = $"{e.Message} \r\n {errorMsg}";
                }
                finally
                {
                    //Update Trnsaction Item
                    foreach (LUMTBTransactionSummary item in groupList)
                    {
                        item.ErrorMessage = errorMsg;
                        item.BatchNbr = batchNbr;
                        item.IsImported = batchNbr != null;
                        this.Caches<LUMTBTransactionSummary>().Update(item);
                        if (errorMsg != null) PXProcessing.SetError<LUMTBTransactionSummary>(item.ProcessIndex ?? 0, errorMsg);
                    }
                }
            }
        }

        private string CreateBatchByMenuItem(LUMTouchBistroPreference preference, List<LUMTBTransactionSummary> groupList, int? ledgerID)
        {
            JournalEntry entry = PXGraph.CreateInstance<JournalEntry>();
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                DateTime? date = groupList[0].DateTimestamp;
                string dateStr = String.Format("{0:yyyy-MM-dd}", date);
                #region Batch
                Batch batch = (Batch)entry.BatchModule.Cache.CreateInstance();
                batch = entry.BatchModule.Insert(batch);
                batch.Module = "GL";
                batch.DateEntered = date;
                batch.LedgerID = ledgerID;
                batch.BranchID = preference.Branch;
                batch.Description = "TouchBistro Sales" + dateStr;
                batch.CuryID = "PHP";
                batch = entry.BatchModule.Update(batch);
                #endregion

                #region GLTran
                foreach (LUMTBTransactionSummary item in groupList)
                {
                    #region Tran A
                    GLTran tranA = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                    tranA = entry.GLTranModuleBatNbr.Insert(tranA);
                    tranA.AccountID = item.AccountID;
                    tranA.SubID = item.SubID;
                    tranA.Qty = item.MenuItemQty;
                    tranA.TranDesc = String.Format("{0}-{1}-{2}", item.MenuGroup, item.SalesCategory, item.MenuItem);
                    tranA.CuryDebitAmt = 0m;
                    tranA.CuryCreditAmt = item.NetSales;
                    tranA = entry.GLTranModuleBatNbr.Update(tranA);
                    #endregion

                    #region Tran B
                    GLTran tranB = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                    tranB = entry.GLTranModuleBatNbr.Insert(tranB);
                    tranB.AccountID = preference.AccountID;
                    tranB.SubID = preference.SubAcctID;
                    tranB.TranDesc = tranA.TranDesc;
                    tranB.CuryDebitAmt = tranA.CuryCreditAmt;
                    tranB.CuryCreditAmt = tranA.CuryDebitAmt;
                    tranB = entry.GLTranModuleBatNbr.Update(tranB);
                    #endregion
                    //Set Transaction item LineNbr
                    item.LineNbr = tranA.LineNbr;
                }

                #endregion
                entry.releaseFromHold.Press();
                entry.Save.Press();
                ts.Complete(entry);
                return batch.BatchNbr;
            }
        }

        private string CreateBatchByAccountsSummary(LUMTouchBistroPreference preference, List<LUMTBTransactionSummary> groupList, int? ledgerID)
        {
            JournalEntry entry = PXGraph.CreateInstance<JournalEntry>();
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                DateTime? date = groupList[0].DateTimestamp;
                string dateStr = String.Format("{0:yyyy-MM-dd}", date);
                #region Batch
                Batch batch = (Batch)entry.BatchModule.Cache.CreateInstance();
                batch = entry.BatchModule.Insert(batch);
                batch.Module = "GL";
                batch.DateEntered = date;
                batch.LedgerID = ledgerID;
                batch.BranchID = preference.Branch;
                batch.Description = "TouchBistro Pay Account" + dateStr;
                batch.CuryID = "PHP";
                batch = entry.BatchModule.Update(batch);
                #endregion

                #region GLTran
                foreach (LUMTBTransactionSummary item in groupList)
                {
                    #region Tran A
                    GLTran tranA = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                    tranA = entry.GLTranModuleBatNbr.Insert(tranA);
                    tranA.AccountID = item.AccountID;
                    tranA.SubID = item.SubID;
                    tranA.TranDesc = String.Format("Account Audit - {0}", item.AccountName);
                    tranA.CuryDebitAmt = item.Total;
                    tranA.CuryCreditAmt = 0m;
                    tranA = entry.GLTranModuleBatNbr.Update(tranA);
                    #endregion

                    #region Tran B
                    GLTran tranB = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                    tranB = entry.GLTranModuleBatNbr.Insert(tranB);
                    tranB.AccountID = preference.AccountID;
                    tranB.SubID = preference.SubAcctID;
                    tranB.TranDesc = tranA.TranDesc;
                    tranB.CuryDebitAmt = tranA.CuryCreditAmt;
                    tranB.CuryCreditAmt = tranA.CuryDebitAmt;
                    tranB = entry.GLTranModuleBatNbr.Update(tranB);
                    #endregion
                    //Set Transaction item LineNbr
                    item.LineNbr = tranA.LineNbr;
                }

                #endregion
                entry.releaseFromHold.Press();
                entry.Save.Press();
                ts.Complete(entry);
                return batch.BatchNbr;
            }
        }

        private string CreateBatchByOutsIns(LUMTouchBistroPreference preference, List<LUMTBTransactionSummary> groupList, int? ledgerID, bool isOut)
        {
            JournalEntry entry = PXGraph.CreateInstance<JournalEntry>();
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                DateTime? date = groupList[0].DateTimestamp;
                string dateStr = String.Format("{0:yyyy-MM-dd}", date);
                #region Batch
                Batch batch = (Batch)entry.BatchModule.Cache.CreateInstance();
                batch = entry.BatchModule.Insert(batch);
                batch.Module = "GL";
                batch.DateEntered = date;
                batch.LedgerID = ledgerID;
                batch.BranchID = preference.Branch;
                batch.Description = "TouchBistro Remittance" + dateStr;
                batch.CuryID = "PHP";
                batch = entry.BatchModule.Update(batch);
                #endregion

                #region GLTran
                foreach (LUMTBTransactionSummary item in groupList)
                {
                    #region Tran A
                    GLTran tranA = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                    tranA = entry.GLTranModuleBatNbr.Insert(tranA);
                    tranA.AccountID = item.AccountID;
                    tranA.SubID = item.SubID;
                    tranA.TranDesc = String.Format("Account Remit - {0}", item.Reason);
                    tranA.CuryDebitAmt = isOut ? 0m : item.Amount;
                    tranA.CuryCreditAmt = isOut ? item.Amount : 0m;
                    tranA = entry.GLTranModuleBatNbr.Update(tranA);
                    #endregion

                    #region Tran B
                    GLTran tranB = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                    tranB = entry.GLTranModuleBatNbr.Insert(tranB);
                    tranB.AccountID = preference.AccountID;
                    tranB.SubID = preference.SubAcctID;
                    tranB.TranDesc = tranA.TranDesc;
                    tranB.CuryDebitAmt = tranA.CuryCreditAmt;
                    tranB.CuryCreditAmt = tranA.CuryDebitAmt;
                    tranB = entry.GLTranModuleBatNbr.Update(tranB);
                    #endregion
                    //Set Transaction item LineNbr
                    item.LineNbr = tranA.LineNbr;
                }

                #endregion
                entry.releaseFromHold.Press();
                entry.Save.Press();
                ts.Complete(entry);
                return batch.BatchNbr;
            }
        }

        private List<LUMTBTransactionSummary> ValidateAccountAndSub(List<LUMTBTransactionSummary> list)
        {
            List<LUMTBTransactionSummary> processData = new List<LUMTBTransactionSummary>();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                item.ProcessIndex = i;
                PXProcessing.SetCurrentItem(item);
                try
                {
                    //無對應Account & sub
                    if (item.AccountID == null || item.SubID == null)
                        throw new PXException($"No account mapping found. Please maintain account mapping.");
                    //寫入代處理資料
                    processData.Add(item);
                }
                catch (Exception e)
                {
                    item.ErrorMessage = e.Message;
                    PXProcessing.SetError<LUMTBTransactionSummary>(item.ProcessIndex ?? 0, e.Message);
                    this.Caches<LUMTBTransactionSummary>().Update(item);
                }
            }
            return processData;
        }
        #endregion

        #region Table
        [Serializable]
        [PXCacheName("Process Filter")]
        public class ProcessFilter : IBqlTable
        {
            #region IsImported
            [PXBool]
            [PXUIField(DisplayName = "Is Import")]
            [PXUnboundDefault(false)]
            public virtual bool? IsImported { get; set; }
            public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
            #endregion

            #region DataType
            [PXString(2, IsFixed = true, IsUnicode = true, InputMask = "")]
            [PXUIField(DisplayName = "Data Type")]
            [PXStringList(
                new String[] {
                    TSDataType.SALES_BY_MENUITEM,
                    TSDataType.ACCOUNTS_SUMMARY,
                    TSDataType.PAY_INS,
                    TSDataType.PAY_OUTS,
                    AccountRefresh },
                new String[] {
                    "Sales By Menu Item",
                    "Accounts Summary",
                    "PayIns",
                    "PayOuts",
                    "Account Refresh" }
                )]
            public virtual string DataType { get; set; }
            public abstract class dataType : PX.Data.BQL.BqlString.Field<dataType> { }
            #endregion

            /// <summary> Account Refresh </summary>
            public const string AccountRefresh = "AR";
            /// <summary> Account Refresh </summary>
            public class accountRefresh : PX.Data.BQL.BqlString.Constant<accountRefresh> { public accountRefresh() : base(AccountRefresh) { } }
        }
        #endregion

    }
}