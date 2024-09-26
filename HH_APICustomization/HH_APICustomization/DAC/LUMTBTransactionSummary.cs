using System;
using PX.Data;
using PX.Objects.GL;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMTBTransactionSummary")]
    public class LUMTBTransactionSummary : PXBqlTable, IBqlTable
    {
        #region Selected
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region TBseqID
        [PXDBIdentity(IsKey = true)]
        public virtual int? TBseqID { get; set; }
        public abstract class tBseqID : PX.Data.BQL.BqlInt.Field<tBseqID> { }
        #endregion

        #region DataType
        [PXDBString(2, IsFixed = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Data Type")]
        public virtual string DataType { get; set; }
        public abstract class dataType : PX.Data.BQL.BqlString.Field<dataType> { }
        #endregion

        #region FileID
        [PXDBGuid()]
        [PXUIField(DisplayName = "FileID")]
        public virtual Guid? FileID { get; set; }
        public abstract class fileID : PX.Data.BQL.BqlGuid.Field<fileID> { }
        #endregion

        #region FileName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "File Name")]
        public virtual string FileName { get; set; }
        public abstract class fileName : PX.Data.BQL.BqlString.Field<fileName> { }
        #endregion

        #region RestaurantID
        [PXDBInt()]
        [PXUIField(DisplayName = "Restaurant ID")]
        [PXSelector(typeof(Search<LUMTouchBistroPreference.restaurantID>),
                typeof(LUMTouchBistroPreference.restaurantCD),
                typeof(LUMTouchBistroPreference.branch),
                typeof(LUMTouchBistroPreference.accountID),
                typeof(LUMTouchBistroPreference.subAcctID),
                typeof(LUMTouchBistroPreference.active),
                SubstituteKey = typeof(LUMTouchBistroPreference.restaurantCD),
                DescriptionField = typeof(LUMTouchBistroPreference.branch)
            )]
        public virtual int? RestaurantID { get; set; }
        public abstract class restaurantID : PX.Data.BQL.BqlInt.Field<restaurantID> { }
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

        #region AccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "Account")]
        [PXSelector(typeof(Account.accountID),
            SubstituteKey = typeof(Account.accountCD),
            DescriptionField = typeof(Account.description))]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubID
        [PXDBInt()]
        [PXUIField(DisplayName = "Sub")]
        [PXSelector(typeof(Sub.subID),
            SubstituteKey = typeof(Sub.subCD),
            DescriptionField = typeof(Sub.description))]
        public virtual int? SubID { get; set; }
        public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }
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

        #region Voids
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Voids")]
        public virtual Decimal? Voids { get; set; }
        public abstract class voids : PX.Data.BQL.BqlDecimal.Field<voids> { }
        #endregion

        #region MenuItemQty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Menu Item Qty")]
        public virtual Decimal? MenuItemQty { get; set; }
        public abstract class menuItemQty : PX.Data.BQL.BqlDecimal.Field<menuItemQty> { }
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

        #region AccountName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Account Name")]
        public virtual string AccountName { get; set; }
        public abstract class accountName : PX.Data.BQL.BqlString.Field<accountName> { }
        #endregion

        #region Payments
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Payments")]
        public virtual Decimal? Payments { get; set; }
        public abstract class payments : PX.Data.BQL.BqlDecimal.Field<payments> { }
        #endregion

        #region Deposits
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Deposits")]
        public virtual Decimal? Deposits { get; set; }
        public abstract class deposits : PX.Data.BQL.BqlDecimal.Field<deposits> { }
        #endregion

        #region ChargedToAccount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Charged To Account")]
        public virtual Decimal? ChargedToAccount { get; set; }
        public abstract class chargedToAccount : PX.Data.BQL.BqlDecimal.Field<chargedToAccount> { }
        #endregion

        #region Subtotal
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Subtotal")]
        public virtual Decimal? Subtotal { get; set; }
        public abstract class subtotal : PX.Data.BQL.BqlDecimal.Field<subtotal> { }
        #endregion

        #region Tips
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Tips")]
        public virtual Decimal? Tips { get; set; }
        public abstract class tips : PX.Data.BQL.BqlDecimal.Field<tips> { }
        #endregion

        #region Total
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Total")]
        public virtual Decimal? Total { get; set; }
        public abstract class total : PX.Data.BQL.BqlDecimal.Field<total> { }
        #endregion

        #region Server
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Server")]
        public virtual string Server { get; set; }
        public abstract class server : PX.Data.BQL.BqlString.Field<server> { }
        #endregion

        #region DateTimestamp
        [PXDBDate()]
        [PXUIField(DisplayName = "Date Timestamp")]
        public virtual DateTime? DateTimestamp { get; set; }
        public abstract class dateTimestamp : PX.Data.BQL.BqlDateTime.Field<dateTimestamp> { }
        #endregion

        #region Reason
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reason")]
        public virtual string Reason { get; set; }
        public abstract class reason : PX.Data.BQL.BqlString.Field<reason> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region Register
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Register")]
        public virtual string Register { get; set; }
        public abstract class register : PX.Data.BQL.BqlString.Field<register> { }
        #endregion

        #region Comment
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Comment")]
        public virtual string Comment { get; set; }
        public abstract class comment : PX.Data.BQL.BqlString.Field<comment> { }
        #endregion

        #region ErrorMessage
        [PXDBString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "Error Message")]
        public virtual string ErrorMessage { get; set; }
        public abstract class errorMessage : PX.Data.BQL.BqlString.Field<errorMessage> { }
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

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion

        #region unbound
        [PXInt()]
        public virtual int? ProcessIndex { get; set; }
        public abstract class processIndex : PX.Data.BQL.BqlInt.Field<processIndex> { }
        #endregion
    }
}