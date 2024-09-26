using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMTouchBistroPreference")]
    public class LUMTouchBistroPreference : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<LUMTouchBistroPreference>.By<restaurantID>
        {
            public static LUMTouchBistroPreference Find(PXGraph graph, int? restaurantID) => FindBy(graph, restaurantID);
        }

        public class UK : PrimaryKeyOf<LUMTouchBistroPreference>.By<restaurantCD>
        {
            public static LUMTouchBistroPreference Find(PXGraph graph, string restaurantCD) => FindBy(graph, restaurantCD);
        }
        #endregion

        #region RestaurantID
        [PXDBIdentity(IsKey = true)]
        public virtual int? RestaurantID { get; set; }
        public abstract class restaurantID : PX.Data.BQL.BqlInt.Field<restaurantID> { }
        #endregion

        #region RestaurantCD
        [PXDBString(25, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Restaurant ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string RestaurantCD { get; set; }
        public abstract class restaurantCD : PX.Data.BQL.BqlString.Field<restaurantCD> { }
        #endregion

        #region Branch
        [PXDBInt()]
        [PXUIField(DisplayName = "Branch", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Branch.branchID),
            SubstituteKey = typeof(Branch.branchCD),
            DescriptionField = typeof(Branch.acctName))]
        public virtual int? Branch { get; set; }
        public abstract class branch : PX.Data.BQL.BqlInt.Field<branch> { }
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

        #region Active
        [PXDBBool()]
        [PXUIField(DisplayName = "Active", Required = true)]
        [PXDefault(true,PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual bool? Active { get; set; }
        public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
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