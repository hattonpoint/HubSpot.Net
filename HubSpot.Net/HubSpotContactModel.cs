using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace HubSpot.Net
{
    public class HubSpotContactModel
    {
        [JsonProperty("addedAt")]
        public long AddedAt { get; set; }

        [JsonProperty("portal-id")]
        public string PortalId { get; set; }

        [JsonProperty("is-contact")]
        public bool IsContact { get; set; }        

        [JsonProperty("properties")]
        public Dictionary<string, HubSpotPropertyModel> Properties { get; set; }

        [JsonProperty("identity-profiles")]
        public List<HubSpotIdentityProfileModel> IdentityProfiles { get; set; }
    }

    public class HubSpotContactsModel
    {
        [JsonProperty("contacts")]
        public List<HubSpotContactModel> Contacts { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("vid-offset")]
        public string VidOffset { get; set; }

        [JsonProperty("time-offset")]
        public long TimeOffSet { get; set; }

        [JsonProperty("status")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string ErrorDescription { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class HubSpotUpdateContactModel
    {
        [JsonProperty("properties")]
        public List<HubSpotPropertyUpdateModel> Properties { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        public HubSpotUpdateContactModel()
        {
            Properties = new List<HubSpotPropertyUpdateModel>();
        }
    }
}