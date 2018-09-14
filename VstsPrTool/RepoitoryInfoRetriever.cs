using VstsPrTool.Clients;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VstsPrTool.Utilities;

namespace VstsPrTool
{
    public class RepoitoryInfoRetriever
    {
        private readonly GitClient _gitClient;
        private readonly VstsClient _vstsClient;

        public RepoitoryInfoRetriever(GitClient gitClient, VstsClient vstsClient)
        {
            _gitClient = gitClient;
            _vstsClient = vstsClient;
        }

        public async Task<RepositoryInfo> GetAsync()
        {
            var repoInfo = new RepositoryInfo();
            string remoteOriginUrl = _gitClient.GetRemoteOriginUrl();
            Tracer.LogVerbose($"Got remote origin url from git: '{remoteOriginUrl}' - trying to parse repo name");

            Match match = Regex.Match(remoteOriginUrl, "(.*)/_git/(.*)");
            if (match.Success)
            {
                repoInfo.BaseUrl = new Uri(match.Groups[1].Value);
                repoInfo.RepositoryName = match.Groups[2].Value;
            }
            else
            {
                throw new Exception($"Can't parse the remote origin url {remoteOriginUrl}");
            }

            VstsResponse<GitRepository> repos = await _vstsClient.GetRepositoriesAsync(repoInfo.BaseUrl);
            GitRepository repo = repos.Values.FirstOrDefault(r => r.name == repoInfo.RepositoryName);
            if (repo == null)
            {
                throw new Exception($"Can't find repo with name {repoInfo.RepositoryName} in {repoInfo.BaseUrl}");
            }

            repoInfo.RepositoryId = repo.id;
            if (string.IsNullOrWhiteSpace(repo.defaultBranch))
            {
                throw new Exception($"Repo {repoInfo.RepositoryName} doesn't have a default branch, can't automatically create a pull request.");
            }

            repoInfo.DefaultBranchRef = repo.defaultBranch;

            return repoInfo;
        }
    }
}