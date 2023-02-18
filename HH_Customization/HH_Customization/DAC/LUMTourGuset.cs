using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CM.Extensions;
using PX.Objects.SO;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourGuset")]
    public class LUMTourGuset : IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourGuset>.By<tourGroupNbr, tourGusetID>
        {
            public static LUMTourGuset Find(PXGraph graph, string tourGroupNbr, int? tourGusetID) => FindBy(graph, tourGroupNbr, tourGusetID);
        }
        #endregion


        #region TourGroupNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tour Group Nbr")]
        [PXDBDefault(typeof(LUMTourGroup.tourGroupNbr))]
        [PXParent(typeof(Select<LUMTourGroup, Where<LUMTourGroup.tourGroupNbr, Equal<Current<tourGroupNbr>>>>))]
        public virtual string TourGroupNbr { get; set; }
        public abstract class tourGroupNbr : PX.Data.BQL.BqlString.Field<tourGroupNbr> { }
        #endregion

        #region TourGusetID
        [PXDBIdentity(IsKey = true)]
        public virtual int? TourGusetID { get; set; }
        public abstract class tourGusetID : PX.Data.BQL.BqlInt.Field<tourGusetID> { }
        #endregion

        #region SubGroupID
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Sub GroupID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string SubGroupID { get; set; }
        public abstract class subGroupID : PX.Data.BQL.BqlString.Field<subGroupID> { }
        #endregion

        #region NameCH
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Chinese Name")]
        public virtual string NameCH { get; set; }
        public abstract class nameCH : PX.Data.BQL.BqlString.Field<nameCH> { }
        #endregion

        #region NameEN
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "English Name")]
        public virtual string NameEN { get; set; }
        public abstract class nameEN : PX.Data.BQL.BqlString.Field<nameEN> { }
        #endregion

        #region BirthDay
        [PXDBDate()]
        [PXUIField(DisplayName = "BirthDay", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual DateTime? BirthDay { get; set; }
        public abstract class birthDay : PX.Data.BQL.BqlDateTime.Field<birthDay> { }
        #endregion

        #region BaseRate
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Base Rate")]
        [PXDefault(typeof(Search<LUMTourTypeClass.baseRate,
            Where<LUMTourTypeClass.typeClassID, Equal<Current<LUMTourGroup.tourTypeClassID>>>>))]
        public virtual Decimal? BaseRate { get; set; }
        public abstract class baseRate : PX.Data.BQL.BqlDecimal.Field<baseRate> { }
        #endregion

        #region AdjAmt
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Adjustment")]
        public virtual Decimal? AdjAmt { get; set; }
        public abstract class adjAmt : PX.Data.BQL.BqlDecimal.Field<adjAmt> { }
        #endregion

        #region Total
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Total",IsReadOnly = true)]
        [PXFormula(typeof(Sub<baseRate, adjAmt>))]
        public virtual Decimal? Total { get; set; }
        public abstract class total : PX.Data.BQL.BqlDecimal.Field<total> { }
        #endregion

        #region NoteText
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Notes")]
        public virtual string NoteText { get; set; }
        public abstract class noteText : PX.Data.BQL.BqlString.Field<noteText> { }
        #endregion

        #region CuryID
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXUIField(DisplayName = "Currency", Required = true)]
        [PXDefault(typeof(Search<LUMTourTypeClass.curyID,
            Where<LUMTourTypeClass.typeClassID, Equal<Current<LUMTourGroup.tourTypeClassID>>>>),
            PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Currency.curyID))]
        public virtual string CuryID { get; set; }
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        #endregion

        #region SOOrderNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Nbr", IsReadOnly = true)]
        [PXSelector(typeof(SOOrder.orderType))]
        public virtual string SOOrderNbr { get; set; }
        public abstract class sOOrderNbr : PX.Data.BQL.BqlString.Field<sOOrderNbr> { }
        #endregion

        #region SOOrderType
        [PXDBString(2, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Type", IsReadOnly = true)]
        [PXSelector(typeof(Search<SOOrderType.orderType>),
            SubstituteKey = typeof(SOOrderType.descr)
            )]

        public virtual string SOOrderType { get; set; }
        public abstract class sOOrderType : PX.Data.BQL.BqlString.Field<sOOrderType> { }
        #endregion

        #region SOLineNbr
        [PXDBInt()]
        [PXUIField(DisplayName = "SOLine Nbr", IsReadOnly = true)]
        public virtual int? SOLineNbr { get; set; }
        public abstract class sOLineNbr : PX.Data.BQL.BqlInt.Field<sOLineNbr> { }
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

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion

        #region unbound
        #region Age
        [PXInt()]
        [PXUIField(DisplayName = "Age", IsReadOnly = true)]
        [PXUnboundDefault(typeof(Sub<DatePart<DatePart.year, Current<LUMTourGroup.dateFrom>>, DatePart<DatePart.year, Current<birthDay>>>))]
        public virtual int? Age { get; set; }
        public abstract class age : PX.Data.BQL.BqlInt.Field<age> { }
        #endregion

        #region Deposit
        [PXDecimal()]
        [PXUIField(DisplayName = "Deposit", IsReadOnly = true)]
        public virtual Decimal? Deposit { get; set; }
        public abstract class deposit : PX.Data.BQL.BqlDecimal.Field<deposit> { }
        #endregion

        #region FinalPayAmount
        [PXDecimal()]
        [PXUIField(DisplayName = "FinalPayAmount", IsReadOnly = true)]
        public virtual Decimal? FinalPayAmount { get; set; }
        public abstract class finalPayAmount : PX.Data.BQL.BqlDecimal.Field<finalPayAmount> { }
        #endregion
        #endregion
    }
}