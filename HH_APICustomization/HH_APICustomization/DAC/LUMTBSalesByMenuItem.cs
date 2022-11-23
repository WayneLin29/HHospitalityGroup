using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
  [Serializable]
  [PXCacheName("LUMTBSalesByMenuItem")]
  public class LUMTBSalesByMenuItem : IBqlTable
  {
    #region TBSalesItemID
    [PXDBInt(IsKey = true)]
    [PXUIField(DisplayName = "TBSales Item ID")]
    public virtual int? TBSalesItemID { get; set; }
    public abstract class tBSalesItemID : PX.Data.BQL.BqlInt.Field<tBSalesItemID> { }
    #endregion

    #region Fileid
    [PXDBGuid()]
    [PXUIField(DisplayName = "Fileid")]
    public virtual Guid? Fileid { get; set; }
    public abstract class fileid : PX.Data.BQL.BqlGuid.Field<fileid> { }
    #endregion

    #region FileName
    [PXDBString(255, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "File Name")]
    public virtual string FileName { get; set; }
    public abstract class fileName : PX.Data.BQL.BqlString.Field<fileName> { }
    #endregion

    #region BranchID
    [PXDBInt()]
    [PXUIField(DisplayName = "Branch ID")]
    public virtual int? BranchID { get; set; }
    public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
    #endregion

    #region Date
    [PXDBDate()]
    [PXUIField(DisplayName = "Date")]
    public virtual DateTime? Date { get; set; }
    public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
    #endregion

    #region IsImported
    [PXDBBool()]
    [PXUIField(DisplayName = "Is Imported")]
    public virtual bool? IsImported { get; set; }
    public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
    #endregion

    #region BatchNbr
    [PXDBString(15, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Batch Nbr")]
    public virtual string BatchNbr { get; set; }
    public abstract class batchNbr : PX.Data.BQL.BqlString.Field<batchNbr> { }
    #endregion

    #region LineNbr
    [PXDBInt()]
    [PXUIField(DisplayName = "Line Nbr")]
    public virtual int? LineNbr { get; set; }
    public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
    #endregion

    #region MenuItem
    [PXDBString(255, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Menu Item")]
    public virtual string MenuItem { get; set; }
    public abstract class menuItem : PX.Data.BQL.BqlString.Field<menuItem> { }
    #endregion

    #region SalesCategory
    [PXDBString(50, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Sales Category")]
    public virtual string SalesCategory { get; set; }
    public abstract class salesCategory : PX.Data.BQL.BqlString.Field<salesCategory> { }
    #endregion

    #region MenuGroup
    [PXDBString(50, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Menu Group")]
    public virtual string MenuGroup { get; set; }
    public abstract class menuGroup : PX.Data.BQL.BqlString.Field<menuGroup> { }
    #endregion

    #region MenuItemVoidQty
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Menu Item Void Qty")]
    public virtual Decimal? MenuItemVoidQty { get; set; }
    public abstract class menuItemVoidQty : PX.Data.BQL.BqlDecimal.Field<menuItemVoidQty> { }
    #endregion

    #region GrossSales
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Gross Sales")]
    public virtual Decimal? GrossSales { get; set; }
    public abstract class grossSales : PX.Data.BQL.BqlDecimal.Field<grossSales> { }
    #endregion

    #region Discounts
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Discounts")]
    public virtual Decimal? Discounts { get; set; }
    public abstract class discounts : PX.Data.BQL.BqlDecimal.Field<discounts> { }
    #endregion

    #region NetSales
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Net Sales")]
    public virtual Decimal? NetSales { get; set; }
    public abstract class netSales : PX.Data.BQL.BqlDecimal.Field<netSales> { }
    #endregion

    #region Tax1
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Tax1")]
    public virtual Decimal? Tax1 { get; set; }
    public abstract class tax1 : PX.Data.BQL.BqlDecimal.Field<tax1> { }
    #endregion

    #region Tax2
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Tax2")]
    public virtual Decimal? Tax2 { get; set; }
    public abstract class tax2 : PX.Data.BQL.BqlDecimal.Field<tax2> { }
    #endregion

    #region Tax3
    [PXDBDecimal()]
    [PXUIField(DisplayName = "Tax3")]
    public virtual Decimal? Tax3 { get; set; }
    public abstract class tax3 : PX.Data.BQL.BqlDecimal.Field<tax3> { }
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

    #region Noteid
    [PXNote()]
    public virtual Guid? Noteid { get; set; }
    public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
    #endregion
  }
}