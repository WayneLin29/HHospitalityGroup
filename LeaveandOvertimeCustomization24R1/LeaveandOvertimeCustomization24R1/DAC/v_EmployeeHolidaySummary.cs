using System;
using PX.Data;

namespace LeaveAndOvertimeCustomization.DAC
{
    [Serializable]
    [PXCacheName("v_EmployeeHolidaySummary")]
    public class v_EmployeeHolidaySummary : PXBqlTable, IBqlTable
    {
        #region EmployeeID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Employee ID")]
        public virtual int? EmployeeID { get; set; }
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
        #endregion

        #region AcctName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Acct Name")]
        public virtual string AcctName { get; set; }
        public abstract class acctName : PX.Data.BQL.BqlString.Field<acctName> { }
        #endregion

        #region Acctcd
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Acctcd")]
        public virtual string Acctcd { get; set; }
        public abstract class acctcd : PX.Data.BQL.BqlString.Field<acctcd> { }
        #endregion

        #region Year
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Year")]
        public virtual int? Year { get; set; }
        public abstract class year : PX.Data.BQL.BqlInt.Field<year> { }
        #endregion

        #region LeaveType
        [PXDBString(20, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Leave Type")]
        public virtual string LeaveType { get; set; }
        public abstract class leaveType : PX.Data.BQL.BqlString.Field<leaveType> { }
        #endregion

        #region Availablehours
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Availablehours")]
        public virtual Decimal? Availablehours { get; set; }
        public abstract class availablehours : PX.Data.BQL.BqlDecimal.Field<availablehours> { }
        #endregion

        #region Usedhour
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Usedhour")]
        public virtual Decimal? Usedhour { get; set; }
        public abstract class usedhour : PX.Data.BQL.BqlDecimal.Field<usedhour> { }
        #endregion

        #region Entitledhours
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Entitledhours")]
        public virtual Decimal? Entitledhours { get; set; }
        public abstract class entitledhours : PX.Data.BQL.BqlDecimal.Field<entitledhours> { }
        #endregion
    }
}