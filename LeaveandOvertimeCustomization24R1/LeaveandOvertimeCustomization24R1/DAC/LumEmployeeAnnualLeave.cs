using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CR;
using PX.Objects.EP;

namespace LeaveAndOvertimeCustomization.DAC
{
    [Serializable]
    [PXCacheName("LumEmployeeAnnualLeave")]
    public class LumEmployeeAnnualLeave : PXBqlTable, IBqlTable
    {
        public class PK : PrimaryKeyOf<LumEmployeeAnnualLeave>.By<employeeID, leaveType>
        {
            public static LumEmployeeAnnualLeave Find(PXGraph graph, int? employeeid, string leaveType) => FindBy(graph, employeeid, leaveType);
        }

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
                SubstituteKey = typeof(EPEmployee.acctCD),
                DescriptionField = typeof(EPEmployee.acctName))]
        public virtual int? EmployeeID { get; set; }
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
        #endregion

        #region LeaveType
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Leave Type")]
        [PXSelector(typeof(
            SelectFrom<LumLeaveType>
            .Where<LumLeaveType.isAnnualLeave.IsEqual<True>>
            .SearchFor<LumLeaveType.leaveType>
            ))]
        public virtual string LeaveType { get; set; }
        public abstract class leaveType : PX.Data.BQL.BqlString.Field<leaveType> { }
        #endregion

        #region LineCntr
        [PXDBInt]
        [PXDefault(0)]
        public virtual int? LineCntr { get; set; }
        public abstract class lineCntr : PX.Data.BQL.BqlInt.Field<lineCntr> { }
        #endregion

        #region AvailCompensatedHrs
        [PXDecimal]
        [PXUIField(DisplayName = "Available Compensated Hours",Enabled = false)]
        public decimal? AvailCompensatedHrs { get;set;}
        public abstract class availCompensatedHrs : PX.Data.BQL.BqlDecimal.Field<availCompensatedHrs> { }
        #endregion

        #region UsedCompensatedHrs
        [PXDecimal]
        [PXUIField(DisplayName = "Approved Compensated Hours", Enabled = false)]
        public decimal? UsedCompensatedHrs { get; set; }
        public abstract class usedCompensatedHrs : PX.Data.BQL.BqlDecimal.Field<usedCompensatedHrs> { }
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