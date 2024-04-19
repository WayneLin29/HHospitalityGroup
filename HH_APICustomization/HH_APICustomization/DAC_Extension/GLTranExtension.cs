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
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
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

        #region UsrIsReviewed
        [PXDBBool]
        [PXUIField(DisplayName = "IsReviewed", Enabled = false)]
        public virtual bool? UsrIsReviewed { get; set; }
        public abstract class usrIsReviewed : PX.Data.BQL.BqlBool.Field<usrIsReviewed> { }
        #endregion

        #region UsrRvBatch
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "RvBatch", Enabled = false)]
        public virtual string UsrRvBatch { get; set; }
        public abstract class usrRvBatch : PX.Data.BQL.BqlString.Field<usrRvBatch> { }
        #endregion

        #region UsrRvLineNbr
        [PXDBInt]
        [PXUIField(DisplayName = "RvLineNbr", Enabled = false)]
        public virtual int? UsrRvLineNbr { get; set; }
        public abstract class usrRvLineNbr : PX.Data.BQL.BqlInt.Field<usrRvLineNbr> { }
        #endregion

        #region UsrTaxZone
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Tax Zone", Enabled = true)]
        [PXSelector(typeof(PX.Objects.TX.TaxZone.taxZoneID), DescriptionField = typeof(PX.Objects.TX.TaxZone.descr), Filterable = true)]
        public virtual string UsrTaxZone { get; set; }
        public abstract class usrTaxZone : PX.Data.BQL.BqlString.Field<usrTaxZone> { }
        #endregion

        #region UsrTaxCategory
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Tax Category", Enabled = true)]
        [PXSelector(typeof(PX.Objects.TX.TaxCategory.taxCategoryID), DescriptionField = typeof(PX.Objects.TX.TaxCategory.descr))]
        public virtual string UsrTaxCategory { get; set; }
        public abstract class usrTaxCategory : PX.Data.BQL.BqlString.Field<usrTaxCategory> { }
        #endregion

        #region UsrPostOrigBatchNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Post OrigBatchNbr", Enabled = false)]
        public virtual string UsrPostOrigBatchNbr { get; set; }
        public abstract class usrPostOrigBatchNbr : PX.Data.BQL.BqlString.Field<usrPostOrigBatchNbr> { }
        #endregion

        #region UsrPostOrigLineNbr
        [PXDBInt]
        [PXUIField(DisplayName = "Post OrigLineNbr", Enabled = false)]
        public virtual int? UsrPostOrigLineNbr { get; set; }
        public abstract class usrPostOrigLineNbr : PX.Data.BQL.BqlInt.Field<usrPostOrigLineNbr> { }
        #endregion

    }
}
