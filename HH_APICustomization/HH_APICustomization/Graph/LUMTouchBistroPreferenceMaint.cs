using System;
using HH_APICustomization.DAC;
using HH_APICustomization.Utils;
using PX.Data;

namespace HH_APICustomization.Graph
{
    public class LUMTouchBistroPreferenceMaint : PXGraph<LUMTouchBistroPreferenceMaint>
    {
        #region Action
        public PXSave<EmptyFilter> Save;
        public PXCancel<EmptyFilter> Cancel;
        #endregion

        #region View
        public PXFilter<EmptyFilter> MasterFilter;
        public PXSelect<LUMTouchBistroPreference> Preferences;

        public PXFilter<AccountMappingFilter> Filter;
        [PXImport(typeof(LUMTouchBistroAccountMapping))]
        public PXSelect<LUMTouchBistroAccountMapping,
            Where<LUMTouchBistroAccountMapping.restaurantID, Equal<Current2<AccountMappingFilter.restaurantID>>,
                Or<Current2<AccountMappingFilter.restaurantID>, IsNull>>,
            OrderBy<
                Asc<LUMTouchBistroAccountMapping.restaurantID,
                Asc<LUMTouchBistroAccountMapping.accountID,
                Asc<LUMTouchBistroAccountMapping.subAcctID,
                Asc<LUMTouchBistroAccountMapping.salesCategory,
                Asc<LUMTouchBistroAccountMapping.menuGroup,
                Asc<LUMTouchBistroAccountMapping.menuItem,
                Asc<LUMTouchBistroAccountMapping.reason,
                Asc<LUMTouchBistroAccountMapping.payAccount>>>>>>>>
                >> AccountMappings;
        #endregion

        #region Event
        #region LUMTouchBistroPreference
        protected virtual void _(Events.RowPersisting<LUMTouchBistroPreference> e)
        {
            if (e.Row == null) return;
            var item = GetByResturantCD(e.Row.RestaurantCD, e.Row.RestaurantID);
            if (item != null)
            {
                GraphUtil.SetError<LUMTouchBistroPreference.restaurantCD>(e.Cache, e.Row, e.Row.RestaurantCD, "Duplicated Restaurant ID.");
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
                    Where<LUMTouchBistroPreference.restaurantCD, Equal<Required<LUMTouchBistroPreference.restaurantCD>>,
                    And<LUMTouchBistroPreference.restaurantID, NotEqual<Required<LUMTouchBistroPreference.restaurantID>>>>>
                .Select(this, resturantCD, selfID);
        }

        #endregion

        #region static
        #region Method
        public static LUMTouchBistroAccountMapping GetSalesByMenuItemAcct(PXGraph graph,LUMTBTransactionSummary data) {
            LUMTouchBistroAccountMapping mapping = null;
            mapping = GetByMenuItem(graph,data.RestaurantID,data.MenuItem);
            if(mapping == null) mapping = GetBySalesCategory(graph, data.RestaurantID, data.SalesCategory);
            if (mapping == null) mapping = GetByMenuGroup(graph, data.RestaurantID, data.MenuGroup);
            return mapping;
        }

        public static LUMTouchBistroAccountMapping GetAccountsSummaryAcct(PXGraph graph, LUMTBTransactionSummary data)
        {
            LUMTouchBistroAccountMapping mapping = null;
            mapping = GetByPayAccount(graph, data.RestaurantID, data.AccountName);
            return mapping;
        }

        public static LUMTouchBistroAccountMapping GetPayOutsInsAcct(PXGraph graph, LUMTBTransactionSummary data)
        {
            LUMTouchBistroAccountMapping mapping = null;
            mapping = GetByReason(graph, data.RestaurantID, data.Reason);
            return mapping;
        }
        #endregion
        #region BQL
        private static LUMTouchBistroAccountMapping GetByMenuItem(PXGraph graph,int? restaurantID, string menuItem) {
            return PXSelect<LUMTouchBistroAccountMapping,
                Where<LUMTouchBistroAccountMapping.restaurantID, Equal<Required<LUMTouchBistroAccountMapping.restaurantID>>,
                And<LUMTouchBistroAccountMapping.menuItem,Equal<Required<LUMTouchBistroAccountMapping.menuItem>>>>>
                .Select(graph, restaurantID, menuItem);
        }

        private static LUMTouchBistroAccountMapping GetByMenuGroup(PXGraph graph, int? restaurantID, string menuGroup)
        {
            return PXSelect<LUMTouchBistroAccountMapping,
                Where<LUMTouchBistroAccountMapping.restaurantID, Equal<Required<LUMTouchBistroAccountMapping.restaurantID>>,
                And<LUMTouchBistroAccountMapping.menuGroup, Equal<Required<LUMTouchBistroAccountMapping.menuGroup>>>>>
                .Select(graph, restaurantID, menuGroup);
        }

        private static LUMTouchBistroAccountMapping GetBySalesCategory(PXGraph graph, int? restaurantID, string salesCategory)
        {
            return PXSelect<LUMTouchBistroAccountMapping,
                Where<LUMTouchBistroAccountMapping.restaurantID, Equal<Required<LUMTouchBistroAccountMapping.restaurantID>>,
                And<LUMTouchBistroAccountMapping.salesCategory, Equal<Required<LUMTouchBistroAccountMapping.salesCategory>>>>>
                .Select(graph, restaurantID, salesCategory);
        }

        private static LUMTouchBistroAccountMapping GetByPayAccount(PXGraph graph, int? restaurantID, string accountName)
        {
            return PXSelect<LUMTouchBistroAccountMapping,
                Where<LUMTouchBistroAccountMapping.restaurantID, Equal<Required<LUMTouchBistroAccountMapping.restaurantID>>,
                And<LUMTouchBistroAccountMapping.payAccount, Equal<Required<LUMTouchBistroAccountMapping.payAccount>>>>>
                .Select(graph, restaurantID, accountName);
        }

        private static LUMTouchBistroAccountMapping GetByReason(PXGraph graph, int? restaurantID, string reason)
        {
            return PXSelect<LUMTouchBistroAccountMapping,
                Where<LUMTouchBistroAccountMapping.restaurantID, Equal<Required<LUMTouchBistroAccountMapping.restaurantID>>,
                And<LUMTouchBistroAccountMapping.reason, Equal<Required<LUMTouchBistroAccountMapping.reason>>>>>
                .Select(graph, restaurantID, reason);
        }

        #endregion
        #endregion

        #region Table
        [Serializable]
        [PXCacheName("Empty Filter")]
        public class EmptyFilter : PXBqlTable, IBqlTable
        { }

        [Serializable]
        [PXCacheName("Account Mapping Filter")]
        public class AccountMappingFilter : PXBqlTable, IBqlTable
        {
            #region RestaurantID
            [PXInt()]
            [PXUIField(DisplayName = "Restauarant ID")]
            [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
            [PXSelector(typeof(Search<LUMTouchBistroPreference.restaurantID>),
                    typeof(LUMTouchBistroPreference.restaurantCD),
                    typeof(LUMTouchBistroPreference.branch),
                    typeof(LUMTouchBistroPreference.accountID),
                    typeof(LUMTouchBistroPreference.subAcctID),
                    typeof(LUMTouchBistroPreference.active),
                    SubstituteKey = typeof(LUMTouchBistroPreference.restaurantCD),
                    DescriptionField = typeof(LUMTouchBistroPreference.branch)
                )]
            public virtual int? RestaurantID { get; set; }
            public abstract class restaurantID : PX.Data.BQL.BqlInt.Field<restaurantID> { }
            #endregion
        }
        #endregion

    }
}