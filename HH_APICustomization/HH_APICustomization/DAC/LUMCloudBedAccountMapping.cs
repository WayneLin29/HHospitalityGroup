using System;
using PX.Data;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedAccountMapping")]
    public class LUMCloudBedAccountMapping : PXBqlTable, IBqlTable
    {
        #region SequenceID
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "SequenceID", Visible = false)]
        public virtual int? SequenceID { get; set; }
        public abstract class sequenceID : PX.Data.BQL.BqlInt.Field<sequenceID> { }
        #endregion

        #region Type
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region CloudBedPropertyID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "PropertyID")]
        [PXSelector(typeof(Search<LUMCloudBedPreference.cloudBedPropertyID, Where<LUMCloudBedPreference.active, Equal<True>>>),
            typeof(LUMCloudBedPreference.branchID),
            typeof(LUMCloudBedPreference.debitAcct),
            typeof(LUMCloudBedPreference.debitSub),
            typeof(LUMCloudBedPreference.creditAcct),
            typeof(LUMCloudBedPreference.creditSub))]
        public virtual string CloudBedPropertyID { get; set; }
        public abstract class cloudBedPropertyID : PX.Data.BQL.BqlString.Field<cloudBedPropertyID> { }
        #endregion

        #region TransCategory
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Trans Category")]
        public virtual string TransCategory { get; set; }
        public abstract class transCategory : PX.Data.BQL.BqlString.Field<transCategory> { }
        #endregion

        #region HouseAccount
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "House Account")]
        public virtual string HouseAccount { get; set; }
        public abstract class houseAccount : PX.Data.BQL.BqlString.Field<houseAccount> { }
        #endregion

        #region TransactionCode
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Code")]
        public virtual string TransactionCode { get; set; }
        public abstract class transactionCode : PX.Data.BQL.BqlString.Field<transactionCode> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Source
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Source")]
        public virtual string Source { get; set; }
        public abstract class source : PX.Data.BQL.BqlString.Field<source> { }
        #endregion

        #region AccountID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Account ID")]
        [PXSelector(typeof(Search<Account.accountID>),
                    typeof(Account.accountCD),
                    typeof(Account.description),
                    SubstituteKey = typeof(Account.accountCD))]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubAccountID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Sub Account ID")]
        [PXSelector(typeof(Search<Sub.subID>),
                    typeof(Sub.subCD),
                    typeof(Sub.description),
                    SubstituteKey = typeof(Sub.subCD))]
        public virtual int? SubAccountID { get; set; }
        public abstract class subAccountID : PX.Data.BQL.BqlInt.Field<subAccountID> { }
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