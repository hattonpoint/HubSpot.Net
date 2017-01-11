using Newtonsoft.Json;
using System.Net;

namespace HubSpot.Net
{
    public class HubSpotAppTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int AccessTokenExpirationSeconds { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        public HttpStatusCode StatusCode { get; set; }


    }
}
