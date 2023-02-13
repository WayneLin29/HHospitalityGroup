using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedRoomAssignment")]
    public class LUMCloudBedRoomAssignment : IBqlTable
    {
        #region ReservationID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region Roomid
        [PXDBString(20,IsKey = true)]
        [PXUIField(DisplayName = "Roomid")]
        public virtual string Roomid { get; set; }
        public abstract class roomid : PX.Data.BQL.BqlString.Field<roomid> { }
        #endregion

        #region RoomType
        [PXDBString(20,IsKey = true)]
        [PXUIField(DisplayName = "Room Type")]
        public virtual string RoomType { get; set; }
        public abstract class roomType : PX.Data.BQL.BqlString.Field<roomType> { }
        #endregion

        #region RoomTypeName
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Room Type Name")]
        public virtual string RoomTypeName { get; set; }
        public abstract class roomTypeName : PX.Data.BQL.BqlString.Field<roomTypeName> { }
        #endregion

        #region Checkin
        [PXDBDate()]
        [PXUIField(DisplayName = "Checkin")]
        public virtual DateTime? Checkin { get; set; }
        public abstract class checkin : PX.Data.BQL.BqlDateTime.Field<checkin> { }
        #endregion

        #region Checkout
        [PXDBDate()]
        [PXUIField(DisplayName = "Checkout")]
        public virtual DateTime? Checkout { get; set; }
        public abstract class checkout : PX.Data.BQL.BqlDateTime.Field<checkout> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
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