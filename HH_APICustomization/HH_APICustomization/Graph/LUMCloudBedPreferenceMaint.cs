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

        public SelectFrom<LUMHHSetup>.View Setup;

        public SelectFrom<LUMRemitRequestApproval>.View RemitApproval;

        #region Action
        public PXAction<LUMCloudBedAPIPreference> signIn;
        [PXUIField(DisplayName = "SIGN IN", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual void SignIn()
        {
            var url = this.APIPreference.Current?.OauthUrl;
            throw new PXRedirectToUrlException(url, "CloudBed");
        }

        public PXAction<LUMCloudBedAPIPreference> subscribe;
        [PXUIField(DisplayName = "SUBSCRIBE", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual void Subscribe()
        {
            PXLongOperation.StartOperation(this, () =>
            {
                var preference = this.APIPreference.Current;
                var actionArry = new string[] { "created", "details_changed", "removed" };
                foreach (var item in CloudBedSetup.View.SelectMulti().RowCast<LUMCloudBedPreference>().Where(x => x.Selected ?? false))
                {
                    // Subscribe 3 actions
                    for (int i = 0; i < actionArry.Length; i++)
                    {
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("endpointUrl", preference?.WebHookUrl);
                        param.Add("object", "roomblock");
                        param.Add("action", actionArry[i]);
                        param.Add("propertyID", item?.CloudBedPropertyID);
                        var subscribeResult = CloudBedHelper.SubscribeClodbedWebhook(CloudBedHelper.UpdateAccessToken(), param);
                        item.SubscriptionID += subscribeResult?.data?.subscriptionID + ";";
                        item.SubscriptionError += subscribeResult?.message + ";";
                    }
                    this.CloudBedSetup.Cache.Update(item);
                }
                this.Save.Press();

            });
        }

        #endregion
    }

    [Serializable]
    public class AccountMappingFilter : PXBqlTable, IBqlTable
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
