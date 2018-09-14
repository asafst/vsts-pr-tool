using Newtonsoft.Json;

namespace VstsPrTool.Clients
{
    public class PrRequestMessage
    {
        [JsonProperty(PropertyName = "sourceRefName")]
        public string SourceRefName { get; set; }

        [JsonProperty(PropertyName = "targetRefName")]
        public string TargetRefName { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}