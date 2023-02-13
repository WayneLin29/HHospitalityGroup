using System;
using HH_APICustomization.DAC;
using PX.Data;

namespace HHAPICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMContactReview")]
    public class LUMContactReview : IBqlTable
    {
        #region ContactID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Contact ID")]
        [PXDefault(typeof(PX.Objects.CR.Contact.contactID))]
        public virtual int? ContactID { get; set; }
        public abstract class contactID : PX.Data.BQL.BqlInt.Field<contactID> { }
        #endregion

        #region ReviewDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Review Date")]
        public virtual DateTime? ReviewDate { get; set; }
        public abstract class reviewDate : PX.Data.BQL.BqlDateTime.Field<reviewDate> { }
        #endregion

        #region ReservationID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXSelector(typeof(Search<LUMCloudBedReservations.reservationID>),
            typeof(LUMCloudBedReservations.propertyID),
            typeof(LUMCloudBedReservations.source),
            typeof(LUMCloudBedReservations.startDate),
            typeof(LUMCloudBedReservations.endDate),
            typeof(LUMCloudBedReservations.status),
            typeof(LUMCloudBedReservations.balance))]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region RoomID
        [PXDBString(20, IsUnicode = true,InputMask = "")]
        [PXUIField(DisplayName = "RoomID")]
        [PXSelector(typeof(Search<LUMCloudBedRoomAssignment.roomid,
                            Where<LUMCloudBedRoomAssignment.reservationID, Equal<Current<LUMContactReview.reservationID>>>>),
                    typeof(LUMCloudBedRoomAssignment.checkin),
                    typeof(LUMCloudBedRoomAssignment.checkout))]
        public virtual string RoomID { get; set; }
        public abstract class roomID : PX.Data.BQL.BqlString.Field<roomID> { }
        #endregion

        #region ReviewText
        [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Review Text")]
        public virtual string ReviewText { get; set; }
        public abstract class reviewText : PX.Data.BQL.BqlString.Field<reviewText> { }
        #endregion

        #region Score
        [PXDBDecimal]
        [PXUIField(DisplayName = "Score")]
        public virtual decimal? Score { get; set; }
        public abstract class score : PX.Data.BQL.BqlDecimal.Field<score> { }
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