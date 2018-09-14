using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;

namespace VstsPrTool.Clients
{
    public class VstsClient
    {
        private const string VSTSResourceId = "499b84ac-1321-427f-aa17-267ca6975798";
        private static readonly AzureServiceTokenProvider AzureServiceTokenProvider = new AzureServiceTokenProvider();

        private static readonly Lazy<HttpClient> LazyHttpClient = new Lazy<HttpClient>(() => {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        });

        private static HttpClient HttpClient => LazyHttpClient.Value;

        private string _accessToken;

        public async Task<VstsResponse<GitRepository>> GetRepositoriesAsync(Uri repositoryBaseUrl)
        {
            return await this.CallVstsAsync<VstsResponse<GitRepository>>(HttpMethod.Get, repositoryBaseUrl, "repositories");
        }

        public async Task<string> CreatePullRequestAsync(RepositoryInfo repoInfo, PrRequestMessage prRequestMessage)
        {
            var prResponse = await this.CallVstsAsync<CreatePullReuqestResponse>(HttpMethod.Post,
                repoInfo.BaseUrl,
                $"repositories/{repoInfo.RepositoryId}/pullrequests",
                JsonConvert.SerializeObject(prRequestMessage));

            if (string.IsNullOrEmpty(prResponse.pullRequestId))
            {
                throw new Exception("Failed creating the PR");
            }

            return prResponse.pullRequestId;
        }

        public string CallGet()
        {
            throw new NotImplementedException();
        }

        public string CallPost()
        {
            throw new NotImplementedException();
        }

        private async Task<T> CallVstsAsync<T>(HttpMethod httpMethod, Uri repositoryBaseUrl, string apiPath, string body = null)
        {
            HttpRequestMessage request = await this.InitializeRequestMessageAsync(httpMethod, repositoryBaseUrl, apiPath, body);
            var response = await HttpClient.SendAsync(request);
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringResponse);
        }

        private async Task<HttpRequestMessage> InitializeRequestMessageAsync(HttpMethod httpMethod, Uri repositoryBaseUrl, string apiPath, string body)
        {
            var target = new Uri(string.Concat(repositoryBaseUrl.AbsoluteUri, "/_apis/git/", apiPath, "?api-version=4.1"));
            
            var request = new HttpRequestMessage(httpMethod, target);

            if (!string.IsNullOrWhiteSpace(body))
            {
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            string accessToken = await this.GetAccessTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = await AzureServiceTokenProvider.GetAccessTokenAsync(VSTSResourceId);
            }

            return _accessToken;
        }
    }
}