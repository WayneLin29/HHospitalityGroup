﻿using System;
using PX.CS;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.Objects.GL;

namespace HHAPICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMPayrollAdjustment")]
    public class LUMPayrollAdjustment : PXBqlTable, IBqlTable
    {
        #region Branch
        [PXUIField(DisplayName = "Branch")]
        [Branch(typeof(AccessInfo.branchID), IsDetail = false, TabOrder = 0, IsKey = true)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region AdjustmentDate
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? AdjustmentDate { get; set; }
        public abstract class adjustmentDate : PX.Data.BQL.BqlDateTime.Field<adjustmentDate> { }
        #endregion

        #region EmployeeID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Employee ID")]
        [PXSelector(typeof(
                SelectFrom<EPEmployee>
                .InnerJoin<BAccount2>.On<EPEmployee.bAccountID.IsEqual<BAccount2.bAccountID>>
                .LeftJoin<Contact>.On<BAccount2.defContactID.IsEqual<Contact.contactID>>
                .SearchFor<EPEmployee.bAccountID>),
                typeof(EPEmployee.acctCD),
                typeof(EPEmployee.acctName),
                typeof(EPEmployee.routeEmails),
                typeof(Contact.eMail),
                typeof(EPEmployee.status),
                typeof(EPEmployee.classID),
                SubstituteKey = typeof(EPEmployee.acctCD))]
        public virtual int? EmployeeID { get; set; }
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
        #endregion

        #region EmployeeName
        [PXString(IsUnicode = true)]
        [PXFormula(typeof(Selector<employeeID, EPEmployee.acctName>))]
        [PXDefault(typeof(Search<EPEmployee.acctName, Where<EPEmployee.bAccountID, Equal<Current<LUMPayrollHour.employeeID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Employee Name", Enabled = false)]
        public virtual string EmployeeName { get; set; }
        public abstract class employeeName : PX.Data.BQL.BqlString.Field<employeeName> { }
        #endregion

        #region AdjustmentType
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Adjustment Type")]
        [PXSelector(typeof(Search<CSAttributeDetail.valueID, Where<CSAttributeDetail.attributeID, Equal<PADJUSTAttr>>>),
                    typeof(CSAttributeDetail.description))]
        public virtual string AdjustmentType { get; set; }
        public abstract class adjustmentType : PX.Data.BQL.BqlString.Field<adjustmentType> { }
        #endregion

        #region AdjustmentDescription
        [PXString(IsUnicode = true)]
        [PXFormula(typeof(Selector<adjustmentType, CSAttributeDetail.description>))]
        [PXDefault(typeof(Search<CSAttributeDetail.description,
                          Where<CSAttributeDetail.attributeID, Equal<Current<LUMPayrollAdjustment.adjustmentType>>,
                            And<CSAttributeDetail.attributeID, Equal<PADJUSTAttr>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Adjustment Description", Enabled = false)]
        public virtual string AdjustmentDescription { get; set; }
        public abstract class adjustmentDescription : PX.Data.BQL.BqlString.Field<adjustmentDescription> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region Remark
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Remark")]
        public virtual string Remark { get; set; }
        public abstract class remark : PX.Data.BQL.BqlString.Field<remark> { }
        #endregion

        #region Approved
        [PXDBBool]
        [PXUIField(DisplayName = "Apprvoed", Enabled = false)]
        public virtual bool? Approved { get; set; }
        public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
        #endregion

        #region CutOffDate
        [PXDBDate]
        [PXUIField(DisplayName = "Cut-Off Date")]
        public virtual DateTime? CutOffDate { get; set; }
        public abstract class cutOffDate : PX.Data.BQL.BqlDateTime.Field<cutOffDate> { }
        #endregion

        #region ApprovedAmount
        [PXDBDecimal]
        [PXUIField(DisplayName = "Approved Amount", Enabled = false)]
        public virtual decimal? ApprovedAmount { get; set; }
        public abstract class approvedAmount : PX.Data.BQL.BqlBool.Field<approvedAmount> { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Hold", Enabled = false)]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region BatchNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Batch Nbr")]
        public virtual string BatchNbr { get; set; }
        public abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }
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
        [PXUIField(Enabled = false)]
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
        [PXUIField(Enabled = false)]
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

    public class PADJUSTAttr : PX.Data.BQL.BqlString.Constant<PADJUSTAttr>
    {
        public PADJUSTAttr() : base("PADJUST") { }
    }
}