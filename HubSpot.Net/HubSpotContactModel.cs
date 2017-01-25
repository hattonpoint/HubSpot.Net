using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace HubSpot.Net
{
    public class HubSpotContactModel
    {
        [JsonProperty("portal-id")]
        public string PortalId { get; set; }

        [JsonProperty("is-contact")]
        public bool IsContact { get; set; }

        [JsonProperty("properties")]
        public HubSpotContactPropertiesModel Properties { get; set; }    
        
        [JsonProperty("identity-profiles")]
        public List<HubSpotIdentityProfileModel> IdentityProfiles { get; set; }
    }

    public class HubSpotContactPropertiesModel
    {
        [JsonProperty("email")]
        public HubSpotPropertyModel Email { get; set; }

        [JsonProperty("firstname")]
        public HubSpotPropertyModel FirstName { get; set; }

        [JsonProperty("lastmodifieddate")]
        public HubSpotPropertyModel LastModifedDate { get; set; }

        [JsonProperty("lastname")]
        public HubSpotPropertyModel LastName { get; set; }

        [JsonProperty("address")]
        public HubSpotPropertyModel Address { get; set; }

        [JsonProperty("city")]
        public HubSpotPropertyModel City { get; set; }

        [JsonProperty("state")]
        public HubSpotPropertyModel State { get; set; }

        [JsonProperty("zip")]
        public HubSpotPropertyModel Zip { get; set; }
        
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

        public string Email { get; set; }

        public HubSpotUpdateContactModel()
        {
            Properties = new List<HubSpotPropertyUpdateModel>();
        }
    }
}