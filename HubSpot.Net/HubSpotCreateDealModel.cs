using Newtonsoft.Json;

namespace HubSpot.Net
{
    public class HubSpotCreateDealModel
    {
        [JsonProperty("associations")]
        public HubSpotCreateDealAssociationsModel Associations { get; set; }

        [JsonProperty("portalId")]
        public string PortalId { get; set; }

        [JsonProperty("properties")]
        public HubSpotPropertyCreateModel[] Properties { get; set; }
    }

    public class HubSpotCreateDealAssociationsModel
    {
        [JsonProperty("associatedCompanyIds")]
        public string[] AssociatedCompanyIds { get; set; }

        [JsonProperty("associatedVids")]
        public string[] AssociatedContactIds { get; set; }

        [JsonProperty("associatedDealIds")]
        public string[] AssociatedDealIds { get; set; }
    }


    public class HubSpotPropertyCreateModel
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class HubSpotPropertyUpdateModel
    {
        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }        
    }
}
