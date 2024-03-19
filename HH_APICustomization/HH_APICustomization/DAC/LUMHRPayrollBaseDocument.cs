using System;
using HH_APICustomization.DAC;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.GL;

namespace HHAPICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMHRPayrollBaseDocument")]
    public class LUMHRPayrollBaseDocument : IBqlTable
    {

        public class PK : PrimaryKeyOf<LUMHRPayrollBaseDocument>.By<docRefNbr>
        {
            public static LUMHRPayrollBaseDocument Find(PXGraph graph, string docRefNbr) => FindBy(graph, docRefNbr);
        }

        #region DocRefNbr
        [PXDefault]
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXUIField(DisplayName = "Ref Nbr")]
        [AutoNumber(typeof(LUMHHSetup.postingSequenceID), typeof(AccessInfo.businessDate))]
        [PXSelector(typeof(Search<LUMHRPayrollBaseDocument.docRefNbr>),
                    typeof(LUMHRPayrollBaseDocument.branchID))]
        public virtual string DocRefNbr { get; set; }
        public abstract class docRefNbr : PX.Data.BQL.BqlString.Field<docRefNbr> { }
        #endregion

        #region BranchID
        [PXUIField(DisplayName = "Branch")]
        [PXDefault]
        [Branch(IsDetail = false, TabOrder = 0)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region ProcessTime
        [PXDBDateAndTime]
        [PXUIField(DisplayName = "Process Time", Enabled = false)]
        public virtual DateTime? ProcessTime { get; set; }
        public abstract class procssTime : PX.Data.BQL.BqlDateTime.Field<procssTime> { }
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