using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.GL;
using static HH_Customization.Descriptor.LUMStringList;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourCostStructure")]
    public class LUMTourCostStructure : PXBqlTable, IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourCostStructure>.By<costStructureID>
        {
            public static LUMTourCostStructure Find(PXGraph graph, int? costStructureID) => FindBy(graph, costStructureID);
        }
        #endregion

        #region TypeClassID
        [PXDBInt()]
        [PXUIField(DisplayName = "Type Class ID")]
        [PXDBDefault(typeof(LUMTourTypeClass.typeClassID))]
        [PXParent(typeof(Select<LUMTourTypeClass, Where<LUMTourTypeClass.typeClassID, Equal<Current<typeClassID>>>>))]
        public virtual int? TypeClassID { get; set; }
        public abstract class typeClassID : PX.Data.BQL.BqlInt.Field<typeClassID> { }
        #endregion

        #region CostStructureID
        [PXDBIdentity(IsKey = true)]
        public virtual int? CostStructureID { get; set; }
        public abstract class costStructureID : PX.Data.BQL.BqlInt.Field<costStructureID> { }
        #endregion

        #region Level
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Level")]
        [LUMTourLevel]
        [PXDefault(LUMTourLevel.GROUP)]
        public virtual string Level { get; set; }
        public abstract class level : PX.Data.BQL.BqlString.Field<level> { }
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

        #region ExtCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ext Cost", Required = true)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
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
    }
}