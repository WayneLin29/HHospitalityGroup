using System;
using PX.Data;

namespace LeaveAndOvertimeCustomization
{
    [Serializable]
    [PXCacheName("LumEmployeeCompensated")]
    public class LumEmployeeCompensated : PXBqlTable, IBqlTable
    {
        #region EmployeeID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Employee ID")]
        public virtual int? EmployeeID { get; set; }
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ref Nbr")]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region AvailableYear
        [PXDBInt()]
        [PXUIField(DisplayName = "Available Year")]
        public virtual int? AvailableYear { get; set; }
        public abstract class availableYear : PX.Data.BQL.BqlDateTime.Field<availableYear> { }
        #endregion

        #region TransferHours
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Transfer Hours")]
        public virtual Decimal? TransferHours { get; set; }
        public abstract class transferHours : PX.Data.BQL.BqlDecimal.Field<transferHours> { }
        #endregion

        #region AllowCarryForward
        [PXDBBool()]
        [PXUIField(DisplayName = "Allow Carry Forward")]
        public virtual bool? AllowCarryForward { get; set; }
        public abstract class allowCarryForward : PX.Data.BQL.BqlBool.Field<allowCarryForward> { }
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