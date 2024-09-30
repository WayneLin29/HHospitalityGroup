using System;
using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Objects.EP;

namespace LeaveAndOvertimeCustomization
{
    [Serializable]
    [PXCacheName("LumOvertimeApproval")]
    public class LumOvertimeApproval : PXBqlTable, IBqlTable, IAssignedMap
    {
        #region ApprovalID
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Approval ID")]
        public virtual int? ApprovalID { get; set; }
        public abstract class approvalID : PX.Data.BQL.BqlInt.Field<approvalID> { }
        #endregion

        #region AssignmentMapID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Assignment Map ID")]
        [PXSelector(typeof(Search<EPAssignmentMap.assignmentMapID, Where<EPAssignmentMap.entityType, Equal<LeaveAndOvertimeCustomization.Descriptor.AssignmentMapType.AssignmentMapTypeLumOvertime>>>), SubstituteKey = typeof(EPAssignmentMap.name))]
        public virtual int? AssignmentMapID { get; set; }
        public abstract class assignmentMapID : PX.Data.BQL.BqlInt.Field<assignmentMapID> { }
        #endregion

        #region AssignmentNotificationID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Assignment Notification ID")]
        [PXSelector(typeof(PX.SM.Notification.notificationID), SubstituteKey = typeof(PX.SM.Notification.name))]
        public virtual int? AssignmentNotificationID { get; set; }
        public abstract class assignmentNotificationID : PX.Data.BQL.BqlInt.Field<assignmentNotificationID> { }
        #endregion

        #region CancelNotificationID
        [PXDBInt()]
        [PXUIField(DisplayName = "Cancel Notification ID")]
        [PXSelector(typeof(PX.SM.Notification.notificationID), SubstituteKey = typeof(PX.SM.Notification.name))]
        public virtual int? CancelNotificationID { get; set; }
        public abstract class cancelNotificationID : PX.Data.BQL.BqlInt.Field<cancelNotificationID> { }
        #endregion

        #region IsActive
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Active")]
        [PXDefault(typeof(Search<LumLeaveAndOvertimePreference.leaveRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : PX.Data.BQL.BqlBool.Field<isActive> { }
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
    }
}