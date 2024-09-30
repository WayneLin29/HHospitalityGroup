using System;
using System.Collections.Generic;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.TM;

namespace LeaveAndOvertimeCustomization.DAC
{

    public class LumLeaveRequestStatus
    {
        public const string OnHold = "H";
        public const string PendingApproval = "P";
        public const string Approved = "A";
        public const string Rejected = "J";
        public const string Release = "R";
        public const string Cancel = "C";

        public class onHold : PX.Data.BQL.BqlString.Constant<onHold>
        {
            public onHold() : base(OnHold) { }
        }

        public class pendingApproval : PX.Data.BQL.BqlString.Constant<pendingApproval>
        {
            public pendingApproval() : base(PendingApproval) { }
        }

        public class approved : PX.Data.BQL.BqlString.Constant<approved>
        {
            public approved() : base(Approved) { }
        }

        public class rejected : PX.Data.BQL.BqlString.Constant<rejected>
        {
            public rejected() : base(Rejected) { }
        }

        public class release : PX.Data.BQL.BqlString.Constant<release>
        {
            public release() : base(Release) { }
        }

        public class cancel : PX.Data.BQL.BqlString.Constant<cancel>
        {
            public cancel() : base(Cancel) { }
        }

        private static readonly IEnumerable<ValueLabelPair> _valueLabelPairs = new ValueLabelList
        {
            { OnHold, "On Hold" },
            { PendingApproval,"Pending Approval" },
            { Approved, "Approved" },
            { Rejected,"Rejected" },
            { Release, "Release"  },
            { Cancel,"Cancel"}
        };

        public IEnumerable<ValueLabelPair> ValueLabelPairs => _valueLabelPairs;

        public class ListAttribute : LabelListAttribute
        {
            public ListAttribute() : base(_valueLabelPairs) { }
        }
    }


