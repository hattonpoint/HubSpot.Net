using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;

namespace HubSpot.Net
{
    public class HubSpotNet : IHubSpotNet
    {
        private const string HubSpotBaseAuthUrl = "https://api.hubapi.com/oauth/v1/";
        private const string HubSpotBaseApiUrl = "https://api.hubapi.com/";
        private const string HubSpotBaseUrl = "https://api.hubapi.com/";
        private string HubSpotClientId { get; } = System.Configuration.ConfigurationManager.AppSettings["HubSpotClientId"];

        private string HubSpotClientSecret { get; } =
            System.Configuration.ConfigurationManager.AppSettings["HubSpotClientSecret"];


        public async Task<HubSpotAppTokenModel> CreateAppTokens(string code, string redirectUri)
        {
            // Use the code to get tokens from HubSpot
            var uri = new UriBuilder(HubSpotBaseAuthUrl + "token");
            var content = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", HubSpotClientId),
                    new KeyValuePair<string, string>("client_secret", HubSpotClientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("code", code)
                });

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(uri.Uri, content);

                var result = await response.Content.ReadAsStringAsync();
                var appTokenModel = JsonConvert.DeserializeObject<HubSpotAppTokenModel>(result);
                appTokenModel.StatusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error during token creation: {appTokenModel.Error} : {appTokenModel.ErrorDescription}");
                }
                return appTokenModel;
            }
        }

        public async Task<HubSpotRefreshTokenInfoModel> GetRefreshTokenInfo(string token)
        {
            var uri = new UriBuilder(HubSpotBaseAuthUrl + "refresh-tokens/" + token);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri.Uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode, "HubSpotNet error during refresh token retrieval");
                }
                var result = await response.Content.ReadAsStringAsync();
                var refreshTokenData = JsonConvert.DeserializeObject<HubSpotRefreshTokenInfoModel>(result);
                refreshTokenData.StatusCode = response.StatusCode;
                return refreshTokenData;
            }
        }

        public async Task DeleteRefreshToken(string token)
        {
            var uri = new UriBuilder(HubSpotBaseAuthUrl + "refresh-tokens/" + token);
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(uri.Uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode, "HubSpotNet error during delete token retrieval");
                }
            }
        }

        public async Task<HubSpotAppTokenModel> RefreshAccessToken(string token, string redirectUri)
        {
            // Use the code to get tokens from HubSpot
            var uri = new UriBuilder(HubSpotBaseAuthUrl + "token");
            var content = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("client_id", HubSpotClientId),
                    new KeyValuePair<string, string>("client_secret", HubSpotClientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("refresh_token", token)
                });

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(uri.Uri, content);

                var result = await response.Content.ReadAsStringAsync();
                var appTokenModel = JsonConvert.DeserializeObject<HubSpotAppTokenModel>(result);
                appTokenModel.StatusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error during token creation: {appTokenModel.Error} : {appTokenModel.ErrorDescription}");
                }
                return appTokenModel;
            }
        }

        public async Task<HubSpotDealModel> CreateDeal(string token, HubSpotCreateDealModel model)
        {
            var uri = new UriBuilder(HubSpotBaseUrl + "deals/v1/deal");
            var content = new StringContent(JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings
            { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(uri.Uri, content);

                var result = await response.Content.ReadAsStringAsync();
                var dealModel = JsonConvert.DeserializeObject<HubSpotDealModel>(result);

                dealModel.StatusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error during deal creation: {dealModel.Error} : {dealModel.ErrorDescription}");
                }

                return dealModel;
            }
        }

        public async Task<List<HubSpotContactModel>> GetRecentContacts(string apiKey, DateTime timeOffset)
        {
            var uri = new UriBuilder(HubSpotBaseUrl + "/contacts/v1/lists/recently_updated/contacts/recent?hapikey=" + apiKey);

            using (var client = new HttpClient())
            {
                var contactsModel = new HubSpotContactsModel();
                var contactList = new List<HubSpotContactModel>();
                var responseTimeOffset = DateTime.MaxValue;
                var firstPass = true;

                while (firstPass || (responseTimeOffset > timeOffset && contactsModel.HasMore))
                {
                    // append time and vid offset if not the first pass
                    if (!firstPass)
                    {
                        uri = new UriBuilder(HubSpotBaseUrl + "/contacts/v1/lists/recently_updated/contacts/recent?hapikey=" + apiKey +
                            "&vidOffset=" + contactsModel.VidOffset + "&timeOffset=" + contactsModel.TimeOffSet.ToString());
                    }

                    var response = await client.GetAsync(uri.Uri);
                    var result = await response.Content.ReadAsStringAsync();

                    contactsModel = JsonConvert.DeserializeObject<HubSpotContactsModel>(result);
                    
                    contactList.AddRange(contactsModel.Contacts);//add contacts to response object

                    contactsModel.StatusCode = response.StatusCode;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HubSpotException(response.StatusCode,
                            $"HubSpotNet error during deal creation: {contactsModel.Error} : {contactsModel.ErrorDescription}");
                    }

                    responseTimeOffset = Helpers.ConvertFromUnixTime(contactsModel.TimeOffSet);

                    if (firstPass)
                        firstPass = false;
                }

                return contactList;
            }
        }

        public async Task UpdateContact(string apiKey, HubSpotUpdateContactModel model)
        {
            var uri = new UriBuilder(HubSpotBaseUrl + "contacts/v1/contact/email/" + model.Email + "/profile?hapikey=" + apiKey);

            var content = new StringContent(JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings
            { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(uri.Uri, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error during deal creation: {response.ReasonPhrase}");
                }                
            }
        }
    }
}
