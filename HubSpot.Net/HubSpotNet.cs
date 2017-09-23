using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Linq;

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

        public async Task<List<HubSpotContactModel>> GetRecentContacts(string apiKey, DateTime timeOffset, List<string> properties = null)
        {
            // add an array of query string params that can be iterated through            
            var uri = new UriBuilder(HubSpotBaseUrl + "/contacts/v1/lists/recently_updated/contacts/recent?hapikey=" + apiKey + "&count=100&propertyMode=value_and_history");

            if(properties != null && properties.Any())
            {
                uri.Query = uri.Query.Substring(1) + "&property=" + string.Join("&property=", properties);
            }
           
            using (var client = new HttpClient())
            {
                var contactsModel = new HubSpotContactsModel();
                var contactList = new List<HubSpotContactModel>();
                var responseTimeOffset = DateTime.MaxValue;
                var uriOriginalString = uri.Uri.ToString();
                var uriString = uriOriginalString;
                var firstPass = true;

                while (firstPass || contactsModel.HasMore)
                {
                    // append time and vid offset if not the first pass
                    if (!firstPass)
                    {
                        uri = new UriBuilder(HubSpotBaseUrl + "/contacts/v1/lists/recently_updated/contacts/recent?hapikey=" + apiKey + "&count=100&propertyMode=value_and_history"
                            + string.Format("&vidOffset={0}&timeOffset={1}", contactsModel.VidOffset, contactsModel.TimeOffSet));

                        if (properties != null && properties.Any())
                        {
                            uri.Query = uri.Query.Substring(1) + "&property=" + string.Join("&property=", properties);
                        }
                    }

                    var response = await client.GetAsync(uri.Uri);
                    var result = await response.Content.ReadAsStringAsync();
                    
                    contactsModel = JsonConvert.DeserializeObject<HubSpotContactsModel>(result);
                    
                    contactList.AddRange(contactsModel.Contacts.Where(c => Helpers.ConvertFromUnixTime(c.AddedAt) >= timeOffset));//check contact timestamp against lastSyncDate

                    contactsModel.StatusCode = response.StatusCode;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HubSpotException(response.StatusCode,
                            $"HubSpotNet error during deal creation: {contactsModel.Error} : {contactsModel.ErrorDescription}");
                    }

                    responseTimeOffset = Helpers.ConvertFromUnixTime(contactsModel.TimeOffSet);
                    if(responseTimeOffset > DateTime.UtcNow)
                    {
                        uriString = uriOriginalString + "&vidOffset=" + contactsModel.VidOffset + "&timeOffset=" + contactsModel.TimeOffSet.ToString();
                    }
                    else                    
                        uriString = string.Empty;

                    if (firstPass)
                        firstPass = false;
                }

                return contactList;
            }
        }

        public async Task<List<HubSpotContactModel>> GetContacts(string apiKey, List<string> properties)
        {
            // add an array of query string params that can be iterated through
            //https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=b6485fd4-41b3-406e-b925-a5a99309b68b&property=firstname&property=lastname&property=email
            //iterate through array of prioperties and append to url

            var uri = new UriBuilder(HubSpotBaseUrl + "contacts/v1/lists/all/contacts/all?hapikey=" + apiKey);
            foreach (var property in properties)
            {
                
            }
            

            using (var client = new HttpClient())
            {
                var contactsModel = new HubSpotContactsModel();
                var contactList = new List<HubSpotContactModel>();
                var responseTimeOffset = DateTime.MaxValue;
                var firstPass = true;

                while (firstPass || contactsModel.HasMore)
                {
                    // append time and vid offset if not the first pass
                    if (!firstPass)
                    {
                        uri = new UriBuilder(HubSpotBaseUrl + "contacts/v1/lists/all/contacts/all?hapikey=" + apiKey);                           
                    }

                    var response = await client.GetAsync(uri.Uri);
                    var result = await response.Content.ReadAsStringAsync();

                    contactsModel = JsonConvert.DeserializeObject<HubSpotContactsModel>(result);


                    //reutrn raw json

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
        
        public async Task<HubSpotContactModel> GetContact(string apiKey, string email)
        {
            var uri = new UriBuilder(string.Format(HubSpotBaseUrl + "contacts/v1/contact/email/{0}/profile?hapikey={1}", email, apiKey));

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri.Uri);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error contact creation: {response.ReasonPhrase}");
                }

                var contactModel = JsonConvert.DeserializeObject<HubSpotContactModel>(result);

                return contactModel;
            }            
        }

        public async Task UpdateContact(string apiKey, HubSpotUpdateContactModel model, string[] param)
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

        public async Task UpdateContacts(string apiKey, List<HubSpotUpdateContactModel> model)
        {
            var uri = new UriBuilder(HubSpotBaseUrl + "contacts/v1/contact/batch/?hapikey=" + apiKey);

            var content = new StringContent(JsonConvert.SerializeObject(model.ToArray(), Formatting.None, new JsonSerializerSettings
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

            throw new NotImplementedException();
        }

        public async Task BatchUpdateContactsByEmail(string apiKey, List<HubSpotUpdateContactModel> model)
        {
            var uri = new UriBuilder(HubSpotBaseUrl + "contacts/v1/contact/batch/?hapikey=" + apiKey);
            
            var modelArray = model.ToArray();

            var content = new StringContent(JsonConvert.SerializeObject(model.ToArray(), Formatting.None, new JsonSerializerSettings
            { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");

            using(var client = new HttpClient())
            {
                var response = await client.PostAsync(uri.Uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error during contacts batch update: {response.ReasonPhrase}");
                }
            }
        }

        public async Task DeleteContact(string apiKey, string vid)
        {            
            var uri = new UriBuilder(string.Format(HubSpotBaseUrl + "contacts/v1/contact/{0}/?hapikey={1}", vid, apiKey));

            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(uri.Uri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HubSpotException(response.StatusCode,
                        $"HubSpotNet error during deal creation: {response.ReasonPhrase}");
                }
            }
        }
    }
}
