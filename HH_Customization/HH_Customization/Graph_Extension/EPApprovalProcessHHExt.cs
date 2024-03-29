﻿using PX.Data;
using PX.Objects.GL;
using System.Linq;
using EPOwned = PX.Objects.EP.EPApprovalProcess.EPOwned;

namespace PX.Objects.EP
{
    public class EPApprovalProcessHHExt : PXGraphExtension<EPApprovalProcess>
    {

        #region Event
        protected virtual void _(Events.FieldDefaulting<EPOwned, EPOwnedHHExt.branchID> e)
        {
            if (e.Row == null) return;
            EntityHelper helper = new EntityHelper(Base);
            //取得對應的簽核資料
            var refRow = helper.GetEntityRow(e.Row.RefNoteID);
            //因為Eva導入請假簽核，原廠抓不到客製資料
            if (refRow != null) { 
                //取得對應資料之Cache
                PXCache cache = Base.Caches[refRow.GetType()];
                //取得BranchID
                e.NewValue = cache.GetValue(refRow, "BranchID");
            }
        }
        #endregion

        #region Table
        public class EPOwnedHHExt : PXCacheExtension<EPOwned>
        {
            #region BranchID
            [PXInt]
            [PXUIField(DisplayName = "Branch")]
            [PXSelector(typeof(Branch.branchID),
                typeof(Branch.branchCD),
                typeof(Branch.acctName),
                SubstituteKey = typeof(Branch.branchCD),
                DescriptionField = typeof(Branch.acctName)
                )]
            [PXUnboundDefault]
            public virtual int? BranchID { get; set; }
            public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
            #endregion
        }
        #endregion
    }
}
