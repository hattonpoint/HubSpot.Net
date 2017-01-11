using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace HubSpot.Net
{
    public class HubSpotDealModel
    {
        [JsonProperty("portalId")]
        public string PortalId { get; set; }

        [JsonProperty("dealId")]
        public string DealId { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("associations")]
        public HubSpotDealAssociationsModel Associations { get; set; }

        [JsonProperty("properties")]
        public HubSpotDealPropertiesModel Properties { get; set; }

        [JsonProperty("status")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string ErrorDescription { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class HubSpotDealAssociationsModel
    {
        [JsonProperty("associatedCompanyIds")]
        public string[] AssociatedCompanyIds { get; set; }

        [JsonProperty("associatedVids")]
        public string[] AssociatedContactIds { get; set; }

        [JsonProperty("associatedDealIds")]
        public string[] AssociatedDealIds { get; set; }
    }

    public class HubSpotDealPropertiesModel
    {
        [JsonProperty("amount")]
        public HubSpotPropertyModel Amount { get; set; }

        [JsonProperty("dealstage")]
        public HubSpotPropertyModel DealStage { get; set; }

        [JsonProperty("pipeline")]
        public HubSpotPropertyModel Pipeline { get; set; }

        [JsonProperty("closedate")]
        public HubSpotPropertyModel CloseDate { get; set; }

        [JsonProperty("createdate")]
        public HubSpotPropertyModel CreateDate { get; set; }

        [JsonProperty("hubspot_owner_id")]
        public HubSpotPropertyModel HubspotOwnerId { get; set; }

        [JsonProperty("hs_createdate")]
        public HubSpotPropertyModel HubSpotCreateDate { get; set; }

        [JsonProperty("dealtype")]
        public HubSpotPropertyModel DealType { get; set; }

        [JsonProperty("dealname")]
        public HubSpotPropertyModel DealName { get; set; }

        [JsonProperty("description")]
        public HubSpotPropertyModel DealDescription { get; set; }

    }

}