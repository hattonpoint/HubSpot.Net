using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.Net
{
    public class HubSpotIdentityProfileModel
    {
        [JsonProperty("vid")]
        public int Vid { get; set; }

        [JsonProperty("saved-at-timestap")]
        public string SavedAtTimestamp { get; set; }

        [JsonProperty("deleted-changed-timestamp")]
        public int DeletedChangedTimestamp { get; set; }

        [JsonProperty("identities")]
        public List<HubSpotIdentity> Identities { get; set; }
    }

    public class HubSpotIdentity
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
