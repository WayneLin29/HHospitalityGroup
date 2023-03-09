using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.SO;
using PX.Data.BQL.Fluent;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourGroup")]
    public class LUMTourGroup : IBqlTable
    {
        #region AutoNumber
        public const string NUMBERING_ID = "TOURGROUP";
        public class numberingID : PX.Data.BQL.BqlString.Constant<numberingID> { public numberingID() : base(NUMBERING_ID) { } }
        #endregion

        #region Key
        public class PK : PrimaryKeyOf<LUMTourGroup>.By<tourGroupNbr>
        {
            public static LUMTourGroup Find(PXGraph graph, string tourGroupNbr) => FindBy(graph, tourGroupNbr);
        }
        #endregion

        #region TourGroupNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tour Group Nbr")]
        [AutoNumber(typeof(Search<Numbering.numberingID,
            Where<Numbering.numberingID, Equal<numberingID>>>), typeof(AccessInfo.businessDate))]
        [PXSelector(typeof(Search<tourGroupNbr>),
                typeof(tourGroupNbr),
                typeof(description),
                typeof(branchID),
                typeof(tourTypeClassID),
                typeof(dateFrom),
                typeof(dateTo)
            )]
        public virtual string TourGroupNbr { get; set; }
        public abstract class tourGroupNbr : PX.Data.BQL.BqlString.Field<tourGroupNbr> { }
        #endregion

        #region TourTypeClassID
        [PXDBInt()]
        [PXUIField(DisplayName = "Tour Type Class ID",Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<LUMTourTypeClass.typeClassID,
            Where<LUMTourTypeClass.branchID, Equal<Current<branchID>>>>),
            typeof(LUMTourTypeClass.typeClassCD),
            typeof(LUMTourTypeClass.description),
            typeof(LUMTourTypeClass.branchID),
            typeof(LUMTourTypeClass.curyID),
            SubstituteKey = typeof(LUMTourTypeClass.typeClassCD),
            DescriptionField = typeof(LUMTourTypeClass.description)
            )]
        public virtual int? TourTypeClassID { get; set; }
        public abstract class tourTypeClassID : PX.Data.BQL.BqlInt.Field<tourTypeClassID> { }
        #endregion

        #region BranchID
        [Branch()]
        [PXDefault(typeof(Current<AccessInfo.branchID>), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region DateFrom
        [PXDBDate()]
        [PXUIField(DisplayName = "Date From", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
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

        #region Unbound

        #region RevenuePHP
        [PXDecimal]
        [PXUIField(DisplayName = "Revenue (PHP)", IsReadOnly = true)]
        [PXUnboundDefault()]
        public virtual decimal? RevenuePHP { get; set; }
        public abstract class revenuePHP : PX.Data.BQL.BqlDecimal.Field<revenuePHP> { }
        #endregion

        #region GrossProfitPHP
        [PXDecimal]
        [PXUIField(DisplayName = "Gross Profit (PHP)", IsReadOnly = true)]
        [PXUnboundDefault()]
        public virtual decimal? GrossProfitPHP { get; set; }
        public abstract class grossProfitPHP : PX.Data.BQL.BqlDecimal.Field<grossProfitPHP> { }
        #endregion

        #region CostPHP
        [PXDecimal]
        [PXUIField(DisplayName = "Cost (PHP)", IsReadOnly = true)]
        [PXUnboundDefault()]
        public virtual decimal? CostPHP { get; set; }
        public abstract class costPHP : PX.Data.BQL.BqlDecimal.Field<costPHP> { }
        #endregion

        #region GrossProfitPer
        [PXDecimal]
        [PXUIField(DisplayName = "Gross Profit %", IsReadOnly = true)]
        [PXUnboundDefault()]
        public virtual decimal? GrossProfitPer { get; set; }
        public abstract class grossProfitPer : PX.Data.BQL.BqlDecimal.Field<grossProfitPer> { }
        #endregion

        #endregion
    }
}