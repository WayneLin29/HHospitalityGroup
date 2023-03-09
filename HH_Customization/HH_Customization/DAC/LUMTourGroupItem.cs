using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;
using PX.Objects.GL;
using PX.Objects.CM.Extensions;
using PX.Objects.AP;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourGroupItem")]
    public class LUMTourGroupItem : IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourGroupItem>.By<tourGroupNbr, tourGroupItemID>
        {
            public static LUMTourGroupItem Find(PXGraph graph, string tourGroupNbr, int? tourGroupItemID) => FindBy(graph, tourGroupNbr, tourGroupItemID);
        }
        #endregion

        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region TourGroupNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tour Group Nbr")]
        [PXDBDefault(typeof(LUMTourGroup.tourGroupNbr))]
        [PXParent(typeof(Select<LUMTourGroup, Where<LUMTourGroup.tourGroupNbr, Equal<Current<tourGroupNbr>>>>))]
        public virtual string TourGroupNbr { get; set; }
        public abstract class tourGroupNbr : PX.Data.BQL.BqlString.Field<tourGroupNbr> { }
        #endregion

        #region TourGroupItemID
        [PXDBIdentity(IsKey = true)]
        public virtual int? TourGroupItemID { get; set; }
        public abstract class tourGroupItemID : PX.Data.BQL.BqlInt.Field<tourGroupItemID> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXUIField(DisplayName = "Inventory ID", Required = true)]
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

        #region Date
        [PXDBDate()]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
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
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
        #endregion

        #region CuryID
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXUIField(DisplayName = "Currency", Required = true)]
        //[PXDefault(typeof(Search<LUMTourTypeClass.curyID,
        //    Where<LUMTourTypeClass.typeClassID, Equal<Current<LUMTourGroup.tourTypeClassID>>>>),
        //    PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Currency.curyID))]
        public virtual string CuryID { get; set; }
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
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
        [PXUIField(DisplayName = "AP Bill")]
        [PXSelector(typeof(Search<APInvoice.refNbr,Where<APInvoice.docType,Equal<APDocType.invoice>>>))]
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
    }
}