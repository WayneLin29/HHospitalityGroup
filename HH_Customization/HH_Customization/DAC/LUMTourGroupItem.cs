using System;
using PX.Data;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourGroupItem")]
    public class LUMTourGroupItem : IBqlTable
    {
        #region TourGroupNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tour Group Nbr")]
        public virtual string TourGroupNbr { get; set; }
        public abstract class tourGroupNbr : PX.Data.BQL.BqlString.Field<tourGroupNbr> { }
        #endregion

        #region TourGroupItemID
        [PXDBIdentity(IsKey = true)]
        public virtual int? TourGroupItemID { get; set; }
        public abstract class tourGroupItemID : PX.Data.BQL.BqlInt.Field<tourGroupItemID> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region ExtCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ext Cost")]
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
        #endregion

        #region CuryID
        [PXDBString(5, IsFixed = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "CuryID")]
        public virtual string CuryID { get; set; }
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        #endregion

        #region AccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "Account ID")]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubID
        [PXDBInt()]
        [PXUIField(DisplayName = "SubID")]
        public virtual int? SubID { get; set; }
        public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }
        #endregion

        #region PayAccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "Pay Account ID")]
        public virtual int? PayAccountID { get; set; }
        public abstract class payAccountID : PX.Data.BQL.BqlInt.Field<payAccountID> { }
        #endregion

        #region PaySubID
        [PXDBInt()]
        [PXUIField(DisplayName = "PaySubID")]
        public virtual int? PaySubID { get; set; }
        public abstract class paySubID : PX.Data.BQL.BqlInt.Field<paySubID> { }
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