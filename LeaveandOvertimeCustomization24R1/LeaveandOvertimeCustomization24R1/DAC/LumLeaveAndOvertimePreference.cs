using System;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.EP;

namespace LeaveAndOvertimeCustomization.DAC
{
    [Serializable]
    [PXCacheName("LumLeaveAndOvertimePreference")]
    public class LumLeaveAndOvertimePreference : PXBqlTable, IBqlTable
    {
        #region LeaveRequestSequenceID
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Leave Request Sequence ID")]
        [PXSelector(typeof(Numbering.numberingID))]
        public virtual string LeaveRequestSequenceID { get; set; }
        public abstract class leaveRequestSequenceID : PX.Data.BQL.BqlString.Field<leaveRequestSequenceID> { }
        #endregion

        #region OvertimeSequenceID
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Overtime Sequence ID")]
        [PXSelector(typeof(Numbering.numberingID))]
        public virtual string OvertimeSequenceID { get; set; }
        public abstract class overtimeSequenceID : PX.Data.BQL.BqlString.Field<overtimeSequenceID> { }
        #endregion

        #region MaxOTinWorkDay
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Maximum OT hours in Working Day")]
        public virtual Decimal? MaxOTinWorkDay { get; set; }
        public abstract class maxOTinWorkDay : PX.Data.BQL.BqlDecimal.Field<maxOTinWorkDay> { }
        #endregion

        #region MaxOTinHoliday
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Maximum OT hours in Holiday")]
        public virtual Decimal? MaxOTinHoliday { get; set; }
        public abstract class maxOTinHoliday : PX.Data.BQL.BqlDecimal.Field<maxOTinHoliday> { }
        #endregion

        #region MaxOTPerMonth
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Maximum OT hours per month")]
        public virtual Decimal? MaxOTPerMonth { get; set; }
        public abstract class maxOTPerMonth : PX.Data.BQL.BqlDecimal.Field<maxOTPerMonth> { }
        #endregion

        #region LeaveRequestApproval
        [EPRequireApproval]
        [PXDefault(false,PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Leave Request Approval Map")]
        public virtual bool? LeaveRequestApproval { get;set;}
        public abstract class leaveRequestApproval : PX.Data.BQL.BqlBool.Field<leaveRequestApproval> { }

        #endregion


        #region LeaveOvertimeApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Overtime Approval Map")]
        public virtual bool? OvertimeApproval { get; set; }
        public abstract class overtimeApproval : PX.Data.BQL.BqlBool.Field<overtimeApproval> { }

        #endregion

        #region OTFactor
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal,"1",PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OT on Weekdays Facotr")]
        public virtual decimal? OTFactor { get;set;}
        public abstract class oTFactor : PX.Data.BQL.BqlDecimal.Field<oTFactor> { }
        #endregion

        #region HolidayFactor
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal,"1",PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Holiday Factor")]
        public virtual decimal? HolidayFactor { get;set;}
        public abstract class holidayFactor : PX.Data.BQL.BqlDecimal.Field<holidayFactor> { }
        #endregion

        #region NationalholidayFactor
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal,"1",PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "National Holiday Factor")]
        public virtual decimal? NationalholidayFactor { get;set;}
        public abstract class nationalholidayFactor : PX.Data.BQL.BqlDecimal.Field<nationalholidayFactor> { }
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