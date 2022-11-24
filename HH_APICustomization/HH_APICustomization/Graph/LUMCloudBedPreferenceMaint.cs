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

        public PXFilter<AccountMappingFilter> acctFilter;

        [PXImport(typeof(LUMCloudBedAccountMapping))]
        public SelectFrom<LUMCloudBedAccountMapping>
              .Where<LUMCloudBedAccountMapping.cloudBedPropertyID.IsEqual<AccountMappingFilter.cloudBedPropertyID.AsOptional>
                                                                 .Or<AccountMappingFilter.cloudBedPropertyID.AsOptional.IsNull>>
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

    [Serializable]
    public class AccountMappingFilter : IBqlTable
    {
        #region CloudBedPropertyID
        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "PropertyID")]
        [PXSelector(typeof(Search<LUMCloudBedPreference.cloudBedPropertyID, Where<LUMCloudBedPreference.active, Equal<True>>>),
           typeof(LUMCloudBedPreference.branchID),
           typeof(LUMCloudBedPreference.debitAcct),
           typeof(LUMCloudBedPreference.debitSub),
           typeof(LUMCloudBedPreference.creditAcct),
           typeof(LUMCloudBedPreference.creditSub))]
        public virtual string CloudBedPropertyID { get; set; }
        public abstract class cloudBedPropertyID : PX.Data.BQL.BqlString.Field<cloudBedPropertyID> { } 
        #endregion
    }
}
