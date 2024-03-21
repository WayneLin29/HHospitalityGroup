using System;
using PX.Data;
using PX.Objects.GL;

namespace HHAPICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMHRPayrollBaseDetails")]
    public class LUMHRPayrollBaseDetails : IBqlTable
    {
        #region DocRefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Doc Ref Nbr", Visible = false, Enabled = false)]
        [PXDBDefault(typeof(LUMHRPayrollBaseDocument.docRefNbr))]
        public virtual string DocRefNbr { get; set; }
        public abstract class docRefNbr : PX.Data.BQL.BqlString.Field<docRefNbr> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Line Nbr", Visible = false, Enabled = false)]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region OriginBranchID
        [PXDefault]
        [Branch(typeof(AccessInfo.branchID), IsDetail = false, TabOrder = 0)]
        [PXUIField(DisplayName = "Origin Branch ID")]
        public virtual int? OriginBranchID { get; set; }
        public abstract class originBranchID : PX.Data.BQL.BqlInt.Field<originBranchID> { }
        #endregion

        #region OriginBatchNbr
        [PXDefault]
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Origin Batch Nbr")]
        public virtual string OriginBatchNbr { get; set; }
        public abstract class originBatchNbr : PX.Data.BQL.BqlString.Field<originBatchNbr> { }
        #endregion

        #region OriginLineNbr
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Origin Line Nbr")]
        public virtual int? OriginLineNbr { get; set; }
        public abstract class originLineNbr : PX.Data.BQL.BqlInt.Field<originLineNbr> { }
        #endregion

        #region TransactionDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Transaction Date")]
        public virtual DateTime? TransactionDate { get; set; }
        public abstract class transactionDate : PX.Data.BQL.BqlDateTime.Field<transactionDate> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region PostingBranchID
        [Branch(typeof(AccessInfo.branchID), IsDetail = false, TabOrder = 0)]
        [PXUIField(DisplayName = "Posting Branch ID")]
        public virtual int? PostingBranchID { get; set; }
        public abstract class postingBranchID : PX.Data.BQL.BqlInt.Field<postingBranchID> { }
        #endregion

        #region AccountID
        [Account(typeof(LUMHRPayrollBaseDetails.postingBranchID), DescriptionField = typeof(Account.description))]
        [PXDefault]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region Subid
        [SubAccount(typeof(LUMHRPayrollBaseDetails.accountID), typeof(LUMHRPayrollBaseDetails.postingBranchID), true)]
        [PXDefault]
        public virtual int? SubID { get; set; }
        public abstract class subid : PX.Data.BQL.BqlInt.Field<subid> { }
        #endregion

        #region TranDesc
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tran Desc")]
        public virtual string TranDesc { get; set; }
        public abstract class tranDesc : PX.Data.BQL.BqlString.Field<tranDesc> { }
        #endregion

        #region DebitAmount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Debit Amount")]
        public virtual Decimal? DebitAmount { get; set; }
        public abstract class debitAmount : PX.Data.BQL.BqlDecimal.Field<debitAmount> { }
        #endregion

        #region CreditAmount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Credit Amount")]
        public virtual Decimal? CreditAmount { get; set; }
        public abstract class creditAmount : PX.Data.BQL.BqlDecimal.Field<creditAmount> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ref Nbr")]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region UsrTaxZone
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tax Zone")]
        [PXSelector(typeof(PX.Objects.TX.TaxZone.taxZoneID), DescriptionField = typeof(PX.Objects.TX.TaxZone.descr), Filterable = true)]
        public virtual string UsrTaxZone { get; set; }
        public abstract class usrTaxZone : PX.Data.BQL.BqlString.Field<usrTaxZone> { }
        #endregion

        #region UsrTaxCategory
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tax Category")]
        [PXSelector(typeof(PX.Objects.TX.TaxCategory.taxCategoryID), DescriptionField = typeof(PX.Objects.TX.TaxCategory.descr))]
        public virtual string UsrTaxCategory { get; set; }
        public abstract class usrTaxCategory : PX.Data.BQL.BqlString.Field<usrTaxCategory> { }
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