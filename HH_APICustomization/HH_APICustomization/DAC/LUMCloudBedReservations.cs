using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedReservations")]
    public class LUMCloudBedReservations : IBqlTable
    {
        public class PK : PrimaryKeyOf<LUMCloudBedReservations>.By<propertyID, reservationID>
        {
            public static LUMCloudBedReservations Find(PXGraph graph, string propertyID, string reservationID) => FindBy(graph, propertyID, reservationID);
        }

        #region PropertyID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string PropertyID { get; set; }
        public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region ReservationID
        [PXDBString(100, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region DateCreated
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Date Created")]
        public virtual DateTime? DateCreated { get; set; }
        public abstract class dateCreated : PX.Data.BQL.BqlDateTime.Field<dateCreated> { }
        #endregion

        #region DateModified
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Date Modified")]
        public virtual DateTime? DateModified { get; set; }
        public abstract class dateModified : PX.Data.BQL.BqlDateTime.Field<dateModified> { }
        #endregion

        #region Source
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Source")]
        public virtual string Source { get; set; }
        public abstract class source : PX.Data.BQL.BqlString.Field<source> { }
        #endregion

        #region ThirdPartyIdentifier
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Third Party Identifier")]
        public virtual string ThirdPartyIdentifier { get; set; }
        public abstract class thirdPartyIdentifier : PX.Data.BQL.BqlString.Field<thirdPartyIdentifier> { }
        #endregion

        #region Status
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Status")]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region StartDate
        [PXDBDate(UseTimeZone = false)]
        [PXUIField(DisplayName = "Start Date")]
        public virtual DateTime? StartDate { get; set; }
        public abstract class startDate : PX.Data.BQL.BqlDateTime.Field<startDate> { }
        #endregion

        #region EndDate
        [PXDBDate(UseTimeZone = false)]
        [PXUIField(DisplayName = "End Date")]
        public virtual DateTime? EndDate { get; set; }
        public abstract class endDate : PX.Data.BQL.BqlDateTime.Field<endDate> { }
        #endregion

        #region Total
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Total")]
        public virtual Decimal? Total { get; set; }
        public abstract class total : PX.Data.BQL.BqlDecimal.Field<total> { }
        #endregion

        #region Balance
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Balance")]
        public virtual Decimal? Balance { get; set; }
        public abstract class balance : PX.Data.BQL.BqlDecimal.Field<balance> { }
        #endregion

        #region SuggestedDeposit
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Suggested Deposit")]
        public virtual Decimal? SuggestedDeposit { get; set; }
        public abstract class suggestedDeposit : PX.Data.BQL.BqlDecimal.Field<suggestedDeposit> { }
        #endregion

        #region SubTotal
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Sub Total")]
        public virtual Decimal? SubTotal { get; set; }
        public abstract class subTotal : PX.Data.BQL.BqlDecimal.Field<subTotal> { }
        #endregion

        #region AdditioonalItems
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Additioonal Items")]
        public virtual Decimal? AdditioonalItems { get; set; }
        public abstract class additioonalItems : PX.Data.BQL.BqlDecimal.Field<additioonalItems> { }
        #endregion

        #region TaxesFees
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Taxes Fees")]
        public virtual Decimal? TaxesFees { get; set; }
        public abstract class taxesFees : PX.Data.BQL.BqlDecimal.Field<taxesFees> { }
        #endregion

        #region GrandTotal
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Grand Total")]
        public virtual Decimal? GrandTotal { get; set; }
        public abstract class grandTotal : PX.Data.BQL.BqlDecimal.Field<grandTotal> { }
        #endregion

        #region Paid
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Paid")]
        public virtual Decimal? Paid { get; set; }
        public abstract class paid : PX.Data.BQL.BqlDecimal.Field<paid> { }
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

        #region GuestName
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = "Guest Name")]
        public virtual string GuestName { get; set; }
        public abstract class guestName : PX.Data.BQL.BqlString.Field<guestName> { }
        #endregion
    }
}