using System;
using HH_Customization.Interface;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.SO;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourItem")]
    public class LUMTourItem : PXBqlTable, IBqlTable, IAPLink, ICreateAPData
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourItem>.By<itemID, sOOrderNbr, sOOrderType>
        {
            public static LUMTourItem Find(PXGraph graph, int? itemID, string sOOrderNbr, string sOOrderType) => FindBy(graph, itemID, sOOrderNbr, sOOrderType);
        }
        public static class FK
        {
            public class Order : SOOrder.PK.ForeignKeyOf<SOLine>.By<sOOrderType, sOOrderNbr> { }
        }
        #endregion

        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
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

        #region ItemID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ItemID { get; set; }
        public abstract class itemID : PX.Data.BQL.BqlInt.Field<itemID> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Pax
        [PXDBInt()]
        [PXUIField(DisplayName = "Pax", IsReadOnly = true)]
        [PXDefault(0)]
        public virtual int? Pax { get; set; }
        public abstract class pax : PX.Data.BQL.BqlInt.Field<pax> { }
        #endregion

        #region UnitPrice
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Unit Price")]
        [PXDefault(
            typeof(Search<InventoryItemCurySettings.basePrice,
                Where<InventoryItemCurySettings.inventoryID, Equal<Current<inventoryID>>,
                    And<InventoryItemCurySettings.curyID, Equal<Current<AccessInfo.baseCuryID>>>>>)
            , PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual Decimal? UnitPrice { get; set; }
        public abstract class unitPrice : PX.Data.BQL.BqlDecimal.Field<unitPrice> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        [PXDefault(typeof(
            Search<InventoryItem.descr,
                Where<InventoryItem.inventoryID, Equal<Current<inventoryID>>>>)
            , PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region ExtCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ext Cost")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXFormula(typeof(Mult<unitPrice, pax>))]
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXUIField(DisplayName = "Inventory ID", Required = true)]
        [PXSelector(typeof(Search<InventoryItem.inventoryID>),
            typeof(InventoryItem.inventoryCD),
            typeof(InventoryItem.descr),
            SubstituteKey = typeof(InventoryItem.inventoryCD),
            DescriptionField = typeof(InventoryItem.descr)
            )]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
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
        [PXUIField(DisplayName = "Vendor")]
        //[PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
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
        [PXSelector(typeof(Search<APInvoice.refNbr, Where<APInvoice.docType, Equal<APDocType.invoice>>>))]
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

        #region Unbouhd
        public decimal? ExtCostCB { get; set; }
        #region TranDesc
        [PXString()]
        [PXUnboundDefault()]
        public virtual string TranDesc { get; set; }
        public abstract class tranDesc : PX.Data.BQL.BqlString.Field<tranDesc> { }
        #endregion

        #region MaxInt
        public class maxInt : PX.Data.BQL.BqlInt.Constant<maxInt> { public maxInt() : base(Int32.MaxValue) { } }
        #endregion

        #region Order
        [PXInt]
        [PXUnboundDefault(typeof(Switch<
                Case<Where<itemID.FromCurrent, Greater<Zero>>, itemID.FromCurrent>,
               maxInt
            >))]
        public virtual int? Seq { get; set; }
        public abstract class seq : PX.Data.BQL.BqlInt.Field<seq> { }

        [PXDateAndTime(DisplayMask = "g", InputMask = "g", UseTimeZone = true)]
        [PXUnboundDefault(typeof(AccessInfo.businessDate))]
        public virtual DateTime? SeqDate { get; set; }
        public abstract class seqDate : PX.Data.BQL.BqlDateTime.Field<seqDate> { }
        #endregion
        #endregion
    }
}