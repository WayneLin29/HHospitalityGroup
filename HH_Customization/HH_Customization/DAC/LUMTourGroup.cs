using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.SO;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using System.Collections.Generic;
using HH_Customization.Graph;
using System.Linq;
using PX.Objects.AP;
using HH_Customization.Descriptor;

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
        [PXUIField(DisplayName = "Tour Type Class ID", Required = true)]
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
        [DACFieldDefault(typeof(LUMTourGroup), "RevenuePHPDefault")]
        public virtual decimal? RevenuePHP { get; set; }
        public abstract class revenuePHP : PX.Data.BQL.BqlDecimal.Field<revenuePHP> { }
        #endregion

        #region CostPHP
        [PXDecimal]
        [PXUIField(DisplayName = "Cost (PHP)", IsReadOnly = true)]
        [DACFieldDefault(typeof(LUMTourGroup), "CostPHPDefault")]
        public virtual decimal? CostPHP { get; set; }
        public abstract class costPHP : PX.Data.BQL.BqlDecimal.Field<costPHP> { }
        #endregion

        #region GrossProfitPHP
        [PXDecimal]
        [PXUIField(DisplayName = "Gross Profit (PHP)", IsReadOnly = true)]
        [DACFieldDefault(typeof(LUMTourGroup), "GrossProfitPHPDefault")]

        public virtual decimal? GrossProfitPHP { get; set; }
        public abstract class grossProfitPHP : PX.Data.BQL.BqlDecimal.Field<grossProfitPHP> { }
        #endregion

        #region GrossProfitPer
        [PXDecimal]
        [PXUIField(DisplayName = "Gross Profit %", IsReadOnly = true)]
        [DACFieldDefault(typeof(LUMTourGroup), "GrossProfitPerDefault")]
        public virtual decimal? GrossProfitPer { get; set; }
        public abstract class grossProfitPer : PX.Data.BQL.BqlDecimal.Field<grossProfitPer> { }
        #endregion

        #endregion

        #region Method
        #region RevenuePHP Default
        public static void RevenuePHPDefault(PXCache sender, PXFieldDefaultingEventArgs e) {
            LUMTourGroup row = (LUMTourGroup)e.Row;
            PXGraph graph = new PXGraph();
            //因剛仔入畫面Guests尚未載入資料，造成資料為空，改為BQL查詢
            var groupBy = GetGuest(graph, row.TourGroupNbr).GroupBy(d => new { d.SOOrderNbr, d.SOOrderType });
            decimal total = 0m;
            foreach (var group in groupBy)
            {
                SOOrder order = SOOrder.PK.Find(graph, group.Key.SOOrderType, group.Key.SOOrderNbr);
                total += (order?.CuryOrderTotal ?? 0m);
            }
            e.NewValue = total;
        }
        #endregion

        #region CostPHP Default
        public static void CostPHPDefault(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            LUMTourGroup row = (LUMTourGroup)e.Row;
            PXGraph graph = new PXGraph();
            decimal total = 0m;
            List<LUMTourGroupItem> groupItems = SelectFrom<LUMTourGroupItem>
                    .Where<LUMTourGroupItem.tourGroupNbr.IsEqual<@P.AsString>>
                    .View.Select(graph, row.TourGroupNbr)
                    .RowCast<LUMTourGroupItem>().ToList();
            List<LUMTourGuest> guests = GetGuest(graph, row.TourGroupNbr);
            //AP by GroupItem
            var groupItemByAP = groupItems.GroupBy(d => new { d.APRefNbr, d.APDocType });
            foreach (var ap in groupItemByAP)
            {
                APInvoice invoice = APInvoice.PK.Find(graph, ap.Key.APDocType, ap.Key.APRefNbr);
                total += (invoice?.OrigDocAmt ?? 0m);
            }
            #region By SO
            var soGroup = guests.GroupBy(d => new { d.SOOrderNbr, d.SOOrderType });
            foreach (var so in soGroup)
            {
                //AP by SO Item
                var itemByAp = SelectFrom<LUMTourItem>
                    .Where<LUMTourItem.sOOrderNbr.IsEqual<@P.AsString>
                    .And<LUMTourItem.sOOrderType.IsEqual<@P.AsString>>>
                    .View.Select(graph, so.Key.SOOrderNbr,so.Key.SOOrderType)
                    .RowCast<LUMTourItem>().ToList()
                    .GroupBy(d => new { d.APRefNbr, d.APDocType });
                foreach (var ap in itemByAp)
                {
                    APInvoice invoice = APInvoice.PK.Find(graph, ap.Key.APDocType, ap.Key.APRefNbr);
                    total += (invoice?.OrigDocAmt ?? 0m);
                }
                //AP by SO Reservation
                var reservationByAp = SelectFrom<LUMTourReservation>
                    .Where<LUMTourReservation.sOOrderNbr.IsEqual<@P.AsString>
                    .And<LUMTourReservation.sOOrderType.IsEqual<@P.AsString>>>
                    .View.Select(graph, so.Key.SOOrderNbr, so.Key.SOOrderType)
                    .RowCast<LUMTourReservation>().ToList()
                    .GroupBy(d => new { d.APRefNbr, d.APDocType });
                foreach (var ap in reservationByAp)
                {
                    APInvoice invoice = APInvoice.PK.Find(graph, ap.Key.APDocType, ap.Key.APRefNbr);
                    total += (invoice?.OrigDocAmt ?? 0m);
                }
                //AP by SO Flight
                var flightByAp = SelectFrom<LUMTourFlight>
                    .Where<LUMTourFlight.sOOrderNbr.IsEqual<@P.AsString>
                    .And<LUMTourFlight.sOOrderType.IsEqual<@P.AsString>>>
                    .View.Select(graph, so.Key.SOOrderNbr, so.Key.SOOrderType)
                    .RowCast<LUMTourFlight>().ToList().GroupBy(d => new { d.APRefNbr, d.APDocType });
                foreach (var ap in flightByAp)
                {
                    APInvoice invoice = APInvoice.PK.Find(graph, ap.Key.APDocType, ap.Key.APRefNbr);
                    total += (invoice?.OrigDocAmt ?? 0m);
                }
            }

            #endregion
            e.NewValue = total;
        }
        #endregion

        #region GrossProfitPHP Default
        public static void GrossProfitPHPDefault(PXCache sender, PXFieldDefaultingEventArgs e) {
            LUMTourGroup row = (LUMTourGroup)e.Row;
            e.NewValue = row.RevenuePHP - row.CostPHP;
        }
        #endregion

        #region GrossProfitPer Default
        public static void GrossProfitPerDefault(PXCache sender, PXFieldDefaultingEventArgs e) {
            LUMTourGroup row = (LUMTourGroup)e.Row;
            decimal revenuePHP = row.RevenuePHP ?? 0m;
            decimal grossProfitPHP = row.GrossProfitPHP ?? 0m;
            if (row.RevenuePHP == null || row.RevenuePHP == 0m) return;
            e.NewValue = Decimal.Round(100 * grossProfitPHP / revenuePHP, 2, MidpointRounding.AwayFromZero);
        }
        #endregion
        #endregion

        #region BQL
        private static List<LUMTourGuest> GetGuest(PXGraph graph,string tourGroupNbr) { 
            return SelectFrom<LUMTourGuest>
                    .Where<LUMTourGuest.tourGroupNbr.IsEqual<@P.AsString>>
                    .View.Select(graph, tourGroupNbr)
                    .RowCast<LUMTourGuest>().ToList();
        }
        #endregion
    }
}