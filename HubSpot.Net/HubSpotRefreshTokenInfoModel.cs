using Newtonsoft.Json;
using System.Net;

namespace HubSpot.Net
{
    public class HubSpotRefreshTokenInfoModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user")]
        public string UserEmail { get; set; }

        [JsonProperty("hub_domain")]
        public string HubDomain { get; set; }

        [JsonProperty("scopes")]
        public string[] Scopes { get; set; }

        [JsonProperty("hub_id")]
        public int HubId { get; set; }

        [JsonProperty("client_id")]
        public string AppClientId { get; set; }

        [JsonProperty("user_id")]
        public int HubSpotUserId { get; set; }

        public int UserId { get; set; }

        public HttpStatusCode StatusCode { get; set; }

    }
}
