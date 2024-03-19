using System;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.GL.DAC;

namespace HHAPICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAllowCombination")]
    public class LUMAllowCombination : IBqlTable
    {
        #region BranchID
        [PXUIField(DisplayName = "Branch")]
        [Branch(typeof(AccessInfo.branchID), IsDetail = false, TabOrder = 0, IsKey = true)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region LedgerID
        [PXDBInt(IsKey = true)]
        [PXDefault(typeof(Search<Branch.ledgerID, Where<Branch.branchID, Equal<Current<LUMAllowCombination.branchID>>>>))]
        [PXUIField(DisplayName = "Ledger", Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search5<Ledger.ledgerID,
                                LeftJoin<OrganizationLedgerLink,
                                    On<Ledger.ledgerID, Equal<OrganizationLedgerLink.ledgerID>>,
                                LeftJoin<Branch,
                                    On<Branch.organizationID, Equal<OrganizationLedgerLink.organizationID>, And<Branch.branchID, Equal<Current2<LUMAllowCombination.branchID>>>>>>,
                                Where<Ledger.balanceType, NotEqual<LedgerBalanceType.budget>>,
                                Aggregate<GroupBy<Ledger.ledgerID>>>),
                        SubstituteKey = typeof(Ledger.ledgerCD),
                        DescriptionField = typeof(Ledger.descr),
                        CacheGlobal = true
        )]
        public virtual int? LedgerID { get; set; }
        public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
        #endregion

        #region AccountID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Account ID")]
        [PXDefault]
        [PXSelector(typeof(Search<Account.accountID>),
                    typeof(Account.accountCD),
                    typeof(Account.description),
                    SubstituteKey = typeof(Account.accountCD))]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region Subid
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Subid")]
        [PXSelector(typeof(Search<Sub.subID>),
                    typeof(Sub.subCD),
                    typeof(Sub.description),
                    SubstituteKey = typeof(Sub.subCD))]
        public virtual int? Subid { get; set; }
        public abstract class subid : PX.Data.BQL.BqlInt.Field<subid> { }
        #endregion

        #region Remeark
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Remark")]
        public virtual string Remark { get; set; }
        public abstract class remark : PX.Data.BQL.BqlString.Field<remark> { }
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