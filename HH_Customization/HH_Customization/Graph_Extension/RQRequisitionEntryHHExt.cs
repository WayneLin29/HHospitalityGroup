using PX.Data;
using PX.Objects.PO;

namespace PX.Objects.RQ
{

    public class RQRequisitionEntryHHExt : PXGraphExtension<RQRequisitionEntry>
    {
        #region IsActive
        public static bool IsActive()
        {
            return true;
        }
        #endregion

        #region Override
        public delegate void InsertRequestLineDelegate(RQRequestLine line, decimal selectQty, bool mergeLines);
        [PXOverride]
        public virtual void InsertRequestLine(RQRequestLine line, decimal selectQty, bool mergeLines, InsertRequestLineDelegate baseMethod)
        {
            baseMethod(line, selectQty, mergeLines);
            RQRequisitionLine item = Base.Lines.Current;
            RQRequest req = RQRequest.PK.Find(Base, line.OrderNbr);
            if (req.ShipDestType == POShippingDestination.Site)
            {
                Base.Lines.Cache.SetValueExt<RQRequisitionLine.siteID>(item, req.SiteID);
                Base.Lines.Update(item);
            }
        }
        #endregion

    }
}
