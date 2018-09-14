using System.Collections.Generic;
using Newtonsoft.Json;

namespace VstsPrTool.Clients
{
    public class VstsResponse<T>
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "value")]
        public IList<T> Values { get; set; }
    }

    public class GitRepository
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string defaultBranch { get; set; }
    }

    public class CreatePullReuqestResponse
    {
        public string pullRequestId { get; set; }
    }

}