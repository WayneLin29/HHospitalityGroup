using HH_APICustomization.DAC;
using HH_APICustomization.Descriptor;
using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.GL
{
    public class JournalEntryExtension : PXGraphExtension<JournalEntry>
    {
        public SelectFrom<LUMHHSetup>.View HHSetup;

        public const string combinationErrorMsg = "please confirm your entry or maintain it in allowed combination.";

        #region Override Method
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            // Valid combination if Module != GL and IsReleased
            var doc = Base.BatchModule.Current;
            if (doc != null && doc?.Module != "GL")
            {
                PXTrace.WriteVerbose($"DEBUG_Combination: {doc?.Module}_{doc?.BatchNbr}_{doc.Status}");
                var allowTable = GetAllowTable();
                // 除了GL以外，其他都是直接先做好release資料在產生傳票

                var validResult = ValidCombination(allowTable, Base.GLTranModuleBatNbr.View.SelectMulti().RowCast<GLTran>(), true);
                if (!validResult.valid)
                    throw new PXException(validResult.errorMsg);
            }

            // When AP release, need to fill some column value
            if (doc != null && doc.Module == "AP")
            {
                foreach (var line in Base.GLTranModuleBatNbr.View.SelectMulti().RowCast<GLTran>())
                {
                    // 有跟AP關聯
                    var apTran = APTran.PK.Find(Base, line?.TranType, line?.RefNbr, line?.TranLineNbr);
                    if (apTran != null)
                    {
                        Base.GLTranModuleBatNbr.SetValueExt<GLTranExtension.usrTaxCategory>(line, apTran?.TaxCategoryID);
                        Base.GLTranModuleBatNbr.SetValueExt<GLTranExtension.usrTaxZone>(line, apTran?.GetExtension<APTranExtension>()?.UsrORTaxZone);
                        Base.GLTranModuleBatNbr.SetValueExt<GLTran.referenceID>(line, apTran?.GetExtension<APTranExtension>()?.UsrORVendor);
                    }
                }
            }
            baseMethod();
        }
        #endregion

        #region Override Action

        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public virtual IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            HHHelper helper = new HHHelper();
            var ledgerInfo_Actual = helper.GetActualLedgerInfo();
            List<Batch> list = new List<Batch>();

            foreach (object obj in adapter.Get())
            {
                Batch batch = null;
                if (obj is Batch)
                    batch = obj as Batch;
                else if (obj is PXResult)
                    batch = (obj as PXResult<Batch>);
                else
                    batch = (Batch)obj;
                if (batch.Status == BatchStatus.Balanced)
                    list.Add(batch);
            }

            // Valid allow
            foreach (var batchItem in list)
            {
                var validResult = ValidCombination(GetAllowTable(),
                                SelectFrom<GLTran>
                                .Where<GLTran.module.IsEqual<P.AsString>
                                  .And<GLTran.batchNbr.IsEqual<P.AsString>>>
                                .View.Select(Base, batchItem?.Module, batchItem?.BatchNbr).RowCast<GLTran>());
                if (!validResult.valid)
                    throw new PXException(validResult.errorMsg);
            }

            var result = baseMethod(adapter);

            // Cleanup IsReviewed
            foreach (var batchItem in list)
            {
                // 非Reverse 的GL Skip
                if (!(batchItem?.AutoReverseCopy ?? false) && string.IsNullOrEmpty(batchItem?.OrigBatchNbr))
                    continue;
                // GLTran
                foreach (var line in SelectFrom<GLTran>
                                     .Where<GLTran.module.IsEqual<P.AsString>
                                       .And<GLTran.batchNbr.IsEqual<P.AsString>>>
                                     .View.Select(Base, batchItem?.OrigModule, batchItem?.OrigBatchNbr).RowCast<GLTran>())
                {
                    // 找對應的 Origin LineNbr -> 更新IsReviewed = false
                    var lineExt = line.GetExtension<GLTranExtension>();
                    if (lineExt?.UsrPostOrigLineNbr != 0)
                    {
                        PXUpdate<Set<GLTranExtension.usrIsReviewed, Required<GLTranExtension.usrIsReviewed>,
                                 Set<GLTranExtension.usrRvBatch, Required<GLTranExtension.usrRvBatch>,
                                 Set<GLTranExtension.usrRvLineNbr, Required<GLTranExtension.usrRvLineNbr>>>>,
                                GLTran,
                                Where<GLTran.batchNbr, Equal<Required<GLTran.batchNbr>>,
                                  And<GLTran.lineNbr, Equal<Required<GLTran.lineNbr>>,
                                  And<GLTran.ledgerID, Equal<Required<GLTran.ledgerID>>>>>>
                       .Update(Base, false, null, null, lineExt?.UsrPostOrigBatchNbr, lineExt?.UsrPostOrigLineNbr, ledgerInfo_Actual?.LedgerID);
                    }
                    else
                    {
                        PXUpdate<Set<GLTranExtension.usrIsReviewed, Required<GLTranExtension.usrIsReviewed>,
                                 Set<GLTranExtension.usrRvBatch, Required<GLTranExtension.usrRvBatch>,
                                 Set<GLTranExtension.usrRvLineNbr, Required<GLTranExtension.usrRvLineNbr>>>>,
                                GLTran,
                                Where<GLTran.batchNbr, Equal<Required<GLTran.batchNbr>>,
                                  And<GLTran.ledgerID, Equal<Required<GLTran.ledgerID>>>>>
                       .Update(Base, false, null, null, lineExt?.UsrPostOrigBatchNbr, ledgerInfo_Actual?.LedgerID);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Method

        /// <summary>
        /// Valid Allowed combination
        /// </summary>
        /// <param name="batchItem"></param>
        public virtual (bool valid, string errorMsg) ValidCombination(IEnumerable<LUMAllowCombination> allowTable, IEnumerable<GLTran> list, bool checkOnlyReleased = false)
        {
            string errorMsg = string.Empty;
            var setup = this.HHSetup.Select().TopFirst;
            var valid = true;
            if (!(setup?.EnableCheckAllowedAccountCombination ?? false))
                return (valid, errorMsg);
            // GLTran
            foreach (var line in list)
            {
                if (checkOnlyReleased && !(line?.Released ?? false))
                    continue;
                var IsAllow = allowTable.FirstOrDefault(x => x.BranchID == line?.BranchID &&
                                                             x.LedgerID == line?.LedgerID &&
                                                             x.AccountID == line?.AccountID &&
                                                             x.Subid == line?.SubID) != null;
                if (!IsAllow)
                {
                    var branchInfo = Branch.PK.Find(Base, line?.BranchID);
                    var acctInfo = Account.PK.Find(Base, line?.AccountID);
                    var subInfo = Sub.PK.Find(Base, line?.SubID);
                    var ledgerInfo = Ledger.PK.Find(Base, line?.LedgerID);
                    valid = false;
                    errorMsg = $"[{branchInfo?.BranchCD?.Trim()}] + [{ledgerInfo?.LedgerCD?.Trim()}] +  [{acctInfo?.AccountCD?.Trim()}] + [{subInfo?.SubCD?.Trim()}]";
                    // 非GL 就直接SHOW一筆錯誤的組合
                    if (line?.Module == "GL")
                        Base.GLTranModuleBatNbr.Cache.RaiseExceptionHandling<GLTran.branchID>(line, line?.BranchID,
                             new PXSetPropertyException<GLTran.branchID>($"{errorMsg} is not allowed, please confirm your entry or maintain it in allowed combination.", PXErrorLevel.Error));
                    else
                        throw new PXException($"{errorMsg} is not allowed, please confirm your entry or maintain it in allowed combination.");
                }
            }
            return (valid, errorMsg);
        }

        public virtual IEnumerable<LUMAllowCombination> GetAllowTable()
            => SelectFrom<LUMAllowCombination>.View.Select(Base).RowCast<LUMAllowCombination>();
    }

    #endregion
}