    [Serializable]
    [PXEMailSource]
    [PXCacheName("Leave Request")]
    [PXPrimaryGraph(typeof(LeaveRequestEntry))]
    public class LumLeaveRequest : PXBqlTable, IBqlTable, PX.Data.EP.IAssign
    {
        #region RefNbr
        [PXDefault]
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [AutoNumber(typeof(LumLeaveAndOvertimePreference.leaveRequestSequenceID), typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Ref Nbr")]
        [PXSelector(typeof(SelectFrom<LumLeaveRequest>
                           .InnerJoin<EPEmployee>.On<LumLeaveRequest.requestEmployeeID.IsEqual<EPEmployee.bAccountID>>
                           .Where<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>>
                           .SearchFor<LumLeaveRequest.refNbr>),
                    typeof(LumLeaveRequest.refNbr),
                    typeof(LumLeaveRequest.leaveType),
                    typeof(LumLeaveRequest.requestEmployeeID),
                    typeof(LumLeaveRequest.status))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region Status
        [PXDBString(1, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        [PXDefault(LumLeaveRequestStatus.OnHold)]
        [LumLeaveRequestStatus.List]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region RequestEmployeeID
        [PXDBInt()]
        [PXUIField(DisplayName = "Request Employee ID", Enabled = false)]
        [PXDefault(typeof(Search<EPEmployee.bAccountID, Where<EPEmployee.userID, Equal<AccessInfo.userID.FromCurrent>>>))]
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
        public virtual int? RequestEmployeeID { get; set; }
        public abstract class requestEmployeeID : PX.Data.BQL.BqlInt.Field<requestEmployeeID> { }
        #endregion

        #region LeaveType
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXDefault]
        [PXUIField(DisplayName = "Leave Type")]
        [PXSelector(typeof(
            SelectFrom<LumLeaveType>
            .SearchFor<LumLeaveType.leaveType>
            ))]
        public virtual string LeaveType { get; set; }
        public abstract class leaveType : PX.Data.BQL.BqlString.Field<leaveType> { }
        #endregion

        #region RequestDate
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Request Date", Enabled = false)]
        public virtual DateTime? RequestDate { get; set; }
        public abstract class requestDate : PX.Data.BQL.BqlDateTime.Field<requestDate> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXDefault]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region LeaveStart
        [PXDefault]
        [PXDBDateAndTime(DisplayMask = "g", InputMask = "g", PreserveTime = true, UseTimeZone = true)]
        [PXUIField(DisplayName = "Leave Start")]
        public virtual DateTime? LeaveStart { get; set; }
        public abstract class leaveStart : PX.Data.BQL.BqlDateTime.Field<leaveStart> { }
        #endregion

        #region LeaveEnd
        [PXDefault]
        [PXDBDateAndTime(DisplayMask = "g", InputMask = "g", PreserveTime = true, UseTimeZone = true)]
        [PXUIField(DisplayName = "Leave End")]
        public virtual DateTime? LeaveEnd { get; set; }
        public abstract class leaveEnd : PX.Data.BQL.BqlDateTime.Field<leaveEnd> { }
        #endregion

        #region DelegateEmployeeID
        [PXDBInt()]
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
        [PXUIField(DisplayName = "Delegate Employee ID")]
        public virtual int? DelegateEmployeeID { get; set; }
        public abstract class delegateEmployeeID : PX.Data.BQL.BqlInt.Field<delegateEmployeeID> { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Hold", Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region PendingApproval
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "PendingApproval", Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? PendingApproval { get; set; }
        public abstract class pendingApproval : PX.Data.BQL.BqlBool.Field<pendingApproval> { }
        #endregion

        #region Approved
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Approved", Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Approved { get; set; }
        public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Rejected", Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Rejected { get; set; }
        public abstract class rejected : PX.Data.BQL.BqlBool.Field<rejected> { }
        #endregion

        #region Release
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Release", Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Release { get; set; }
        public abstract class release : PX.Data.BQL.BqlBool.Field<release> { }
        #endregion

        #region WorkgroupID
        [PXDBInt]
        [PXDBDefault]
        [PXUIField(DisplayName = "Approval Workgroup")]
        [PXDefault(typeof(Search<EPEmployee.defaultWorkgroupID, Where<EPEmployee.userID, Equal<AccessInfo.userID.FromCurrent>>>))]
        [PXSelector(typeof(SelectFrom<EPCompanyTree>
                          .InnerJoin<EPCompanyTreeMember>.On<EPCompanyTreeMember.workGroupID.IsEqual<EPCompanyTree.workGroupID>>
                          .InnerJoin<BAccount2>.On<EPCompanyTreeMember.contactID.IsEqual<BAccount2.defContactID>>
                          .InnerJoin<EPEmployee>.On<BAccount2.bAccountID.IsEqual<EPEmployee.bAccountID>>
                          .Where<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>>
                          .SearchFor<EPCompanyTree.workGroupID>),
                    SubstituteKey = typeof(EPCompanyTree.description))]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : PX.Data.BQL.BqlInt.Field<workgroupID> { }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = "Approver")]
        [PXDefault(typeof(Search<CREmployee.defContactID, Where<CREmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : PX.Data.BQL.BqlInt.Field<ownerID> { }
        #endregion

        #region IsApprover
        [PXBool]
        public virtual bool? IsApprover { get; set; }
        public abstract class isApprover : PX.Data.BQL.BqlBool.Field<isApprover> { }
        #endregion

        #region Duration
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Duration(Hrs)", Enabled = false)]
        public virtual decimal? Duration { get; set; }
        public abstract class duration : PX.Data.BQL.BqlDecimal.Field<duration> { }
        #endregion

        #region ApprovedLeaveTimes
        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Approved Leave (Hrs)", Enabled = false)]
        public virtual decimal? ApprovedLeaveTimes { get; set; }
        public abstract class approvedLeaveTimes : PX.Data.BQL.BqlDecimal.Field<approvedLeaveTimes> { }
        #endregion

        #region PendingApprovalTimes
        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Pending Approval Leave (Hrs)", Enabled = false)]
        public virtual decimal? PendingApprovalTimes { get; set; }
        public abstract class pendingApprovalTimes : PX.Data.BQL.BqlDecimal.Field<pendingApprovalTimes> { }
        #endregion

        #region EntitledAnnualLeaveTimes 
        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Entitled Annual Leave", Visible = false, Enabled = false)]
        public virtual decimal? EntitledAnnualLeaveTimes { get; set; }
        public abstract class entitledAnnualLeaveTimes : PX.Data.BQL.BqlDecimal.Field<entitledAnnualLeaveTimes> { }
        #endregion

        #region RemainingAvailableHours
        [PXDecimal]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Remaining Available Hours", Visible = false, Enabled = false)]
        public virtual decimal? RemainingAvailableHours { get; set; }
        public abstract class remainingAvailableHours : PX.Data.BQL.BqlDecimal.Field<remainingAvailableHours> { }
        #endregion

        #region NoteID
        [PXNote(ShowInReferenceSelector = true)]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
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