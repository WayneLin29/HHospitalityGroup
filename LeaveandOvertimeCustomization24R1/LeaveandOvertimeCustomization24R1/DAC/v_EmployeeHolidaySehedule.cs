using System;
using PX.Data;

namespace LeaveAndOvertimeCustomization.DAC
{
    [Serializable]
    [PXCacheName("v_EmployeeHolidaySehedule")]
    public class v_EmployeeHolidaySehedule : PXBqlTable, IBqlTable
    {
        #region Catagory
        [PXDBString(11, InputMask = "")]
        [PXUIField(DisplayName = "Catagory")]
        public virtual string Catagory { get; set; }
        public abstract class catagory : PX.Data.BQL.BqlString.Field<catagory> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Ref Nbr")]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region Type
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region RequestEmployeeID
        [PXDBInt()]
        [PXUIField(DisplayName = "Request Employee ID")]
        public virtual int? RequestEmployeeID { get; set; }
        public abstract class requestEmployeeID : PX.Data.BQL.BqlInt.Field<requestEmployeeID> { }
        #endregion

        #region RequestDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Request Date")]
        public virtual DateTime? RequestDate { get; set; }
        public abstract class requestDate : PX.Data.BQL.BqlDateTime.Field<requestDate> { }
        #endregion

        #region StartDate
        [PXDBDateAndTime()]
        [PXUIField(DisplayName = "Start Date")]
        public virtual DateTime? StartDate { get; set; }
        public abstract class startDate : PX.Data.BQL.BqlDateTime.Field<startDate> { }
        #endregion

        #region EndDate
        [PXDBDateAndTime()]
        [PXUIField(DisplayName = "End Date")]
        public virtual DateTime? EndDate { get; set; }
        public abstract class endDate : PX.Data.BQL.BqlDateTime.Field<endDate> { }
        #endregion

        #region Duration
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Duration")]
        public virtual Decimal? Duration { get; set; }
        public abstract class duration : PX.Data.BQL.BqlDecimal.Field<duration> { }
        #endregion
    }
}