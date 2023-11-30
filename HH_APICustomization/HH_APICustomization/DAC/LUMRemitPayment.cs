using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMRemitPayment")]
    public class LUMRemitPayment : IBqlTable
    {
        #region RefNbr
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ref Nbr")]
        [PXDefault(typeof(LUMRemittance.refNbr))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Line Nbr")]
        [PXDefault]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region Description
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description", Enabled = false)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region RecordedAmt
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Recorded Amt", Enabled = false)]
        public virtual Decimal? RecordedAmt { get; set; }
        public abstract class recordedAmt : PX.Data.BQL.BqlDecimal.Field<recordedAmt> { }
        #endregion

        #region RemitAmt
        [PXDBDecimal()]
        //[PXDefault(0, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Remit Amt")]
        public virtual Decimal? RemitAmt { get; set; }
        public abstract class remitAmt : PX.Data.BQL.BqlDecimal.Field<remitAmt> { }
        #endregion

        #region OpenAmt
        [PXDecimal()]
        [PXFormula(typeof(Sub<LUMRemitPayment.recordedAmt, LUMRemitPayment.remitAmt>))]
        [PXUIField(DisplayName = "Open Amount", Enabled = false)]
        public virtual Decimal? OpenAmt { get; set; }
        public abstract class openAmt : PX.Data.BQL.BqlDecimal.Field<openAmt> { }
        #endregion

        #region OPRemark
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Remark")]
        public virtual string OPRemark { get; set; }
        public abstract class oPRemark : PX.Data.BQL.BqlString.Field<oPRemark> { }
        #endregion

        #region ADRemark
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Audit Remark")]
        public virtual string ADRemark { get; set; }
        public abstract class aDRemark : PX.Data.BQL.BqlString.Field<aDRemark> { }
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