using System;
using PX.Data;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourGroup")]
    public class LUMTourGroup : IBqlTable
    {
        #region TourGroupNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tour Group Nbr")]
        public virtual string TourGroupNbr { get; set; }
        public abstract class tourGroupNbr : PX.Data.BQL.BqlString.Field<tourGroupNbr> { }
        #endregion

        #region TourTypeClassID
        [PXDBInt()]
        [PXUIField(DisplayName = "Tour Type Class ID")]
        public virtual int? TourTypeClassID { get; set; }
        public abstract class tourTypeClassID : PX.Data.BQL.BqlInt.Field<tourTypeClassID> { }
        #endregion

        #region BranchID
        [PXDBInt()]
        [PXUIField(DisplayName = "Branch ID")]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region DateFrom
        [PXDBDate()]
        [PXUIField(DisplayName = "Date From")]
        public virtual DateTime? DateFrom { get; set; }
        public abstract class dateFrom : PX.Data.BQL.BqlDateTime.Field<dateFrom> { }
        #endregion

        #region DateTo
        [PXDBDate()]
        [PXUIField(DisplayName = "Date To")]
        public virtual DateTime? DateTo { get; set; }
        public abstract class dateTo : PX.Data.BQL.BqlDateTime.Field<dateTo> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region TourGuide
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tour Guide")]
        public virtual string TourGuide { get; set; }
        public abstract class tourGuide : PX.Data.BQL.BqlString.Field<tourGuide> { }
        #endregion

        #region Helper
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Helper")]
        public virtual string Helper { get; set; }
        public abstract class helper : PX.Data.BQL.BqlString.Field<helper> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
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