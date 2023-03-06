using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.SO;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourFlight")]
    public class LUMTourFlight : IBqlTable
    {
        #region Const
        public const string FLIGHT = "FLIGHT";
        public class flight : PX.Data.BQL.BqlString.Constant<flight> { public flight() : base(FLIGHT) { } }
        #endregion

        #region Key
        public class PK : PrimaryKeyOf<LUMTourFlight>.By<fligthID, sOOrderNbr, sOOrderType>
        {
            public static LUMTourFlight Find(PXGraph graph, int? fligthID, string sOOrderNbr, string sOOrderType) => FindBy(graph, fligthID, sOOrderNbr, sOOrderType);
        }
        public static class FK
        {
            public class Order : SOOrder.PK.ForeignKeyOf<SOLine>.By<sOOrderType, sOOrderNbr> { }
        }
        #endregion

        #region SOOrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Nbr")]
        [PXDBDefault(typeof(SOOrder.orderNbr), DefaultForUpdate = false)]
        [PXParent(typeof(FK.Order))]
        public virtual string SOOrderNbr { get; set; }
        public abstract class sOOrderNbr : PX.Data.BQL.BqlString.Field<sOOrderNbr> { }
        #endregion

        #region SOOrderType
        [PXDBString(5, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Type")]
        [PXDefault(typeof(SOOrder.orderType))]

        public virtual string SOOrderType { get; set; }
        public abstract class sOOrderType : PX.Data.BQL.BqlString.Field<sOOrderType> { }
        #endregion

        #region FligthID
        [PXDBIdentity(IsKey = true)]
        public virtual int? FligthID { get; set; }
        public abstract class fligthID : PX.Data.BQL.BqlInt.Field<fligthID> { }
        #endregion

        #region AirLine
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Air Line", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string AirLine { get; set; }
        public abstract class airLine : PX.Data.BQL.BqlString.Field<airLine> { }
        #endregion

        #region FligthNumber
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Fligth Number", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string FligthNumber { get; set; }
        public abstract class fligthNumber : PX.Data.BQL.BqlString.Field<fligthNumber> { }
        #endregion

        #region BookCode
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Book Code", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string BookCode { get; set; }
        public abstract class bookCode : PX.Data.BQL.BqlString.Field<bookCode> { }
        #endregion

        #region DepartureAirport
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Departure Airport", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string DepartureAirport { get; set; }
        public abstract class departureAirport : PX.Data.BQL.BqlString.Field<departureAirport> { }
        #endregion

        #region DeptDateTime
        [PXDBDateAndTime(DisplayMask = "g", InputMask = "g", PreserveTime = true, UseTimeZone = true)]
        [PXUIField(DisplayName = "Dept Date Time", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual DateTime? DeptDateTime { get; set; }
        public abstract class deptDateTime : PX.Data.BQL.BqlDateTime.Field<deptDateTime> { }
        #endregion

        #region ArrivalAirport
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Arrival Airport", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string ArrivalAirport { get; set; }
        public abstract class arrivalAirport : PX.Data.BQL.BqlString.Field<arrivalAirport> { }
        #endregion

        #region ArrivalDateTime
        [PXDBDateAndTime(DisplayMask = "g", InputMask = "g", PreserveTime = true, UseTimeZone = true)]
        [PXUIField(DisplayName = "Arrival Date Time", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual DateTime? ArrivalDateTime { get; set; }
        public abstract class arrivalDateTime : PX.Data.BQL.BqlDateTime.Field<arrivalDateTime> { }
        #endregion

        #region ExtCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ext Cost")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
        #endregion

        #region Pax
        [PXDBInt()]
        [PXUIField(DisplayName = "Pax", IsReadOnly = true)]
        [PXDefault(0)]
        public virtual int? Pax { get; set; }
        public abstract class pax : PX.Data.BQL.BqlInt.Field<pax> { }
        #endregion

        #region Remark
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Remark")]
        public virtual string Remark { get; set; }
        public abstract class remark : PX.Data.BQL.BqlString.Field<remark> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXUIField(DisplayName = "Inventory ID",Required = true)]
        [PXDefault(typeof(Search<InventoryItem.inventoryID, Where<InventoryItem.inventoryCD, Equal<flight>>>)
            , PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<InventoryItem.inventoryID>),
            typeof(InventoryItem.inventoryCD),
            typeof(InventoryItem.descr),
            SubstituteKey = typeof(InventoryItem.inventoryCD),
            DescriptionField = typeof(InventoryItem.descr)
            )]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region AccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "Account", Required = true)]
        [PXDefault(
            typeof(Search<InventoryItem.cOGSAcctID,
                Where<InventoryItem.inventoryID, Equal<Current<inventoryID>>>>)
            , PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<Account.accountID, Where<Account.active, Equal<True>>>),
                typeof(Account.accountCD),
                typeof(Account.description),
                SubstituteKey = typeof(Account.accountCD),
                DescriptionField = typeof(Account.description)
            )]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubID
        [PXDBInt()]
        [PXUIField(DisplayName = "Sub Account", Required = true)]
        [PXDefault(
            typeof(Search<InventoryItem.cOGSSubID,
                Where<InventoryItem.inventoryID, Equal<Current<inventoryID>>>>)
            , PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<Sub.subID, Where<Sub.active, Equal<True>>>),
                typeof(Sub.subCD),
                typeof(Sub.description),
                SubstituteKey = typeof(Sub.subCD),
                DescriptionField = typeof(Sub.description)
            )]
        public virtual int? SubID { get; set; }
        public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }
        #endregion

        #region VendorID
        [PXDBInt()]
        [PXUIField(DisplayName = "Vendor", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<Vendor.bAccountID>),
                typeof(Vendor.acctCD),
                typeof(Vendor.acctName),
                SubstituteKey = typeof(Vendor.acctCD),
                DescriptionField = typeof(Vendor.acctName)
            )]
        public virtual int? VendorID { get; set; }
        public abstract class vendorID : PX.Data.BQL.BqlInt.Field<vendorID> { }
        #endregion

        #region APRefNbr
        [PXDBString(15)]
        [PXUIField(DisplayName = "AP Bill", IsReadOnly = true)]
        public virtual string APRefNbr { get; set; }
        public abstract class aPRefNbr : PX.Data.BQL.BqlString.Field<aPRefNbr> { }
        #endregion

        #region APDocType
        [PXDBString(5)]
        [PXUIField(DisplayName = "AP Doc Type", IsReadOnly = true)]
        public virtual string APDocType { get; set; }
        public abstract class aPDocType : PX.Data.BQL.BqlString.Field<aPDocType> { }
        #endregion

        #region APLineNbr
        [PXDBInt()]
        [PXUIField(DisplayName = "Line Nbr", IsReadOnly = true)]
        public virtual int? APLineNbr { get; set; }
        public abstract class aPLineNbr : PX.Data.BQL.BqlInt.Field<aPLineNbr> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
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

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}