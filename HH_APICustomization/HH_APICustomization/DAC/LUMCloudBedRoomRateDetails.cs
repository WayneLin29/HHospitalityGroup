using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedRoomRateDetails")]
    public class LUMCloudBedRoomRateDetails : IBqlTable
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

        #region RateDate
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Rate Date")]
        public virtual DateTime? RateDate { get; set; }
        public abstract class rateDate : PX.Data.BQL.BqlDateTime.Field<rateDate> { }
        #endregion

        #region Rate
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Rate")]
        public virtual Decimal? Rate { get; set; }
        public abstract class rate : PX.Data.BQL.BqlDecimal.Field<rate> { }
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