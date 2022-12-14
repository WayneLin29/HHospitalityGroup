using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;

namespace PX.Objects.IN
{
    public class INRegisterHHExt : PXCacheExtension<PX.Objects.IN.INRegister>
    {
        #region UsrReasonCode
        [PXDBString(CS.ReasonCode.reasonCodeID.Length, IsUnicode = true)]
        [PXSelector(typeof(Search<ReasonCode.reasonCodeID>))]
        [PXRestrictor(typeof(Where<ReasonCode.usage, Equal<Optional<INRegister.docType>>>), Messages.ReasonCodeDoesNotMatch)]
        [PXUIField(DisplayName = "Reason Code")]
        public virtual string UsrReasonCode { get; set; }
        public abstract class usrReasonCode : PX.Data.BQL.BqlString.Field<usrReasonCode> { }
        #endregion

        #region UsrProjectBatchNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Project Batch", Visibility = PXUIVisibility.Visible, Enabled = false)]
        //[PXSelector(typeof(Search<Batch.batchNbr, Where<Batch.module.IsEqual<BatchModule.moduleGL>>>))]
        public virtual string UsrProjectBatchNbr { get; set; }
        public abstract class usrProjectBatchNbr : PX.Data.BQL.BqlString.Field<usrProjectBatchNbr> { }
        #endregion
    }
}