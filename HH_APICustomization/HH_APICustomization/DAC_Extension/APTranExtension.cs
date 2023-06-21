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

namespace PX.Objects.AP
{
    public class APTranExtension : PXCacheExtension<APTran>
    {
        #region UsrORTaxZone
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "OR TaxZone", Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.descr), Filterable = true)]
        [PXFormula(typeof(Default<APInvoice.suppliedByVendorLocationID, APInvoice.suppliedByVendorID, APInvoice.vendorLocationID>))]
        public virtual string UsrORTaxZone { get; set; }
        public abstract class usrORTaxZone : PX.Data.BQL.BqlString.Field<usrORTaxZone> { }
        #endregion

        #region UsrORStatus
        [PXDBString]
        [PXDefault("A", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(SearchFor<CSAttributeDetail.valueID>
                           .Where<CSAttributeDetail.attributeID.IsEqual<ORSTATUSAttr>>),
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

        #region UsrORBranch
        [Branch(typeof(APRegister.branchID),PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Branch")]
        public virtual int? UsrORBranch { get; set; }
        public abstract class usrORBranch : PX.Data.BQL.BqlInt.Field<usrORBranch> { }
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
