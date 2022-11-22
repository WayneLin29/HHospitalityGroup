using HH_APICustomization.Graph;
using PX.Data;
using PX.Data.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using HH_APICustomization.APIHelper;

namespace HH_APICustomization.WebHook
{
    public class CloudBedWebHook : IWebhookHandler
    {
        public async Task<System.Web.Http.IHttpActionResult> ProcessRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var scope = GetAdminScope())
            {
                if (request.Method == HttpMethod.Get)
                {
                    var code = HttpUtility.ParseQueryString(request.RequestUri.Query).Get("CODE");
                    // Get Access Token By Code
                    if (!string.IsNullOrEmpty(code))
                        CloudBedHelper.GetAccessToeknByCode(code);
                }
            }
            return new OkResult(request);
        }

        private IDisposable GetAdminScope()
        {
            var userName = "admin";
            if (PXDatabase.Companies.Length > 0)
            {
                var company = PXAccess.GetCompanyName();
                if (string.IsNullOrEmpty(company))
                {
                    company = PXDatabase.Companies[0];
                }
                userName = userName + "@" + company;
            }
            return new PXLoginScope(userName);
        }
    }
}
