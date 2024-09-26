using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.Common;
using PX.Objects.Common.Bql;
using PX.Objects.CR;
using PX.Objects.IN;
using System.Collections;

namespace PX.Objects.PO
{
    public class POReceiptEntryHHExt : PXGraphExtension<POReceiptEntry>
    {
        #region View
        public PXFilter<POReceiptLineSetter> POSetterDialog;
        #endregion

        #region Action
        public PXAction<POReceipt> lineSetter;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Update Location", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable LineSetter(PXAdapter adapter)
        {
            var current = POSetterDialog.Current;
            if (POSetterDialog.AskExt() == WebDialogResult.OK)
            {
                foreach (POReceiptLine line in Base.transactions.Select())
                {
                    Base.transactions.Cache.SetValueExt<POReceiptLine.siteID>(line, current.SiteID);
                    Base.transactions.Cache.SetValueExt<POReceiptLine.locationID>(line, current.LocationID);
                    Base.transactions.Update(line);
                }
            }

            POSetterDialog.Cache.Clear();
            return adapter.Get();
        }
        #endregion

        #region Event
        protected virtual void _(Events.RowSelected<POReceipt> e,PXRowSelected baseMethod)
        {
            baseMethod?.Invoke(e.Cache,e.Args);
            if (e.Row == null) return;
            lineSetter.SetEnabled(e.Row.Status != POReceiptStatus.Released);
        }

        protected virtual void _(Events.FieldUpdated<POReceiptLineSetter,POReceiptLineSetter.siteID> e) {
            e.Cache.SetDefaultExt<POReceiptLineSetter.locationID>(e.Row);
        }
        #endregion

        #region Table
        public class POReceiptLineSetter : PXBqlTable, IBqlTable
        {
            #region SiteID
            [PXInt]
            [PXUIField(DisplayName = "Warehouse", Visibility = PXUIVisibility.Visible, FieldClass = "INSITE")]
            [PXSelector(
                typeof(Search<INSite.siteID, Where<INSite.active, Equal<True>>>),
                typeof(INSite.siteCD),
                typeof(INSite.descr),
                SubstituteKey = typeof(INSite.siteCD),
                DescriptionField = typeof(INSite.descr)
                )]
            public virtual int? SiteID { get; set; }
            public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
            #endregion

            #region LocationID
            [PXInt]
            [PXUIField(DisplayName = "Location")]
            [PXSelector(
                typeof(Search<INLocation.locationID, Where<INLocation.active,Equal<True>,
                    And<INLocation.siteID, Equal<Current<siteID>>>>>),
                typeof(INLocation.locationCD),
                typeof(INLocation.descr),
                SubstituteKey = typeof(INLocation.locationCD),
                DescriptionField = typeof(INLocation.descr)
                )]
            [PXUnboundDefault(typeof(Search<INLocation.locationID, Where<INLocation.siteID, Equal<Current<siteID>>>>))]
            public virtual int? LocationID { get; set; }
            public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
            #endregion
        }
        #endregion
    }
}
