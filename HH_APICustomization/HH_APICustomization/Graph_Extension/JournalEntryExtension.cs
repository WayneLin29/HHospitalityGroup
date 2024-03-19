using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
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

        public const string combinationErrorMsg = "please confirm your entry or maintain it in allowed combination.";

        #region Override Method
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            // Valid combination if Module != GL and IsReleased
            var doc = Base.BatchModule.Current;
            if (doc != null && doc?.Module != "GL" && doc?.Status == BatchStatus.Unposted)
            {
                var allowTable = GetAllowTable();
                // 除了GL以外，其他都是直接先做好release資料在產生傳票

                var valid = ValidCombination(allowTable, Base.GLTranModuleBatNbr.View.SelectMulti().RowCast<GLTran>(), true);
                if (!valid)
                    throw new PXException(combinationErrorMsg);
            }
            baseMethod();
        }
        #endregion

        #region Override Action

        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public virtual IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
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
                var valid = ValidCombination(GetAllowTable(),
                                SelectFrom<GLTran>
                                .Where<GLTran.module.IsEqual<P.AsString>
                                  .And<GLTran.batchNbr.IsEqual<P.AsString>>>
                                .View.Select(Base, batchItem?.Module, batchItem?.BatchNbr).RowCast<GLTran>());
                if (!valid)
                    throw new PXException(combinationErrorMsg);
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
                                     .View.Select(Base, batchItem?.Module, batchItem?.OrigBatchNbr).RowCast<GLTran>())
                {
                    // 找對應的 Origin LineNbr -> 更新IsReviewed = false
                    var lineExt = line.GetExtension<GLTranExtension>();
                    if (lineExt?.UsrPostOrigLineNbr != 0)
                    {
                        PXDatabase.Update<GLTran>(
                            new PXDataFieldAssign<GLTranExtension.usrIsReviewed>(false),
                            new PXDataFieldRestrict<GLTran.batchNbr>(lineExt?.UsrPostOrigBatchNbr),
                            new PXDataFieldRestrict<GLTran.lineNbr>(lineExt?.UsrPostOrigLineNbr));
                    }
                    else
                    {
                        PXDatabase.Update<GLTran>(
                           new PXDataFieldAssign<GLTranExtension.usrIsReviewed>(false),
                           new PXDataFieldRestrict<GLTran.batchNbr>(lineExt?.UsrPostOrigBatchNbr));
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
        public virtual bool ValidCombination(IEnumerable<LUMAllowCombination> allowTable, IEnumerable<GLTran> list, bool checkOnlyReleased = false)
        {
            var valid = true;
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
                    Base.GLTranModuleBatNbr.Cache.RaiseExceptionHandling<GLTran.branchID>(line, line?.BranchID,
                         new PXSetPropertyException<GLTran.branchID>($"[{branchInfo?.BranchCD?.Trim()}] + [{ledgerInfo?.LedgerCD?.Trim()}] +  [{acctInfo?.AccountCD?.Trim()}] + [{subInfo?.SubCD?.Trim()}]  is not allowed, please confirm your entry or maintain it in allowed combination.", PXErrorLevel.Error));
                    valid = false;
                    //throw new PXException($"[{branchInfo?.BranchCD?.Trim()}] + [{ledgerInfo?.LedgerCD?.Trim()}] +  [{acctInfo?.AccountCD?.Trim()}] + [{subInfo?.SubCD?.Trim()}]  is not allowed, please confirm your entry or maintain it in allowed combination.");
                }
            }
            return valid;
        }

        public virtual IEnumerable<LUMAllowCombination> GetAllowTable()
            => SelectFrom<LUMAllowCombination>.View.Select(Base).RowCast<LUMAllowCombination>();
    }

    #endregion
}
