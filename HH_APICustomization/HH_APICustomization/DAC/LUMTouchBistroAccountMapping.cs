using System;
using PX.Data;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMTouchBistroAccountMapping")]
    public class LUMTouchBistroAccountMapping : PXBqlTable, IBqlTable
    {
        #region AccountMappingID
        [PXDBIdentity(IsKey = true)]
        public virtual int? AccountMappingID { get; set; }
        public abstract class accountMappingID : PX.Data.BQL.BqlInt.Field<accountMappingID> { }
        #endregion

        #region Type
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region RestaurantID
        [PXDBInt()]
        [PXUIField(DisplayName = "Restaurant ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<LUMTouchBistroPreference.restaurantID>),
                typeof(LUMTouchBistroPreference.restaurantCD),
                typeof(LUMTouchBistroPreference.branch),
                typeof(LUMTouchBistroPreference.accountID),
                typeof(LUMTouchBistroPreference.subAcctID),
                typeof(LUMTouchBistroPreference.active),
                SubstituteKey = typeof(LUMTouchBistroPreference.restaurantCD),
                DescriptionField = typeof(LUMTouchBistroPreference.branch)
            )]
        public virtual int? RestaurantID { get; set; }
        public abstract class restaurantID : PX.Data.BQL.BqlInt.Field<restaurantID> { }
        #endregion

        #region SalesCategory
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Sales Category")]
        public virtual string SalesCategory { get; set; }
        public abstract class salesCategory : PX.Data.BQL.BqlString.Field<salesCategory> { }
        #endregion

        #region MenuGroup
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Menu Group")]
        public virtual string MenuGroup { get; set; }
        public abstract class menuGroup : PX.Data.BQL.BqlString.Field<menuGroup> { }
        #endregion

        #region MenuItem
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Menu Item")]
        public virtual string MenuItem { get; set; }
        public abstract class menuItem : PX.Data.BQL.BqlString.Field<menuItem> { }
        #endregion

        #region PayAccount
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Pay Account")]
        public virtual string PayAccount { get; set; }
        public abstract class payAccount : PX.Data.BQL.BqlString.Field<payAccount> { }
        #endregion

        #region Reason
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reason")]
        public virtual string Reason { get; set; }
        public abstract class reason : PX.Data.BQL.BqlString.Field<reason> { }
        #endregion

        #region AccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "Account ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Account.accountID),
            SubstituteKey = typeof(Account.accountCD),
            DescriptionField = typeof(Account.description))]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubAcctID
        [PXDBInt()]
        [PXUIField(DisplayName = "Sub Acct ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Sub.subID),
            SubstituteKey = typeof(Sub.subCD),
            DescriptionField = typeof(Sub.description))]
        public virtual int? SubAcctID { get; set; }
        public abstract class subAcctID : PX.Data.BQL.BqlInt.Field<subAcctID> { }
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

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}