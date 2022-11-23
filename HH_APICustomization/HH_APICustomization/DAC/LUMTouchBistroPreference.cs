using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMTouchBistroPreference")]
    public class LUMTouchBistroPreference : IBqlTable
    {
        #region Keys
        //public class PK : PrimaryKeyOf<LUMTouchBistroPreference>.By<resturantID>
        //{
        //    public static LUMTouchBistroPreference Find(PXGraph graph, int? resturantID) => FindBy(graph, resturantID);
        //}

        //public class UK : PrimaryKeyOf<LUMTouchBistroPreference>.By<resturantCD>
        //{
        //    public static LUMTouchBistroPreference Find(PXGraph graph, string resturantCD) => FindBy(graph, resturantCD);
        //}
        #endregion

        #region ResturantID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ResturantID { get; set; }
        public abstract class resturantID : PX.Data.BQL.BqlInt.Field<resturantID> { }
        #endregion

        #region ResturantCD
        [PXDBString(25, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Resturant ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string ResturantCD { get; set; }
        public abstract class resturantCD : PX.Data.BQL.BqlString.Field<resturantCD> { }
        #endregion

        #region BranchID
        [PXDBInt()]
        [PXUIField(DisplayName = "Branch ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Branch.branchID),
            SubstituteKey = typeof(Branch.branchCD),
            DescriptionField = typeof(Branch.acctName))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region CashAccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "Cash Account ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Account.accountID),
            SubstituteKey = typeof(Account.accountCD),
            DescriptionField = typeof(Account.description))]
        public virtual int? CashAccountID { get; set; }
        public abstract class cashAccountID : PX.Data.BQL.BqlInt.Field<cashAccountID> { }
        #endregion

        #region CashSubAcctID
        [PXDBInt()]
        [PXUIField(DisplayName = "Cash Sub Acct ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Sub.subID),
            SubstituteKey = typeof(Sub.subCD),
            DescriptionField = typeof(Sub.description))]
        public virtual int? CashSubAcctID { get; set; }
        public abstract class cashSubAcctID : PX.Data.BQL.BqlInt.Field<cashSubAcctID> { }
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