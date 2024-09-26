using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMRemitBlock")]
    public class LUMRemitBlock : PXBqlTable, IBqlTable
    {
        #region UNBOUND

        #region RoomName
        [PXString]
        [PXUIField(DisplayName = "Room Name")]
        public virtual string RoomName { get; set; }
        public abstract class roomName : PX.Data.BQL.BqlString.Field<roomName> { }
        #endregion

        #endregion

        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ref Nbr", Enabled = false)]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region BlockID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Block ID", Enabled = false)]
        public virtual string BlockID { get; set; }
        public abstract class blockID : PX.Data.BQL.BqlString.Field<blockID> { }
        #endregion

        #region CheckMessage
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Check Message", Enabled = false)]
        public virtual string CheckMessage { get; set; }
        public abstract class checkMessage : PX.Data.BQL.BqlString.Field<checkMessage> { }
        #endregion

        #region OPRemark
        [PXDBString(512, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "OPRemark")]
        public virtual string OPRemark { get; set; }
        public abstract class oPRemark : PX.Data.BQL.BqlString.Field<oPRemark> { }
        #endregion

        #region ADRemark
        [PXDBString(512, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "ADRemark")]
        public virtual string ADRemark { get; set; }
        public abstract class aDRemark : PX.Data.BQL.BqlString.Field<aDRemark> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        [PXUIField(Enabled = false)]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        [PXUIField(Enabled = false)]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        [PXUIField(Enabled = false)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}