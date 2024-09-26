using System;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMHRPayrollAccountMapping")]
    public class LUMHRPayrollAccountMapping : PXBqlTable, IBqlTable
    {
        #region PayrollType
        [PXDBString(200, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Payroll Type")]
        public virtual string PayrollType { get; set; }
        public abstract class payrollType : PX.Data.BQL.BqlString.Field<payrollType> { }
        #endregion

        #region Branch
        [Branch(typeof(APRegister.branchID), IsKey = true)]
        [PXUIField(DisplayName = "Branch")]
        public virtual int? Branch { get; set; }
        public abstract class branch : PX.Data.BQL.BqlInt.Field<branch> { }
        #endregion

        #region DebitAccount
        [Account(typeof(LUMHRPayrollAccountMapping.branch), DisplayName = "Debit Account", Visibility = PXUIVisibility.Visible, Filterable = false, DescriptionField = typeof(Account.description))]
        public virtual int? DebitAccount { get; set; }
        public abstract class debitAccount : PX.Data.BQL.BqlInt.Field<debitAccount> { }
        #endregion

        #region DebitSub
        [SubAccount(typeof(LUMHRPayrollAccountMapping.debitAccount), typeof(LUMHRPayrollAccountMapping.branch), true, DisplayName = "Debit Sub", Visibility = PXUIVisibility.Visible, Filterable = true, TabOrder = 100)]
        public virtual int? DebitSub { get; set; }
        public abstract class debitSub : PX.Data.BQL.BqlInt.Field<debitSub> { }
        #endregion

        #region CreditAcount
        [Account(typeof(LUMHRPayrollAccountMapping.branch), DisplayName = "Credit Account", Visibility = PXUIVisibility.Visible, Filterable = false, DescriptionField = typeof(Account.description))]
        public virtual int? CreditAcount { get; set; }
        public abstract class creditAcount : PX.Data.BQL.BqlInt.Field<creditAcount> { }
        #endregion

        #region CreditSub
        [SubAccount(typeof(LUMHRPayrollAccountMapping.creditAcount), typeof(LUMHRPayrollAccountMapping.branch), true, DisplayName = "Credit Sub", Visibility = PXUIVisibility.Visible, Filterable = true, TabOrder = 100)]
        public virtual int? CreditSub { get; set; }
        public abstract class creditSub : PX.Data.BQL.BqlInt.Field<creditSub> { }
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