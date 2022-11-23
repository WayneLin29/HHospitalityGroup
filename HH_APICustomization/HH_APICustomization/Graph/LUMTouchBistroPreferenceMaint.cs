using System;
using HH_APICustomization.DAC;
using HH_APICustomization.Utils;
using PX.Data;

namespace HH_APICustomization.Graph
{
    public class LUMTouchBistroPreferenceMaint : PXGraph<LUMTouchBistroPreferenceMaint>
    {
        #region Action
        public PXSave<LUMTouchBistroPreference> Save;
        public PXCancel<LUMTouchBistroPreference> Cancel;
        #endregion

        #region View
        public PXSelect<LUMTouchBistroPreference> Preferences;

        public PXFilter<AccountMappingFilter> Filter;
        [PXImport(typeof(LUMTouchBistroAccountMapping))]
        public PXSelect<LUMTouchBistroAccountMapping,
            Where<LUMTouchBistroAccountMapping.restauarantID, Equal<Current2<AccountMappingFilter.restauarantID>>,
                Or<Current2<AccountMappingFilter.restauarantID>, IsNull>>> AccountMappings;
        #endregion

        #region Event
        #region LUMTouchBistroPreference
        protected virtual void _(Events.RowPersisting<LUMTouchBistroPreference> e)
        {
            if (e.Row == null) return;
            // Acuminator disable once PX1045 PXGraphCreateInstanceInEventHandlers [Justification]
            var item = GetByResturantCD(e.Row.ResturantCD, e.Row.ResturantID);
            if (item != null)
            {
                GraphUtil.SetError<LUMTouchBistroPreference.resturantCD>(e.Cache, e.Row, e.Row.ResturantCD, "Duplicated Restaurant ID.");
            }

        }
        #endregion
        #region LUMTouchBistroAccountMapping
        #endregion
        #endregion

        #region BQL
        protected virtual LUMTouchBistroPreference GetByResturantCD(string resturantCD, int? selfID)
        {
            return PXSelect<LUMTouchBistroPreference,
                    Where<LUMTouchBistroPreference.resturantCD, Equal<Required<LUMTouchBistroPreference.resturantCD>>,
                    And<LUMTouchBistroPreference.resturantID, NotEqual<Required<LUMTouchBistroPreference.resturantID>>>>>
                .Select(this, resturantCD, selfID);
        }

        #endregion

        #region Table
        [Serializable]
        [PXCacheName("Account Mapping Filter")]
        public class AccountMappingFilter : IBqlTable
        {
            #region RestauarantID
            [PXInt()]
            [PXUIField(DisplayName = "Restauarant ID")]
            [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
            [PXSelector(typeof(Search<LUMTouchBistroPreference.resturantID>),
                    typeof(LUMTouchBistroPreference.resturantCD),
                    typeof(LUMTouchBistroPreference.branchID),
                    typeof(LUMTouchBistroPreference.cashAccountID),
                    typeof(LUMTouchBistroPreference.cashSubAcctID),
                    typeof(LUMTouchBistroPreference.active),
                    SubstituteKey = typeof(LUMTouchBistroPreference.resturantCD),
                    DescriptionField = typeof(LUMTouchBistroPreference.branchID)
                )]
            public virtual int? RestauarantID { get; set; }
            public abstract class restauarantID : PX.Data.BQL.BqlInt.Field<restauarantID> { }
            #endregion
        }
        #endregion

    }
}