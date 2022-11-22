using HH_APICustomization.APIHelper;
using HH_APICustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Graph
{
    public class LUMCloudBedPreferenceMaint : PXGraph<LUMCloudBedPreferenceMaint>
    {
        public PXSave<LUMCloudBedAPIPreference> Save;
        public PXCancel<LUMCloudBedAPIPreference> Cancel;

        public SelectFrom<LUMCloudBedAPIPreference>.View APIPreference;

        [PXImport(typeof(LUMCloudBedPreference))]
        public SelectFrom<LUMCloudBedPreference>.View CloudBedSetup;

        [PXImport(typeof(LUMCloudBedAccountMapping))]
        public SelectFrom<LUMCloudBedAccountMapping>
              .OrderBy<LUMCloudBedAccountMapping.accountID.Asc,
                       LUMCloudBedAccountMapping.subAccountID.Asc,
                       LUMCloudBedAccountMapping.cloudBedPropertyID.Asc,
                       LUMCloudBedAccountMapping.type.Asc,
                       LUMCloudBedAccountMapping.transCategory.Asc,
                       LUMCloudBedAccountMapping.transactionCode.Asc,
                       LUMCloudBedAccountMapping.houseAccount.Asc,
                       LUMCloudBedAccountMapping.description.Asc>.View AccountMapping;

        #region Action
        public PXAction<LUMCloudBedAPIPreference> signIn;
        [PXUIField(DisplayName = "SIGN IN", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual void SignIn()
        {
            var url = this.APIPreference.Current?.OauthUrl;
            throw new PXRedirectToUrlException(url, "CloudBed");
        }
        #endregion
    }
}
