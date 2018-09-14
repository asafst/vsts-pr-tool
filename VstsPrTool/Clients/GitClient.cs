using System;
using System.Linq;
using LibGit2Sharp;

namespace VstsPrTool.Clients
{
    public class GitClient
    {
        private readonly string _gitRepoPath;
        
        public GitClient(string gitRepoPath)
        {
            _gitRepoPath = gitRepoPath;
            ValidateRepository();
        }

        private void ValidateRepository()
        {
            try
            {
                using (var repo = new Repository(_gitRepoPath))
                {
                    if (!repo.Config.Any(c =>
                        c.Key.Equals("remote.origin.url", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        throw new Exception("Can't find a remote origin for the current git repo");
                    }

                    if (!repo.Head.IsTracking)
                    {
                        throw new Exception("The current branch is not pushed to remote repository");
                    }
                }
            }
            catch (RepositoryNotFoundException)
            {
                throw new Exception("Repository not found");
            }
            
        }

        public string GetRemoteOriginUrl()
        {
            using (var repo = new Repository(_gitRepoPath))
            {
                ConfigurationEntry<string> remote = repo.Config.First(c =>
                    c.Key.Equals("remote.origin.url", StringComparison.InvariantCultureIgnoreCase));

                return remote.Value;
            }
        }

        public string GetCurrentBranch()
        {
            using (var repo = new Repository(_gitRepoPath))
            {
                return repo.Head.FriendlyName;
            }
        }
    }
}