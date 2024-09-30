using System;
using PX.Data;

namespace LeaveAndOvertimeCustomization.DAC
{
    [Serializable]
    [PXCacheName("LumLeaveType")]
    public class LumLeaveType : PXBqlTable, IBqlTable
    {
        #region LeaveType
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Leave Type")]
        public virtual string LeaveType { get; set; }
        public abstract class leaveType : PX.Data.BQL.BqlString.Field<leaveType> { }
        #endregion

        #region Description
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Leaveuom
        [PXDBString(1, IsUnicode = true, InputMask = "")]
        [PXDefault(TypeCode.String, "D")]
        [PXStringList(new string[] { "D", "H" }, new string[] { "Day", "Hour" })]
        [PXUIField(DisplayName = "Leave Uom")]
        public virtual string Leaveuom { get; set; }
        public abstract class leaveuom : PX.Data.BQL.BqlString.Field<leaveuom> { }
        #endregion

        #region MaxLeaveDays
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Maxinum Days per Year")]
        public virtual Decimal? MaxLeaveDays { get; set; }
        public abstract class maxLeaveDays : PX.Data.BQL.BqlDecimal.Field<maxLeaveDays> { }
        #endregion

        #region MaxLeaveHour
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Minimum Hours per Leave")]
        public virtual Decimal? MinLeaveHour { get; set; }
        public abstract class minLeaveHour : PX.Data.BQL.BqlDecimal.Field<minLeaveHour> { }
        #endregion

        #region IsAnnualLeave
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Annual Leave")]
        public virtual bool? IsAnnualLeave { get; set; }
        public abstract class isAnnualLeave : PX.Data.BQL.BqlBool.Field<isAnnualLeave> { }
        #endregion

        #region IsAttachedRequired
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Attached Required")]
        public virtual bool? IsAttachedRequired { get; set; }
        public abstract class isAttachedRequired : PX.Data.BQL.BqlBool.Field<isAttachedRequired> { }
        #endregion

        #region IsOnlyWorkDay
        [PXDBBool()]
        [PXUIField(DisplayName = "Only Work Day")]
        public virtual bool? IsOnlyWorkDay { get; set; }
        public abstract class isOnlyWorkDay : PX.Data.BQL.BqlBool.Field<isOnlyWorkDay> { }
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