using System;
using HH_APICustomization.DAC;
using HH_APICustomization.Descriptor;
using HH_APICustomization.Graph;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.SM;
using PX.TM;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMRemittance")]
    [PXPrimaryGraph(typeof(LUMCloudBedRemitTransactionProcess))]
    public class LUMRemittance : IBqlTable, PX.Data.EP.IAssign
    {
        public class PK : PrimaryKeyOf<LUMRemittance>.By<refNbr>
        {
            public static LUMRemittance Find(PXGraph graph, string refNbr) => FindBy(graph, refNbr);
        }

        #region RefNbr
        [PXDefault]
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXUIField(DisplayName = "Ref Nbr.", Required = true)]
        [AutoNumber(typeof(LUMHHSetup.remitSequenceID), typeof(AccessInfo.businessDate))]
        [PXSelector(typeof(Search<LUMRemittance.refNbr>),
            typeof(LUMRemittance.branch),
            typeof(LUMRemittance.date),
            typeof(LUMRemittance.status))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXUIField(DisplayName = "Date")]
        [PXDefault(typeof(AccessInfo.businessDate))]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Shift
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXDefault]
        [PXUIField(DisplayName = "Shift")]
        [PXStringList(new string[] { "AM", "PM", "GY", "RESA", "RESA GY" },
                      new string[] { "AM", "PM", "GY", "RESA", "RESA GY" })]
        public virtual string Shift { get; set; }
        public abstract class shift : PX.Data.BQL.BqlString.Field<shift> { }
        #endregion

        #region Branch
        [Branch(typeof(APRegister.branchID))]
        [PXDefault(typeof(AccessInfo.branchID))]
        [PXUIField(DisplayName = "Branch")]
        public virtual int? Branch { get; set; }
        public abstract class branch : PX.Data.BQL.BqlInt.Field<branch> { }
        #endregion

        #region Status
        [PXDBString(1, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Status")]
        [LUMRemitStatus.List]
        [PXDefault("H")]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region BatchNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Batch Nbr")]
        public virtual string BatchNbr { get; set; }
        public abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }
        #endregion

        #region VoidedBy
        [PXDBGuid]
        [PXUIField(DisplayName = "Voided By")]
        [PXSelector(typeof(Search<Users.pKID>),
                    SubstituteKey = typeof(Users.username))]
        public virtual Guid? VoidedBy { get; set; }
        public abstract class voidedBy : PX.Data.BQL.BqlGuid.Field<voidedBy> { }
        #endregion

        #region VoidReason
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Void Reason")]
        public virtual string VoidReason { get; set; }
        public abstract class voidReason : PX.Data.BQL.BqlString.Field<voidReason> { }
        #endregion

        #region VoidBatchNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Void Batch Nbr")]
        public virtual string VoidBatchNbr { get; set; }
        public abstract class voidBatchNbr : PX.Data.BQL.BqlString.Field<voidBatchNbr> { }
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
        [PXDefault(typeof(workgroupID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXCompanyTreeSelector]
        [PXUIField(DisplayName = "Workgroup", Enabled = false)]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : PX.Data.BQL.BqlInt.Field<workgroupID> { }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = "Owner", Enabled = true)]
        [PXDefault(typeof(Search<CREmployee.defContactID, Where<CREmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : PX.Data.BQL.BqlInt.Field<ownerID> { }
        #endregion

        #region IsApprover
        [PXBool]
        public virtual bool? IsApprover { get; set; }
        public abstract class isApprover : PX.Data.BQL.BqlBool.Field<isApprover> { }
        #endregion

        #region PostingDate
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Posting Date")]
        public virtual DateTime? PostingDate { get; set; }
        public abstract class postingDate : PX.Data.BQL.BqlDateTime.Field<postingDate> { }
        #endregion

        #region ADRemark
        [PXString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "ADRemark")]
        public virtual string ADRemark { get; set; }
        public abstract class aDRemark : PX.Data.BQL.BqlString.Field<aDRemark> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region RoomRevenue
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "RoomRevenue", Enabled = false)]
        public virtual decimal? RoomRevenue { get; set; }
        public abstract class roomRevenue : PX.Data.BQL.BqlDecimal.Field<roomRevenue> { }
        #endregion

        #region AdjRoomRevenue
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Adj RoomRevenue", Enabled = false)]
        public virtual decimal? AdjRoomRevenue { get; set; }
        public abstract class adjRoomRevenue : PX.Data.BQL.BqlDecimal.Field<adjRoomRevenue> { }
        #endregion

        #region CalcRoomRevenue
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Calc RoomRevenue", Enabled = false)]
        public virtual decimal? CalcRoomRevenue { get; set; }
        public abstract class calcRoomRevenue : PX.Data.BQL.BqlDecimal.Field<calcRoomRevenue> { }
        #endregion

        #region Taxes
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Taxes", Enabled = false)]
        public virtual decimal? Taxes { get; set; }
        public abstract class taxes : PX.Data.BQL.BqlDecimal.Field<taxes> { }
        #endregion

        #region AdjTaxes
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "AdjTaxes", Enabled = false)]
        public virtual decimal? AdjTaxes { get; set; }
        public abstract class adjTaxes : PX.Data.BQL.BqlDecimal.Field<adjTaxes> { }
        #endregion

        #region CalcTaxes
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "CalcTaxes", Enabled = false)]
        public virtual decimal? CalcTaxes { get; set; }
        public abstract class calcTaxes : PX.Data.BQL.BqlDecimal.Field<calcTaxes> { }
        #endregion

        #region Other
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Other", Enabled = false)]
        public virtual decimal? Other { get; set; }
        public abstract class other : PX.Data.BQL.BqlDecimal.Field<other> { }
        #endregion

        #region AdjOther
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "AdjOther", Enabled = false)]
        public virtual decimal? AdjOther { get; set; }
        public abstract class adjOther : PX.Data.BQL.BqlDecimal.Field<adjOther> { }
        #endregion

        #region CalcOther
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "CalcOther", Enabled = false)]
        public virtual decimal? CalcOther { get; set; }
        public abstract class calcOther : PX.Data.BQL.BqlDecimal.Field<calcOther> { }
        #endregion

        #region Payment
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Payment", Enabled = false)]
        public virtual decimal? Payment { get; set; }
        public abstract class payment : PX.Data.BQL.BqlDecimal.Field<payment> { }
        #endregion

        #region Refund
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Refund", Enabled = false)]
        public virtual decimal? Refund { get; set; }
        public abstract class refund : PX.Data.BQL.BqlDecimal.Field<refund> { }
        #endregion
    }
}