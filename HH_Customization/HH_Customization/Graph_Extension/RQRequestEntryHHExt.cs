using System;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.PO;

namespace PX.Objects.RQ
{
    public class RQRequestEntryHHExt : PXGraphExtension<RQRequestEntry>
    {
        #region IsActive
        public static bool IsActive()
        {
            return true;
        }
        #endregion

        #region Event
        protected virtual void _(Events.FieldDefaulting<RQRequest, RQRequest.shipDestType> e,PXFieldDefaulting baseMethod)
        {
            baseMethod.Invoke(e.Cache,e.Args);
            if (e.Row == null) return;
            e.NewValue = POShippingDestination.Site;
        }
        #endregion
    }
}
