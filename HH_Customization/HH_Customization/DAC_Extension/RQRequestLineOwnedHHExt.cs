using PX.Data;
using PX.Objects.IN;
using PX.Objects.RQ;

namespace PX.Objects.RQ
{
    public class RQRequestLineOwnedHHExt : PXCacheExtension<RQRequestLineOwned>
    {
        #region IsActive
        public static bool IsActive()
        {
            return true;
        }
        #endregion

        #region SiteID
        //[Site(DescriptionField = typeof(INSite.descr))]
        [PXInt]
        [PXUIField(DisplayName = "Warehouse", Visibility = PXUIVisibility.Visible, FieldClass = "INSITE")]
        [PXSelector(typeof(INSite.siteID),
            typeof(INSite.siteCD),
            typeof(INSite.descr),
            SubstituteKey = typeof(INSite.siteCD),
            DescriptionField = typeof(INSite.descr)
            )]
        [PXDBScalar(typeof(Search<RQRequest.siteID, Where<RQRequest.orderNbr, Equal<RQRequestLineOwned.orderNbr>>>))]
        public virtual int? SiteID { get; set; }
        public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
        #endregion
    }
}
