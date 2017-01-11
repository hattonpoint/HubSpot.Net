using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HubSpot.Net
{
    public class HubSpotPropertyModel
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("versions")]
        public HubSpotPropertyVersionModel[] Versions { get; set; }
    }

    public class HubSpotPropertyVersionModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("sourceVid")]
        public string[] SourceVid { get; set; }
    }
}
