using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMTouchBistroDataReceivedV")]
    public class LUMTouchBistroDataReceivedV : PXBqlTable, IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region FileID
        [PXDBGuid(IsKey = true)]
        [PXUIField(DisplayName = "FileID")]
        public virtual Guid? FileID { get; set; }
        public abstract class fileID : PX.Data.BQL.BqlGuid.Field<fileID> { }
        #endregion

        #region FileName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "File Name")]
        public virtual string FileName { get; set; }
        public abstract class fileName : PX.Data.BQL.BqlString.Field<fileName> { }
        #endregion

        #region EmailSubject
        [PXDBString(998, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Email Subject")]
        public virtual string EmailSubject { get; set; }
        public abstract class emailSubject : PX.Data.BQL.BqlString.Field<emailSubject> { }
        #endregion

        #region MailFrom
        [PXDBString(500, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Mail From")]
        public virtual string MailFrom { get; set; }
        public abstract class mailFrom : PX.Data.BQL.BqlString.Field<mailFrom> { }
        #endregion

        #region MailTo
        [PXDBString(3000, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Mail To")]
        public virtual string MailTo { get; set; }
        public abstract class mailTo : PX.Data.BQL.BqlString.Field<mailTo> { }
        #endregion

        #region CreatedAt
        [PXDBDate()]
        [PXUIField(DisplayName = "Created At")]
        public virtual DateTime? CreatedAt { get; set; }
        public abstract class createdAt : PX.Data.BQL.BqlDateTime.Field<createdAt> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Imported")]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion
    }
}