using System.Threading.Tasks;
using VstsPrTool.Clients;
using VstsPrTool.Utilities;

namespace VstsPrTool
{
    public class PullRequestCreator
    {
        private readonly RepoitoryInfoRetriever _repoitoryInfoRetriever;
        private readonly VstsClient _vstsClient;
        private readonly GitClient _gitClient;

        public PullRequestCreator(RepoitoryInfoRetriever repoitoryInfoRetriever, VstsClient vstsClient, GitClient gitClient)
        {
            _repoitoryInfoRetriever = repoitoryInfoRetriever;
            _vstsClient = vstsClient;
            _gitClient = gitClient;
        }

        public async Task<string> RunAsync(PullRequestOptions options)
        {
            RepositoryInfo repoInfo = await _repoitoryInfoRetriever.GetAsync();

            Tracer.LogInfo($"Found the following repo info: Base url {repoInfo.BaseUrl}, Name: {repoInfo.RepositoryName}");

            string currentBranch = _gitClient.GetCurrentBranch();
            Tracer.LogVerbose($"Current branch: {currentBranch}");

            string title = string.IsNullOrWhiteSpace(options.Title)
                ? $"Merge {currentBranch} to {repoInfo.DefaultBranchRef}"
                : options.Title;

            var prRequestMessage = new PrRequestMessage
            {
                Title = title,
                Description = string.IsNullOrWhiteSpace(options.Description) ? title : options.Description,
                SourceRefName = $"refs/heads/{currentBranch}",
                TargetRefName = repoInfo.DefaultBranchRef
            };

            Tracer.LogInfo($"Creating PR with title '{title}'");

            string pullRequestId = await _vstsClient.CreatePullRequestAsync(repoInfo, prRequestMessage);
            return $"{repoInfo.BaseUrl}/_git/{repoInfo.RepositoryName}/pullrequest/{pullRequestId}?_a=overview";
        }
    }
}