using PX.Data;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.PM;

namespace PX.Objects.IN
{
    public class INReleaseProcessHHExt : PXGraphExtension<INReleaseProcess>
    {

        #region Override
        public delegate void PersistDelegate();
        [PXOverride]
        public virtual void Persist(PersistDelegate baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                INRegister item = Base.inregister.Current;
                item.GetExtension<INRegisterHHExt>().UsrProjectBatchNbr = CreateBatch(item);
                Base.inregister.UpdateCurrent();
                baseMethod();
                ts.Complete();
            }

        }
        #endregion

        #region Method
        public virtual string CreateBatch(INRegister item)
        {
            JournalEntry entry = PXGraph.CreateInstance<JournalEntry>();
            #region Batch
            Batch batch = (Batch)entry.BatchModule.Cache.CreateInstance();
            batch = entry.BatchModule.Insert(batch);
            batch.Module = BatchModule.GL;
            batch.DateEntered = item.TranDate;
            Ledger ledger = Ledger.UK.Find(Base, "ACTUAL");
            batch.LedgerID = ledger.LedgerID;
            batch.BranchID = item.BranchID;
            batch.Description = item.TranDesc;
            batch.CuryID = "PHP";
            batch = entry.BatchModule.Update(batch);
            #endregion

            #region GLTran
            foreach (INTran tran in GetTran(item.RefNbr))
            {
                if (ProjectDefaultAttribute.IsNonProject(tran.ProjectID)) continue;
                #region Tran A
                GLTran tranA = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                tranA = entry.GLTranModuleBatNbr.Insert(tranA);
                ReasonCode rc = ReasonCode.PK.Find(Base, tran.ReasonCode);
                tranA.AccountID = rc.AccountID;
                tranA.SubID = rc.SubID;
                tranA.Qty = tran.Qty;
                tranA.UOM = tran.UOM;
                tranA.InventoryID = tran.InventoryID;
                tranA.TranDesc = tran.TranDesc;
                tranA.RefNbr = tran.RefNbr;
                tranA.TranLineNbr = tran.LineNbr;
                tranA.ProjectID = tran.ProjectID;
                tranA.TaskID = tran.TaskID;
                tranA.CuryDebitAmt = tran.TranCost;
                tranA.CuryCreditAmt = 0m;
                tranA = entry.GLTranModuleBatNbr.Update(tranA);
                #endregion

                #region Tran B
                GLTran tranB = (GLTran)entry.GLTranModuleBatNbr.Cache.CreateInstance();
                tranB.AccountID = tranA.AccountID;
                tranB.SubID = tranA.SubID;
                tranB.Qty = tranA.Qty;
                tranB.UOM = tranA.UOM;
                tranB.InventoryID = tranA.InventoryID;
                tranB.TranDesc = tranA.TranDesc;
                tranB.RefNbr = tranA.RefNbr;
                tranB.TranLineNbr = tranA.LineNbr;
                tranB.ProjectID = ProjectDefaultAttribute.NonProject();
                //tranB.TaskID = null;
                tranB.CuryDebitAmt = tranA.CuryCreditAmt;
                tranB.CuryCreditAmt = tranA.CuryDebitAmt;
                tranB = entry.GLTranModuleBatNbr.Update(tranB);
                #endregion
            }

            #endregion
            entry.releaseFromHold.Press();
            entry.Save.Press();
            entry.release.Press();
            return batch.BatchNbr;
        }
        #endregion

        #region BQL
        protected virtual PXResultset<INTran> GetTran(string refNbr)
        {
            return PXSelect<INTran,
                 Where<INTran.docType, Equal<INDocType.issue>,
                     And<INTran.refNbr, Equal<Required<INTran.refNbr>>>>>
                     .Select(Base, refNbr);
        }
        #endregion
    }
}
