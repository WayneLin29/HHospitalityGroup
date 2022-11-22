using System;
using PX.Data;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedPreference")]
    public class LUMCloudBedPreference : IBqlTable
    {
        #region CloudBedPropertyID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Cloud Bed Property ID")]
        public virtual string CloudBedPropertyID { get; set; }
        public abstract class cloudBedPropertyID : PX.Data.BQL.BqlString.Field<cloudBedPropertyID> { }
        #endregion

        #region BranchID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Branch ID")]
        [PXSelector(typeof(Search<Branch.branchID, Where<Branch.active.IsEqual<True>>>),
            typeof(Branch.branchCD),
            SubstituteKey = typeof(Branch.branchCD))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region ClearingAcct
        [PXDBInt()]
        [PXUIField(DisplayName = "Clearing Acct")]
        [PXSelector(typeof(Search<Account.accountID>),
                    typeof(Account.accountCD),
                    typeof(Account.description),
                    SubstituteKey = typeof(Account.accountCD))]
        public virtual int? ClearingAcct { get; set; }
        public abstract class clearingAcct : PX.Data.BQL.BqlInt.Field<clearingAcct> { }
        #endregion

        #region ClearingSub
        [PXDBInt()]
        [PXUIField(DisplayName = "Clearing Sub")]
        [PXSelector(typeof(Search<Sub.subID>),
                    typeof(Sub.subCD),
                    typeof(Sub.description),
                    SubstituteKey = typeof(Sub.subCD))]
        public virtual int? ClearingSub { get; set; }
        public abstract class clearingSub : PX.Data.BQL.BqlInt.Field<clearingSub> { }
        #endregion

        #region DebitAcct
        [PXDBInt()]
        [PXUIField(DisplayName = "Debit Acct")]
        [PXSelector(typeof(Search<Account.accountID>),
                    typeof(Account.accountCD),
                    typeof(Account.description),
                    SubstituteKey = typeof(Account.accountCD))]
        public virtual int? DebitAcct { get; set; }
        public abstract class debitAcct : PX.Data.BQL.BqlInt.Field<debitAcct> { }
        #endregion

        #region DebitSub
        [PXDBInt()]
        [PXUIField(DisplayName = "Debit Sub")]
        [PXSelector(typeof(Search<Sub.subID>),
                    typeof(Sub.subCD),
                    typeof(Sub.description),
                    SubstituteKey = typeof(Sub.subCD))]
        public virtual int? DebitSub { get; set; }
        public abstract class debitSub : PX.Data.BQL.BqlInt.Field<debitSub> { }
        #endregion

        #region CreditAcct
        [PXDBInt()]
        [PXUIField(DisplayName = "Credit Acct")]
        [PXSelector(typeof(Search<Account.accountID>),
                    typeof(Account.accountCD),
                    typeof(Account.description),
                    SubstituteKey = typeof(Account.accountCD))]
        public virtual int? CreditAcct { get; set; }
        public abstract class creditAcct : PX.Data.BQL.BqlInt.Field<creditAcct> { }
        #endregion

        #region CreditSub
        [PXDBInt()]
        [PXUIField(DisplayName = "Credit Sub")]
        [PXSelector(typeof(Search<Sub.subID>),
                    typeof(Sub.subCD),
                    typeof(Sub.description),
                    SubstituteKey = typeof(Sub.subCD))]
        public virtual int? CreditSub { get; set; }
        public abstract class creditSub : PX.Data.BQL.BqlInt.Field<creditSub> { }
        #endregion

        #region Active
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Active")]
        public virtual bool? Active { get; set; }
        public abstract class active : PX.Data.BQL.BqlBool.Field<active> { }
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