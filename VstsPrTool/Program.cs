using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using VstsPrTool.Clients;
using VstsPrTool.Utilities;

namespace VstsPrTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => RunAsync(opts).GetAwaiter().GetResult());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to create PR, due to error: {e.Message}");
            }
        }

        private static async Task RunAsync(Options opts)
        {
            Tracer.SetVerbosity(opts.Verbose);
            var prOptions = new PullRequestOptions
            {
                Title = opts.Title,
                Description = opts.Description
            };

            string gitRepoPath = opts.GitRepositoryPath;
            if (string.IsNullOrWhiteSpace(gitRepoPath))
            {
                gitRepoPath = Directory.GetCurrentDirectory();
            }

            Tracer.LogVerbose($"Running PR creator with options {prOptions} on repo {gitRepoPath}");


            var gitClient = new GitClient(gitRepoPath);
            var vstsClient = new VstsClient();
            var repoInfoRetreiver = new RepoitoryInfoRetriever(gitClient, vstsClient);
            
            var prCreator = new PullRequestCreator(repoInfoRetreiver, vstsClient, gitClient);
            string prUrl = await prCreator.RunAsync(prOptions);

            Tracer.LogInfo($"PR created succesffully! PR url was copied to clipboard: {prUrl}");
            Clipboard.Copy(prUrl);
        }
    }

    public class Options
    {
        [Option('t', "title", Required = false, HelpText = "The pull request title.")]
        public string Title { get; set; }

        [Option('d', "description", Required = false, HelpText = "The pull request desription.")]
        public string Description { get; set; }

        [Option('r', "repo", Required = false, HelpText = "The git repository path.")]
        public string GitRepositoryPath { get; set; }

        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
}
