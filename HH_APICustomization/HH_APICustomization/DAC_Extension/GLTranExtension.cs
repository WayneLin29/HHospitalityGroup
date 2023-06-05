using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.GL;

namespace PX.Objects.GL
{
    public class GLTranExtension : PXCacheExtension<GLTran>
    {
        [PXDBBool]
        [PXDefault(false,PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Reconciled", Enabled = false)]
        public virtual bool? UsrReconciled { get; set; }
        public abstract class usrReconciled : PX.Data.BQL.BqlBool.Field<usrReconciled> { }

        [PXDBDate(UseTimeZone = false)]
        [PXUIField(DisplayName = "Reconciled Date", Enabled = false)]
        public virtual DateTime? UsrReconciledDate { get; set; }
        public abstract class usrReconciledDate : PX.Data.BQL.BqlDateTime.Field<usrReconciledDate> { }

        [PXDBString(15)]
        [PXUIField(DisplayName = "Reconciled Batch", Enabled = false)]
        public virtual string UsrReconciledBatch { get; set; }
        public abstract class usrReconciledBatch : PX.Data.BQL.BqlString.Field<usrReconciledBatch> { }

        [PXDBGuid]
        [PXUIField(DisplayName = "Reconciled By", Enabled = false)]
        public virtual Guid? UsrReconciledBy { get; set; }
        public abstract class usrReconciledBy : PX.Data.BQL.BqlGuid.Field<usrReconciledBy> { }
    }
}
