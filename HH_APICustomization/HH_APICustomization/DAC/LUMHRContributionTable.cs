using System;
using HH_APICustomization.Descriptor;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMHRContributionTable")]
    public class LUMHRContributionTable : PXBqlTable, IBqlTable
    {
        #region Type
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region EffectiveDate
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Effective Date")]
        public virtual DateTime? EffectiveDate { get; set; }
        public abstract class effectiveDate : PX.Data.BQL.BqlDateTime.Field<effectiveDate> { }
        #endregion

        #region RangeFrom
        [PXDBDecimal(IsKey = true)]
        [PXUIField(DisplayName = "Range From")]
        public virtual decimal? RangeFrom { get; set; }
        public abstract class rangeFrom : PX.Data.BQL.BqlDecimal.Field<rangeFrom> { }
        #endregion

        #region RangeTo
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Range To")]
        public virtual decimal? RangeTo { get; set; }
        public abstract class rangeTo : PX.Data.BQL.BqlDecimal.Field<rangeTo> { }
        #endregion

        #region Method
        [LUMDDLAttribute("CALMETHOD")]
        [PXDBString(200, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Method")]
        public virtual string Method { get; set; }
        public abstract class method : PX.Data.BQL.BqlString.Field<method> { }
        #endregion

        #region Employer
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Employer")]
        public virtual decimal? Employer { get; set; }
        public abstract class employer : PX.Data.BQL.BqlDecimal.Field<employer> { }
        #endregion

        #region Employee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Employee")]
        public virtual decimal? Employee { get; set; }
        public abstract class employee : PX.Data.BQL.BqlDecimal.Field<employee> { }
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