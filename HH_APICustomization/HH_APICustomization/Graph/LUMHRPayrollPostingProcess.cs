using HH_APICustomization.DAC;
using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using PX.Objects.GL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Graph
{
    public class LUMHRPayrollPostingProcess : PXGraph<LUMHRPayrollPostingProcess, LUMHRPayrollBaseDocument>
    {
        public PXSetup<LUMHHSetup> Setup;

        public SelectFrom<LUMHRPayrollBaseDocument>.View Document;

        [PXImport(typeof(LUMHRPayrollBaseDetails))]
        public SelectFrom<LUMHRPayrollBaseDetails>
              .Where<LUMHRPayrollBaseDetails.docRefNbr.IsEqual<LUMHRPayrollBaseDocument.docRefNbr.FromCurrent>>
              .View Transactions;



        #region Action
        public PXAction<LUMHRPayrollBaseDocument> CreateJournalTransaction;
        [PXButton(Connotation = PX.Data.WorkflowAPI.ActionConnotation.Primary)]
        [PXUIField(DisplayName = "Mass Posting", MapEnableRights = PXCacheRights.Select)]
        public IEnumerable createJournalTransaction(PXAdapter adapter)
        {
            var data = this.Transactions.Select().RowCast<LUMHRPayrollBaseDetails>();
            if (!ValidPostingData(data))
                throw new PXException("Please check the error message below");
            PXLongOperation.StartOperation(this, () =>
            {
                using (PXTransactionScope sc = new PXTransactionScope())
                {
                    try
                    {
                        // 最後更新Jounal Transcation的對應表
                        var mappingTable = new List<FIledgerMapTable>();
                        var glGraph = PXGraph.CreateInstance<JournalEntry>();
                        var ledgerInfo = SelectFrom<Ledger>
                                        .Where<Ledger.ledgerCD.IsEqual<P.AsString>>
                                        .View.Select(this, "FIN").TopFirst;
                        foreach (var groupData in data.GroupBy(x => x.OriginBatchNbr))
                        {
                            var mapData = new FIledgerMapTable();
                            var _FIBatchNbr = $"FI{groupData.Key}";
                            using (new PXReadDeletedScope())
                            {
                                var FILedger = SelectFrom<Batch>
                                              .Where<Batch.batchNbr.IsEqual<P.AsString>>
                                              .View.Select(this, _FIBatchNbr).TopFirst;

                                #region Header
                                var doc = glGraph.BatchModule.Cache.CreateInstance() as Batch;
                                doc.Module = FILedger == null ? "FI" : "GL";
                                doc = glGraph.BatchModule.Cache.Insert(doc) as Batch;

                                if (doc.Module == "FI")
                                    glGraph.BatchModule.SetValueExt<Batch.batchNbr>(doc, _FIBatchNbr);
                                // 有FIN Ledger
                                if (!string.IsNullOrEmpty(ledgerInfo?.LedgerCD))
                                    glGraph.BatchModule.SetValueExt<Batch.ledgerID>(doc, ledgerInfo?.LedgerID);
                                glGraph.BatchModule.SetValueExt<Batch.branchID>(doc, groupData.FirstOrDefault()?.OriginBranchID);
                                glGraph.BatchModule.SetValueExt<Batch.dateEntered>(doc, DateTime.Now);
                                glGraph.Save.Press();
                                #endregion

                                #region Details
                                foreach (var item in groupData)
                                {
                                    var line = glGraph.GLTranModuleBatNbr.Cache.CreateInstance() as GLTran;
                                    line.BranchID = item?.PostingBranchID;
                                    line.AccountID = item?.AccountID;
                                    line.SubID = item?.SubID;
                                    line.TranDesc = item?.TranDesc;
                                    line.CuryDebitAmt = item?.DebitAmount ?? 0;
                                    line.CuryCreditAmt = item?.CreditAmount ?? 0;
                                    line.RefNbr = item?.RefNbr;
                                    line.GetExtension<GLTranExtension>().UsrTaxZone = item?.UsrTaxZone;
                                    line.GetExtension<GLTranExtension>().UsrTaxCategory = item?.UsrTaxCategory;
                                    line.GetExtension<GLTranExtension>().UsrPostOrigBatchNbr = item?.OriginBatchNbr;
                                    line.GetExtension<GLTranExtension>().UsrPostOrigLineNbr = item?.OriginLineNbr;
                                    line.Module = doc.Module;
                                    line.LedgerID = doc.LedgerID;
                                    line = glGraph.GLTranModuleBatNbr.Cache.Insert(line) as GLTran;
                                    glGraph.GLTranModuleBatNbr.SetValueExt<GLTran.module>(line, doc.Module);

                                    // 有OriginLineNbr 則更新對應的資料
                                    if (item?.OriginLineNbr != 0)
                                    {
                                        PXDatabase.Update<GLTran>(
                                            new PXDataFieldAssign<GLTranExtension.usrIsReviewed>(true),
                                            new PXDataFieldAssign<GLTranExtension.usrRvBatch>(doc.BatchNbr),
                                            new PXDataFieldAssign<GLTranExtension.usrRvLineNbr>(line.LineNbr),
                                            new PXDataFieldRestrict<GLTran.batchNbr>(item?.OriginBatchNbr),
                                            new PXDataFieldRestrict<GLTran.lineNbr>(item?.OriginLineNbr));
                                    }
                                    else
                                    {
                                        PXDatabase.Update<GLTran>(
                                           new PXDataFieldAssign<GLTranExtension.usrIsReviewed>(true),
                                           new PXDataFieldAssign<GLTranExtension.usrRvBatch>(doc.BatchNbr),
                                           new PXDataFieldAssign<GLTranExtension.usrRvLineNbr>(0),
                                           new PXDataFieldRestrict<GLTran.batchNbr>(item?.OriginBatchNbr));
                                    }
                                }
                                #endregion
                            }
                            glGraph.Save.Press();
                            glGraph.releaseFromHold.Press();
                            glGraph.release.Press();
                        }

                        this.Document.Current.ProcessTime = DateTime.Now;
                        this.Document.UpdateCurrent();
                        this.Save.Press();
                        sc.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            });
            return adapter.Get();
        }
        #endregion

        #region Event
        public virtual void _(Events.FieldDefaulting<LUMHRPayrollBaseDetails.lineNbr> e)
        {
            var currentList = this.Transactions.Select().RowCast<LUMHRPayrollBaseDetails>();
            var maxLineNbr = currentList.Count() == 0 ? 0 : currentList.Max(x => x?.LineNbr ?? 0);
            e.NewValue = maxLineNbr + 1;
        }
        #endregion

        #region Method

        /// <summary>
        /// Validation upload data
        /// </summary>
        /// <param name="data"> upload data </param>
        /// <returns></returns>
        public virtual bool ValidPostingData(IEnumerable<LUMHRPayrollBaseDetails> data)
        {
            var valid = true;
            foreach (var groupBatch in data.GroupBy(x => x.OriginBatchNbr))
            {
                #region  檢查2. LIneNbr 要馬全為0 要馬全部為0/LineNbr 若全為0, 則該Batch 不可以有任何 UsrIsReviewed = 1
                // 檢查2. LIneNbr 要馬全為0 要馬全部為0 - Error Msg: Please confirm [batchnbr] whether it is linked by line or by journal entry.
                if (groupBatch.Count(x => x.OriginLineNbr != 0) > 0 && groupBatch.Count(x => x.OriginLineNbr == 0) > 0)
                {
                    groupBatch.ToList().ForEach(x =>
                    {
                        this.Transactions.Cache.RaiseExceptionHandling<LUMHRPayrollBaseDetails.originBatchNbr>(x, x.OriginBatchNbr,
                           new PXSetPropertyException<LUMHRPayrollBaseDetails.originBatchNbr>($"Error Msg: Please confirm [{x.OriginBatchNbr}] whether it is linked by line or by journal entry.", PXErrorLevel.Error));
                    });
                    valid = false;
                }
                // 檢查2a. LineNbr 若全為0, 則該Batch 不可以有任何 UsrIsReviewed = 1
                else if (groupBatch.Count(x => x.OriginLineNbr != 0) == 0)
                {
                    var _glTran = SelectFrom<GLTran>
                                .Where<GLTran.batchNbr.IsEqual<P.AsString>
                                  .And<GLTranExtension.usrIsReviewed.IsEqual<True>>>
                                .View.Select(this, groupBatch.Key).TopFirst;
                    if (_glTran != null)
                    {
                        groupBatch.ToList().ForEach(x =>
                        {
                            this.Transactions.Cache.RaiseExceptionHandling<LUMHRPayrollBaseDetails.originBatchNbr>(x, x.OriginBatchNbr,
                               new PXSetPropertyException<LUMHRPayrollBaseDetails.originBatchNbr>($"Error Msg: [{groupBatch.Key}] has been partially processed, cannot performed as full anymore. Please review your entry.", PXErrorLevel.Error));
                        });
                        valid = false;
                    }
                }
                #endregion

                // 前面已經檢查失敗 則略過
                if (!valid)
                    continue;

                foreach (var item in groupBatch)
                {
                    var _glTran = SelectFrom<GLTran>
                            .Where<GLTran.batchNbr.IsEqual<P.AsString>
                              .And<GLTran.lineNbr.IsEqual<P.AsInt>>>
                            .View.Select(this, item?.OriginBatchNbr, item?.OriginLineNbr).TopFirst;
                    #region 檢查1. LineNbr 不為0的話，需檢查 系統中是否存在對應的 Batch + LineNbr 
                    // 檢查1. LineNbr 不為0的話，需檢查 系統中是否存在對應的 Batch + LineNbr 
                    if (item?.OriginLineNbr != 0 && _glTran == null)
                    {
                        this.Transactions.Cache.RaiseExceptionHandling<LUMHRPayrollBaseDetails.originBatchNbr>(item, item.OriginBatchNbr,
                            new PXSetPropertyException<LUMHRPayrollBaseDetails.originBatchNbr>($"[{item?.OriginBatchNbr}] + [{item?.OriginLineNbr}] cannot be found, please confirm your entry.", PXErrorLevel.Error));
                        valid = false;
                    }
                    #endregion
                    #region 檢查3. Orign Batch - UsrIsReviewed 者 不可再執行一次 
                    // 檢查3. Orign Batch - UsrIsReviewed 者 不可再執行一次
                    if (_glTran.GetExtension<GLTranExtension>()?.UsrIsReviewed ?? false)
                    {
                        this.Transactions.Cache.RaiseExceptionHandling<LUMHRPayrollBaseDetails.originBatchNbr>(item, item.OriginBatchNbr,
                           new PXSetPropertyException<LUMHRPayrollBaseDetails.originBatchNbr>($"[{item?.OriginBatchNbr}] - [{item?.OriginLineNbr}] is already reviewed and posted, please confirm your entry.", PXErrorLevel.Error));
                        valid = false;
                    }
                    #endregion
                }
            }
            return valid;
        }

        #endregion
    }

    public class FIledgerMapTable
    {
        public string OriginBatchNbr { get; set; }
        public int? OriginLineNbr { get; set; }
        public string RvBatchNbr { get; set; }
        public int? RvLineNbr { get; set; }
    }
}
