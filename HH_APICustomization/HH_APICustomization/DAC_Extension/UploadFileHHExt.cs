using PX.Data;

namespace PX.SM
{
    public class UploadFileHHExt:PXCacheExtension<UploadFile>
    {
        #region UsrIsImported
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Imported")]
        public virtual bool? UsrIsImported { get; set; }
        public abstract class usrIsImported : PX.Data.BQL.BqlBool.Field<usrIsImported> { }
        #endregion
    }
}
