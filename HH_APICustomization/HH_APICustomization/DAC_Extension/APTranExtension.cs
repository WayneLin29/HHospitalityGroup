using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.TX;
using PX.Objects.GL;
using PX.Data.BQL.Fluent;
using PX.CS;
using HH_APICustomization.Graph;

namespace PX.Objects.AP
{
    public class APTranExtension : PXCacheExtension<APTran>
    {
        #region UsrORTaxZone
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "OR TaxZone", Visibility = PXUIVisibility.Visible)]
        [PXDefault(typeof(APInvoice.taxZoneID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.descr), Filterable = true)]
        [PXFormula(typeof(Default<APInvoice.suppliedByVendorLocationID, APInvoice.suppliedByVendorID, APInvoice.vendorLocationID>))]
        public virtual string UsrORTaxZone { get; set; }
        public abstract class usrORTaxZone : PX.Data.BQL.BqlString.Field<usrORTaxZone> { }
        #endregion

        #region UsrORStatus
        [PXDBString]
        [PXDefault("A", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(SearchFor<CSAttributeDetail.valueID>
                           .Where<CSAttributeDetail.attributeID.IsEqual<ORSTATUSAttr>
                             .And<CSAttributeDetail.disabled.IsEqual<False>>>),
                    SubstituteKey = typeof(CSAttributeDetail.description))]
        [PXUIField(DisplayName = "OR Status", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string UsrORStatus { get; set; }
        public abstract class usrORStatus : PX.Data.BQL.BqlString.Field<usrORStatus> { }
        #endregion

        #region UsrORNumber
        [PXDBString]
        [PXUIField(DisplayName = "OR Number")]
        public virtual string UsrOrNumber { get; set; }
        public abstract class usrORNumber : PX.Data.BQL.BqlString.Field<usrORNumber> { }
        #endregion

        #region UsrORBranch2
        [PXSelector(typeof(Search<CSAttributeDetail.valueID,
                          Where<CSAttributeDetail.attributeID, Equal<IVBRANCHAttr>,
                            And<CSAttributeDetail.disabled, NotEqual<True>>>>),
                   typeof(CSAttributeDetail.description))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Branch")]
        public virtual string UsrORBranch2 { get; set; }
        public abstract class usrORBranch2 : PX.Data.BQL.BqlString.Field<usrORBranch2> { }
        #endregion

        #region UsrORDate
        [PXDBDate]
        [PXDefault(typeof(APInvoice.docDate), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Date")]
        public virtual DateTime? UsrORDate { get; set; }
        public abstract class usrORDate : PX.Data.BQL.BqlDateTime.Field<usrORDate> { }
        #endregion

        #region UsrORVendor
        [VendorActive(Visibility = PXUIVisibility.SelectorVisible,
                      DescriptionField = typeof(Vendor.acctName),
                      CacheGlobal = true,
                      Filterable = true)]
        [PXDefault(typeof(APInvoice.vendorID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Vendor")]
        public virtual int? UsrORVendor { get; set; }
        public abstract class usrORVendor : PX.Data.BQL.BqlInt.Field<usrORVendor> { }
        #endregion
    }

    public class ORSTATUSAttr : PX.Data.BQL.BqlString.Constant<ORSTATUSAttr>
    {
        public ORSTATUSAttr() : base("ORSTATUS") { }

    }
}
