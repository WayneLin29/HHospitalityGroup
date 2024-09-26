using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMRemitReservation")]
    public class LUMRemitReservation : PXBqlTable, IBqlTable
    {

        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ref Nbr", Enabled = false)]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region ReservationID
        [PXDBString(100, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reservation ID", Enabled = false)]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region CheckMessage
        [PXDBString(512, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Check Message", Enabled = false)]
        public virtual string CheckMessage { get; set; }
        public abstract class checkMessage : PX.Data.BQL.BqlString.Field<checkMessage> { }
        #endregion

        #region Status
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region GuestName
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Guest Name", Enabled = false)]
        public virtual string GuestName { get; set; }
        public abstract class guestName : PX.Data.BQL.BqlString.Field<guestName> { }
        #endregion

        #region CheckIn
        [PXDBDate()]
        [PXUIField(DisplayName = "Check In", Enabled = false)]
        public virtual DateTime? CheckIn { get; set; }
        public abstract class checkIn : PX.Data.BQL.BqlDateTime.Field<checkIn> { }
        #endregion

        #region CheckOut
        [PXDBDate()]
        [PXUIField(DisplayName = "Check Out", Enabled = false)]
        public virtual DateTime? CheckOut { get; set; }
        public abstract class checkOut : PX.Data.BQL.BqlDateTime.Field<checkOut> { }
        #endregion

        #region Balance
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Balance", Enabled = false)]
        public virtual Decimal? Balance { get; set; }
        public abstract class balance : PX.Data.BQL.BqlDecimal.Field<balance> { }
        #endregion

        #region Total
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Total", Enabled = false)]
        public virtual Decimal? Total { get; set; }
        public abstract class total : PX.Data.BQL.BqlDecimal.Field<total> { }
        #endregion

        #region PendingCount
        [PXDBInt()]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Toggle Out", Enabled = false)]
        public virtual int? PendingCount { get; set; }
        public abstract class pendingCount : PX.Data.BQL.BqlInt.Field<pendingCount> { }
        #endregion

        #region RoomRevenue
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Room Revenue", Enabled = false)]
        public virtual Decimal? RoomRevenue { get; set; }
        public abstract class roomRevenue : PX.Data.BQL.BqlDecimal.Field<roomRevenue> { }
        #endregion

        #region Taxes
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Taxes", Enabled = false)]
        public virtual Decimal? Taxes { get; set; }
        public abstract class taxes : PX.Data.BQL.BqlDecimal.Field<taxes> { }
        #endregion

        #region Fees
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Fees", Enabled = false)]
        public virtual Decimal? Fees { get; set; }
        public abstract class fees : PX.Data.BQL.BqlDecimal.Field<fees> { }
        #endregion

        #region Others
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Others", Enabled = false)]
        public virtual Decimal? Others { get; set; }
        public abstract class others : PX.Data.BQL.BqlDecimal.Field<others> { }
        #endregion

        #region Payment
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Payment", Enabled = false)]
        public virtual Decimal? Payment { get; set; }
        public abstract class payment : PX.Data.BQL.BqlDecimal.Field<payment> { }
        #endregion

        #region IsOutOfScope
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Is Out Of Scope", Enabled = false)]
        public virtual bool? IsOutOfScope { get; set; }
        public abstract class isOutOfScope : PX.Data.BQL.BqlBool.Field<isOutOfScope> { }
        #endregion

        #region OPRemark
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "OPRemark")]
        public virtual string OPRemark { get; set; }
        public abstract class oPRemark : PX.Data.BQL.BqlString.Field<oPRemark> { }
        #endregion

        #region ADRemark
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "ADRemark")]
        public virtual string ADRemark { get; set; }
        public abstract class aDRemark : PX.Data.BQL.BqlString.Field<aDRemark> { }
        #endregion

        #region ToRemitCount
        [PXDBInt()]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "To Remit", Enabled = false)]
        public virtual int? ToRemitCount { get; set; }
        public abstract class toRemitCount : PX.Data.BQL.BqlInt.Field<toRemitCount> { }
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