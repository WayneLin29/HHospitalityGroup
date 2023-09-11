using HH_APICustomization.DAC;
using HH_APICustomization.Entity;
using HH_APICustomization.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.APIHelper
{
    public static class CloudBedHelper
    {
        /// <summary> Get CloudBed Acceess Token and Write into Preference </summary>
        public static void GetAccessToeknByCode(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var preference = GetCloudBedAPIPreference();
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://hotels.cloudbeds.com/api/v1.1/access_token");
                    requestMessage.Content = new FormUrlEncodedContent(GetTokenFormBodyByCode(code));
                    HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                    var responseResult = response.Content.ReadAsStringAsync().Result;
                    var accessEntity = JsonConvert.DeserializeObject<CloudBed_TokenEntity>(responseResult);
                    if (accessEntity != null)
                    {
                        var graph = PXGraph.CreateInstance<LUMCloudBedPreferenceMaint>();
                        graph.APIPreference.Current = graph.APIPreference.Select();
                        graph.APIPreference.Current.OauthCODE = code;
                        graph.APIPreference.Current.AuthResponseMessage = responseResult.Length > 2048 ? responseResult.Substring(0, 2048) : responseResult;
                        graph.APIPreference.Current.AccessToken = accessEntity.access_token;
                        graph.APIPreference.Current.RefreshToken = accessEntity.refresh_token;
                        graph.APIPreference.Current.RefreshTokenExpiresTime = DateTime.Now.AddMonths(1);
                        graph.APIPreference.UpdateCurrent();
                        graph.Save.Press();
                    }
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError($"Get CloudBed AccessToken Fail ({ex.Message})");
                }
            }
        }

        /// <summary> Get CloudBed Acceess Token and Write into Preference </summary>
        public static string UpdateAccessToken()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var preference = GetCloudBedAPIPreference();
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://hotels.cloudbeds.com/api/v1.1/access_token");
                    requestMessage.Content = new FormUrlEncodedContent(GetTokenFormBodyByRefreshToken());
                    HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                    var accessEntity = JsonConvert.DeserializeObject<CloudBed_TokenEntity>(response.Content.ReadAsStringAsync().Result);
                    if (accessEntity != null)
                    {
                        var graph = PXGraph.CreateInstance<LUMCloudBedPreferenceMaint>();
                        graph.APIPreference.Current = graph.APIPreference.Select();
                        graph.APIPreference.Current.AccessToken = accessEntity.access_token;
                        graph.APIPreference.UpdateCurrent();
                        graph.Save.Press();
                    }
                    return accessEntity.access_token;
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError($"Get CloudBed AccessToken Fail ({ex.Message})");
                    return null;
                }
            }
        }

        /// <summary> Get TransactionData </summary>
        public static List<HH_APICustomization.Entity.Transaction> GetTransactionData(DateTime fromDate, DateTime toDate)
        {
            var accessToken = UpdateAccessToken();
            try
            {
                int pageNumber = 1;
                var transactionData = new List<HH_APICustomization.Entity.Transaction>();
                var url = $"https://hotels.cloudbeds.com/api/v1.1/getTransactions?includeDeleted=true&sortBy=transactionDateTime&orderBy=desc&pageSize=100&pageNumber={pageNumber}&modifiedFrom={fromDate.ToString("yyyy-MM-dd")}&modifiedTo={toDate.ToString("yyyy-MM-dd")}";
                PXTrace.WriteInformation($"Get Transaction Data : {url}");
                HttpResponseMessage response = SendAPIRequest(url, accessToken, HttpMethod.Get);
                var transactionEntity = JsonConvert.DeserializeObject<CloudBed_TransactionEntity>(response.Content.ReadAsStringAsync().Result);
                transactionData.AddRange(transactionEntity.data);
                var total = transactionEntity.total;
                // 重複抓取資料直到全抓
                if (transactionEntity.success && transactionEntity.count != total)
                {
                    int totalPage = total % 100 == 0 ? total / 100 : total / 100 + 1;
                    for (pageNumber = 2; pageNumber <= totalPage; pageNumber++)
                    {
                        url = $"https://hotels.cloudbeds.com/api/v1.1/getTransactions?includeDeleted=true&sortBy=transactionDateTime&orderBy=desc&pageSize=100&pageNumber={pageNumber}&modifiedFrom={fromDate.ToString("yyyy-MM-dd")}&modifiedTo={toDate.ToString("yyyy-MM-dd")}";
                        response = SendAPIRequest(url, accessToken, HttpMethod.Get);
                        transactionEntity = JsonConvert.DeserializeObject<CloudBed_TransactionEntity>(response.Content.ReadAsStringAsync().Result);
                        transactionData.AddRange(transactionEntity.data);
                    }
                }
                return transactionData;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex.Message);
                return null;
            }
        }

        /// <summary> Get Reservation </summary>
        public static List<HH_APICustomization.Entity.Reservation> GetReservationData(DateTime fromDate, DateTime toDate)
        {
            var accessToken = UpdateAccessToken();
            try
            {
                // 查詢Reservation 因沒給Property，故實區統一為UTC+0
                int pageNumber = 1;
                var reservationData = new List<HH_APICustomization.Entity.Reservation>();
                var url = $"https://hotels.cloudbeds.com/api/v1.1/getReservations?modifiedFrom={fromDate.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss")}&modifiedTo={toDate.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss")}&pageSize=100&pageNumber={pageNumber}";
                PXTrace.WriteInformation($"Get Reservation Data : {url}");
                HttpResponseMessage response = SendAPIRequest(url, accessToken, HttpMethod.Get);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new PXException(response.Content.ReadAsStringAsync().Result);
                var reservationEntity = JsonConvert.DeserializeObject<CloudBed_ReservationEntity>(response.Content.ReadAsStringAsync().Result);
                reservationData.AddRange(reservationEntity.data);
                int total = reservationEntity.total;
                // 重複抓取資料直到全抓
                if (reservationEntity.success && reservationEntity.count != total)
                {
                    int totalPage = total % 100 == 0 ? total / 100 : total / 100 + 1;
                    for (pageNumber = 2; pageNumber <= totalPage; pageNumber++)
                    {
                        url = url = $"https://hotels.cloudbeds.com/api/v1.1/getReservations?modifiedFrom={fromDate.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss")}&modifiedTo={toDate.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss")}&pageSize=100&pageNumber={pageNumber}";
                        response = SendAPIRequest(url, accessToken, HttpMethod.Get);
                        reservationEntity = JsonConvert.DeserializeObject<CloudBed_ReservationEntity>(response.Content.ReadAsStringAsync().Result);
                        reservationData.AddRange(reservationEntity.data);
                    }
                }
                return reservationData;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex.Message);
                return null;
            }
        }

        /// <summary> Get Reservation With Rate Details </summary>
        public static List<ReservationRateDetail> GetReservationWithRate(List<string> propertyidList, DateTime fromDate, DateTime toDate)
        {
            var accessToken = UpdateAccessToken();
            try
            {
                var tempJbList = new List<JObject>();
                List<ReservationRateDetail> result = new List<ReservationRateDetail>();
                foreach (var propertyID in propertyidList)
                {
                    int pageNumber = 1;
                    var url = $"https://hotels.cloudbeds.com/api/v1.1/getReservationsWithRateDetails?propertyID={propertyID}&modifiedFrom={fromDate.ToString("yyyy-MM-dd HH:mm:ss")}&modifiedTo={toDate.ToString("yyyy-MM-dd HH:mm:ss")}&pageSize=100&pageNumber={pageNumber}";
                    PXTrace.WriteInformation($"Get Reservation WithRate : {url}");
                    HttpResponseMessage response = SendAPIRequest(url, accessToken, HttpMethod.Get);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new PXException(response.Content.ReadAsStringAsync().Result);
                    var reservationJObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    AddJobjectManualy("data", reservationJObject, tempJbList);
                    int total = int.Parse(reservationJObject["total"].ToString());
                    // 重複抓取資料直到全抓
                    if (bool.Parse(reservationJObject["success"].ToString()) && int.Parse(reservationJObject["count"].ToString()) != total)
                    {
                        int totalPage = total % 100 == 0 ? total / 100 : total / 100 + 1;
                        for (pageNumber = 2; pageNumber <= totalPage; pageNumber++)
                        {
                            url = $"https://hotels.cloudbeds.com/api/v1.1/getReservationsWithRateDetails?propertyID={propertyID}&modifiedFrom={fromDate.ToString("yyyy-MM-dd HH:mm:ss")}&modifiedTo={toDate.ToString("yyyy-MM-dd HH:mm:ss")}&pageSize=100&pageNumber={pageNumber}";
                            response = SendAPIRequest(url, accessToken, HttpMethod.Get);
                            reservationJObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            AddJobjectManualy("data", reservationJObject, tempJbList);
                        }
                    }
                }
                // 排除 RoomRate = null 的資料
                foreach (var element in tempJbList.ToList())
                {
                    if (element["detailedRates"].Count() != 0)
                        result.Add(element.ToObject<ReservationRateDetail>());
                }
                return result;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex.Message);
                return null;
            }
        }

        public static HttpResponseMessage SendAPIRequest(string url, string accessToken, HttpMethod httpMethod)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, url);
                HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();
                return response;
            }
        }

        /// <summary> Get OAuth2 Token FormBody By CODE</summary>
        public static Dictionary<string, string> GetTokenFormBodyByCode(string code)
        {
            var preference = GetCloudBedAPIPreference();
            return new Dictionary<string, string>()
            {
                {"grant_type", "authorization_code"},
                {"client_id", preference?.ClientID},
                {"client_secret", preference?.ClientSecret},
                {"redirect_uri",preference?.WebHookUrl},
                {"code",code}
            };
        }

        /// <summary> Get OAuth2 Token FormBody By Refresh_Token</summary>
        public static Dictionary<string, string> GetTokenFormBodyByRefreshToken()
        {
            var preference = GetCloudBedAPIPreference();
            return new Dictionary<string, string>()
            {
                {"grant_type", "refresh_token"},
                {"client_id", preference.ClientID},
                {"client_secret", preference.ClientSecret},
                {"refresh_token",preference.RefreshToken},
            };
        }

        private static LUMCloudBedAPIPreference GetCloudBedAPIPreference()
            => SelectFrom<LUMCloudBedAPIPreference>.View.Select(new PX.Data.PXGraph()).TopFirst;

        private static List<JObject> AddJobjectManualy(string keyName, JObject sourceObject, List<JObject> destinationObject)
        {
            foreach (JObject item in sourceObject[keyName])
                destinationObject.Add(item);
            return destinationObject;
        }
    }
}
