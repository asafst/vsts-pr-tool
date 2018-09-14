using System;

namespace VstsPrTool
{
    public class RepositoryInfo
    {
        public Uri BaseUrl { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryId { get; set; }
        public string DefaultBranchRef { get; set; }
    }
}
