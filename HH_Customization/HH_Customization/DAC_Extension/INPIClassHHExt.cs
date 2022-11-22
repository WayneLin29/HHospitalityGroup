using PX.Data;
using PX.Objects.CS;

namespace PX.Objects.IN
{
    public class INPIClassHHExt :PXCacheExtension<INPIClass>
    {
        #region UsrReasonCode
        [PXDBString(CS.ReasonCode.reasonCodeID.Length, IsUnicode = true)]
        [PXSelector(typeof(Search<ReasonCode.reasonCodeID, Where<ReasonCode.usage, Equal<ReasonCodeUsages.adjustment>>>), DescriptionField = typeof(ReasonCode.descr))]
        [PXUIField(DisplayName = "Reason Code")]
        public virtual string UsrReasonCode { get; set; }
        public abstract class usrReasonCode : PX.Data.BQL.BqlString.Field<usrReasonCode> { }
        #endregion
    }
}
