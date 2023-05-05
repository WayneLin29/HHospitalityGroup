using System;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.EP;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMHRApprovalV")]
    public class LUMHRApprovalV : IBqlTable
    {
        #region Selected
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region DataType
        [PXDBString(2, InputMask = "",IsKey = true)]
        [PXUIField(DisplayName = "Data Type")]
        public virtual string DataType { get; set; }
        public abstract class dataType : PX.Data.BQL.BqlString.Field<dataType> { }
        #endregion

        #region CutoffDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Cutoff Date")]
        public virtual DateTime? CutoffDate { get; set; }
        public abstract class cutoffDate : PX.Data.BQL.BqlDateTime.Field<cutoffDate> { }
        #endregion

        #region BranchID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Branch ID")]
        [PXSelector(typeof(Search<Branch.branchID>),
                typeof(Branch.branchCD),
                typeof(Branch.acctName),
                SubstituteKey = typeof(Branch.branchCD),
                DescriptionField = typeof(Branch.acctName)
                )]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region EmployeeID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Employee ID")]
        [PXSelector(typeof(Search<EPEmployee.bAccountID>),
                typeof(EPEmployee.acctCD),
                typeof(EPEmployee.acctName),
                SubstituteKey = typeof(EPEmployee.acctCD),
                DescriptionField = typeof(EPEmployee.acctName)
                )]
        public virtual int? EmployeeID { get; set; }
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
        #endregion

        #region Date
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Type
        [PXDBString(20, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Type")]
        //[PXSelector(typeof(Search<EPEarningType.typeCD>),
        //    typeof(EPEarningType.typeCD),
        //    typeof(EPEarningType.description))]
        //[PXSelector(typeof(Search<CSAttributeDetail.valueID, Where<CSAttributeDetail.attributeID, Equal<PADJUSTAttr>>>),
        //            typeof(CSAttributeDetail.description))]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region Hour
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Hour")]
        public virtual Decimal? Hour { get; set; }
        public abstract class hour : PX.Data.BQL.BqlDecimal.Field<hour> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region Approved
        [PXDBBool]
        [PXUIField(DisplayName = "Approved")]
        public virtual bool? Approved { get; set; }
        public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
        #endregion
    }
}