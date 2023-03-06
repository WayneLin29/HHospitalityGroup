using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.SO;
using PX.Objects.IN;
using PX.Objects.GL;
using static PX.Objects.SO.SOOrderEntryHHExt;
using PX.Objects.AP;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourReservation")]
    public class LUMTourReservation : IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourReservation>.By<tourReservationID, sOOrderNbr, sOOrderType>
        {
            public static LUMTourReservation Find(PXGraph graph, int? tourReservationID, string sOOrderNbr, string sOOrderType) => FindBy(graph, tourReservationID, sOOrderNbr, sOOrderType);
        }
        public static class FK
        {
            public class Order : SOOrder.PK.ForeignKeyOf<SOLine>.By<sOOrderType, sOOrderNbr> { }
        }
        #endregion

        #region TourReservationID
        [PXDBIdentity(IsKey = true)]
        public virtual int? TourReservationID { get; set; }
        public abstract class tourReservationID : PX.Data.BQL.BqlInt.Field<tourReservationID> { }
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

        #region ReservationID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reservation ID",Required = true)]
        [PXDefault(PersistingCheck =PXPersistingCheck.NullOrBlank)]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXUIField(DisplayName = "Inventory ID",Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
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

        #region Remark
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Remark")]
        public virtual string Remark { get; set; }
        public abstract class remark : PX.Data.BQL.BqlString.Field<remark> { }
        #endregion

        #region ExtCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ext Cost")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
        #endregion

        #region APRefNbr
        [PXDBString(15)]
        [PXUIField(DisplayName = "AP Bill")]
        public virtual string APRefNbr { get; set; }
        public abstract class aPRefNbr : PX.Data.BQL.BqlString.Field<aPRefNbr> { }
        #endregion

        #region APDocType
        [PXDBString(5)]
        [PXUIField(DisplayName = "AP Doc Type")]
        public virtual string APDocType { get; set; }
        public abstract class aPDocType : PX.Data.BQL.BqlString.Field<aPDocType> { }
        #endregion

        #region APLineNbr
        [PXDBInt()]
        [PXUIField(DisplayName = "Line Nbr")]
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

        #region unbound
        #region ExtCost(CB)
        [PXDecimal()]
        [PXUIField(DisplayName = "Ext Cost(CB)",IsReadOnly = true)]
        [PXUnboundDefault(typeof(Search4<LUMTourRoomReservations.extCost
            , Where<LUMTourRoomReservations.reservationID, Equal<Current<reservationID>>>
            , Aggregate<Sum<LUMTourRoomReservations.extCost>>>))]
        public virtual Decimal? ExtCostCB { get; set; }
        public abstract class extCostCB : PX.Data.BQL.BqlDecimal.Field<extCostCB> { }
        #endregion
        #endregion
    }
}