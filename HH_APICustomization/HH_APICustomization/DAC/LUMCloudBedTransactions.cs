using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedTransactions")]
    public class LUMCloudBedTransactions : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXUIField(DisplayName = "Imported")]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region BatchNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Batch Nbr")]
        public virtual string BatchNbr { get; set; }
        public abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }
        #endregion

        #region LineNbr
        [PXDBInt()]
        [PXUIField(DisplayName = "Line Nbr")]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region PropertyID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string PropertyID { get; set; }
        public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region ReservationID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region SubReservationID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Sub Reservation ID")]
        public virtual string SubReservationID { get; set; }
        public abstract class subReservationID : PX.Data.BQL.BqlString.Field<subReservationID> { }
        #endregion

        #region HouseAccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "House Account ID")]
        public virtual int? HouseAccountID { get; set; }
        public abstract class houseAccountID : PX.Data.BQL.BqlInt.Field<houseAccountID> { }
        #endregion

        #region HouseAccountName
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "House Account Name")]
        public virtual string HouseAccountName { get; set; }
        public abstract class houseAccountName : PX.Data.BQL.BqlString.Field<houseAccountName> { }
        #endregion

        #region GuestID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Guest ID")]
        public virtual string GuestID { get; set; }
        public abstract class guestID : PX.Data.BQL.BqlString.Field<guestID> { }
        #endregion

        #region PropertyName
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property Name")]
        public virtual string PropertyName { get; set; }
        public abstract class propertyName : PX.Data.BQL.BqlString.Field<propertyName> { }
        #endregion

        #region TransactionDateTime
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Transaction Date Time")]
        public virtual DateTime? TransactionDateTime { get; set; }
        public abstract class transactionDateTime : PX.Data.BQL.BqlDateTime.Field<transactionDateTime> { }
        #endregion

        #region TransactionDateTimeUTC
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Transaction Date Time UTC")]
        public virtual DateTime? TransactionDateTimeUTC { get; set; }
        public abstract class transactionDateTimeUTC : PX.Data.BQL.BqlDateTime.Field<transactionDateTimeUTC> { }
        #endregion

        #region TransactionLastModifiedDateTime
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Transaction Last Modified Date Time")]
        public virtual DateTime? TransactionLastModifiedDateTime { get; set; }
        public abstract class transactionLastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<transactionLastModifiedDateTime> { }
        #endregion

        #region TransactionLastModifiedDateTimeUTC
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Transaction Last Modified Date Time UTC")]
        public virtual DateTime? TransactionLastModifiedDateTimeUTC { get; set; }
        public abstract class transactionLastModifiedDateTimeUTC : PX.Data.BQL.BqlDateTime.Field<transactionLastModifiedDateTimeUTC> { }
        #endregion

        #region GuestCheckIn
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Guest Check In")]
        public virtual DateTime? GuestCheckIn { get; set; }
        public abstract class guestCheckIn : PX.Data.BQL.BqlDateTime.Field<guestCheckIn> { }
        #endregion

        #region GuestCheckOut
        [PXDBDateAndTime(UseTimeZone = false)]
        [PXUIField(DisplayName = "Guest Check Out")]
        public virtual DateTime? GuestCheckOut { get; set; }
        public abstract class guestCheckOut : PX.Data.BQL.BqlDateTime.Field<guestCheckOut> { }
        #endregion

        #region RoomTypeID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Room Type ID")]
        public virtual string RoomTypeID { get; set; }
        public abstract class roomTypeID : PX.Data.BQL.BqlString.Field<roomTypeID> { }
        #endregion

        #region RoomTypeName
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Room Type Name")]
        public virtual string RoomTypeName { get; set; }
        public abstract class roomTypeName : PX.Data.BQL.BqlString.Field<roomTypeName> { }
        #endregion

        #region RoomName
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Room Name")]
        public virtual string RoomName { get; set; }
        public abstract class roomName : PX.Data.BQL.BqlString.Field<roomName> { }
        #endregion

        #region GuestName
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Guest Name")]
        public virtual string GuestName { get; set; }
        public abstract class guestName : PX.Data.BQL.BqlString.Field<guestName> { }
        #endregion

        #region Description
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Category
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Category")]
        public virtual string Category { get; set; }
        public abstract class category : PX.Data.BQL.BqlString.Field<category> { }
        #endregion

        #region TransactionCode
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Code")]
        public virtual string TransactionCode { get; set; }
        public abstract class transactionCode : PX.Data.BQL.BqlString.Field<transactionCode> { }
        #endregion

        #region TransactionNotes
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Notes")]
        public virtual string TransactionNotes { get; set; }
        public abstract class transactionNotes : PX.Data.BQL.BqlString.Field<transactionNotes> { }
        #endregion

        #region Quantity
        [PXDBInt()]
        [PXUIField(DisplayName = "Quantity")]
        public virtual int? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlInt.Field<quantity> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region Currency
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Currency")]
        public virtual string Currency { get; set; }
        public abstract class currency : PX.Data.BQL.BqlString.Field<currency> { }
        #endregion

        #region UserName
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "User Name")]
        public virtual string UserName { get; set; }
        public abstract class userName : PX.Data.BQL.BqlString.Field<userName> { }
        #endregion

        #region TransactionType
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region TransactionCategory
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Category")]
        public virtual string TransactionCategory { get; set; }
        public abstract class transactionCategory : PX.Data.BQL.BqlString.Field<transactionCategory> { }
        #endregion

        #region ItemCategoryName
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Item Category Name")]
        public virtual string ItemCategoryName { get; set; }
        public abstract class itemCategoryName : PX.Data.BQL.BqlString.Field<itemCategoryName> { }
        #endregion

        #region TransactionID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction ID")]
        public virtual string TransactionID { get; set; }
        public abstract class transactionID : PX.Data.BQL.BqlString.Field<transactionID> { }
        #endregion

        #region ParentTransactionID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Parent Transaction ID")]
        public virtual string ParentTransactionID { get; set; }
        public abstract class parentTransactionID : PX.Data.BQL.BqlString.Field<parentTransactionID> { }
        #endregion

        #region CardType
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Card Type")]
        public virtual string CardType { get; set; }
        public abstract class cardType : PX.Data.BQL.BqlString.Field<cardType> { }
        #endregion

        #region IsDeleted
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Deleted")]
        public virtual bool? IsDeleted { get; set; }
        public abstract class isDeleted : PX.Data.BQL.BqlBool.Field<isDeleted> { }
        #endregion

        #region ErrorMessage
        [PXDBString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "Error Message")]
        public virtual string ErrorMessage { get; set; }
        public abstract class errorMessage : PX.Data.BQL.BqlString.Field<errorMessage> { }
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