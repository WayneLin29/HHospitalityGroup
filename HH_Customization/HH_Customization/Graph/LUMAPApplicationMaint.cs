using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;

namespace HH_Customization.Graph
{
    public class LUMAPApplicationMaint : PXGraph<LUMAPApplicationMaint>
    {

        public const string NO_EMPTY = "{0} can't be empty";
        bool isError = false;

        public PXCancel<LUMAPApplicationFilter> Cancel;

        public PXFilter<LUMAPApplicationFilter> Filter;

        #region Action
        #region UpdateVendor
        public PXAction<LUMAPApplicationFilter> updateVendor;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Update Vendor", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable UpdateVendor(PXAdapter adapter)
        {
            if (!CheckRequest()) return adapter.Get();
            var row = Filter.Current;
            PXLongOperation.StartOperation(this, () =>
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    var baseRegister = GetAPRegister(row.AppliedRefNbr);
                    var ppmRegister = GetAPRegister(row.ApplyingRefNbr);
                    baseRegister.VendorID = ppmRegister.VendorID;
                    baseRegister.VendorLocationID = ppmRegister.VendorLocationID;
                    this.Caches<APRegister>().Update(baseRegister);
                    this.Caches<APRegister>().PersistUpdated(baseRegister);

                    ts.Complete();
                }
            });
            return adapter.Get();
        }
        #endregion

        #region RemoveApplication
        public PXAction<LUMAPApplicationFilter> removeApplication;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Application", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable RemoveApplication(PXAdapter adapter)
        {
            var row = Filter.Current;
            if (!CheckRequest()) return adapter.Get();
            PXLongOperation.StartOperation(this, () =>
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    foreach (var apAdjust in GetAPAdjust(row.AppliedRefNbr, row.ApplyingRefNbr))
                    {
                        this.Caches<APAdjust>().Delete(apAdjust);
                        this.Caches<APAdjust>().PersistDeleted(apAdjust);
                    }
                    ts.Complete();
                }
            });
            return adapter.Get();
        }
        #endregion
        #endregion

        #region Method
        public virtual bool CheckRequest()
        {
            var row = Filter.Current;
            if (row.ApplyingRefNbr == null)
                SetError<LUMAPApplicationFilter.applyingRefNbr>(Filter.Cache, row, row.ApplyingRefNbr, String.Format(NO_EMPTY, "Applying RefNbr"));
            if (row.AppliedRefNbr == null)
                SetError<LUMAPApplicationFilter.appliedRefNbr>(Filter.Cache, row, row.AppliedRefNbr, String.Format(NO_EMPTY, "Applied RefNbr"));
            return !isError;
        }

        private void SetError<T>(PXCache cache, object row, object data, string message) where T : IBqlField
        {
            isError = true;
            cache.RaiseExceptionHandling<T>(row, data, new PXException(message, PXErrorLevel.Error));
        }
        #endregion

        #region BQL
        public List<APAdjust> GetAPAdjust(string adjdRefNbr, string adjgRefNbr)
        {
            return
                SelectFrom<APAdjust>.
                Where<APAdjust.curyAdjgAmt.IsEqual<Zero>.
                    And<APAdjust.adjdRefNbr.IsEqual<@P.AsString>.
                    And<APAdjust.adjgRefNbr.IsEqual<@P.AsString>>>>
                .View.Select(this, adjdRefNbr, adjgRefNbr)
                .RowCast<APAdjust>().ToList();
        }

        public APRegister GetAPRegister(string refNbr)
        {
            return
                SelectFrom<APRegister>.
                Where<APRegister.refNbr.IsEqual<@P.AsString>>
                .View.Select(this, refNbr);
        }
        #endregion

        #region Table
        [Serializable]
        [PXHidden]
        public class LUMAPApplicationFilter : IBqlTable
        {
            #region ApplyingRefNbr
            [PXString()]
            [PXUIField(DisplayName = "Applying RefNbr", Required = true)]
            [PXSelector(typeof(Search<APRegister.refNbr, Where<APRegister.status, Equal<APDocStatus.open>>>),
                typeof(APRegister.refNbr),
                typeof(APRegister.docType),
                typeof(APRegister.branchID),
                typeof(APRegister.vendorID),
                typeof(APRegister.vendorID_Vendor_acctName),
                typeof(APRegister.docDesc),
                typeof(APRegister.curyOrigDocAmt),
                typeof(APRegister.docBal),
                DescriptionField = typeof(APRegister.docDesc)
                )]
            public virtual string ApplyingRefNbr { get; set; }
            public abstract class applyingRefNbr : PX.Data.BQL.BqlString.Field<applyingRefNbr> { }
            #endregion

            #region AppliedRefNbr
            [PXString()]
            [PXUIField(DisplayName = "Applied RefNbr", Required = true)]
            [PXSelector(typeof(Search<APRegister.refNbr, Where<APRegister.status, Equal<APDocStatus.open>>>),
                typeof(APRegister.refNbr),
                typeof(APRegister.docType),
                typeof(APRegister.branchID),
                typeof(APRegister.vendorID),
                typeof(APRegister.vendorID_Vendor_acctName),
                typeof(APRegister.docDesc),
                typeof(APRegister.curyOrigDocAmt),
                typeof(APRegister.docBal),
                DescriptionField = typeof(APRegister.docDesc)
                )]
            public virtual string AppliedRefNbr { get; set; }
            public abstract class appliedRefNbr : PX.Data.BQL.BqlString.Field<appliedRefNbr> { }
            #endregion
        }

        #endregion
    }
}