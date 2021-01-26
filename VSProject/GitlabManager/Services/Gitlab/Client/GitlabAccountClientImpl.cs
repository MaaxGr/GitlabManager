using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabApiClient.Models.Users.Responses;
using GitlabManager.Services.Gitlab.Model;
using GitlabManager.Services.Logging;
using GitlabManager.Utils;
using Newtonsoft.Json;

namespace GitlabManager.Services.Gitlab.Client
{
    /// <summary>
    /// Default implementation of <see cref="IGitlabService"/>
    /// Connection to gitlab are implemented in the following way
    /// * Through library to get infos for authenticated user (e.g. his username)
    /// * Through HttpClient to get all infos for projects
    ///
    /// The prefered method should be the library, because it has a better dsl and we don't have to
    /// deal with http headers and other low level stuff instead using direct method calls.
    ///
    /// Unfortunately there are two problems with the library  
    /// * Projects can't be serialized (circular dependency)
    /// * A issue in the library which leads to crash on particular projects (overflow of the registry-size variable)
    ///   (Issue just openend: <see cref="https://github.com/nmklotas/GitLabApiClient/issues/203"/> 
    ///
    /// Link to the used library
    /// <see cref="https://github.com/nmklotas/GitLabApiClient"/>
    /// </summary>
    public class GitlabAccountClientImpl : IGitlabAccountClient
    {
        private readonly GitLabClient _client;
        private string _hostUrl;
        private string _authenticationToken;

        private HttpClient _httpClient;

        public GitlabAccountClientImpl(string hostUrl, string authenticationToken)
        {
            _hostUrl = hostUrl;
            _authenticationToken = authenticationToken;
            
            LoggingService.LogD($"hosturl '{hostUrl}'");
            LoggingService.LogD($"token '{authenticationToken}'");
            
            _client = new GitLabClient(hostUrl, authenticationToken);
            
            _httpClient = new HttpClient {BaseAddress = new Uri(_hostUrl)};
            _httpClient.DefaultRequestHeaders.Add("PRIVATE-TOKEN", _authenticationToken);
        }

        public async Task<Session> GetCurrentSession()
        {
            return await _client.Users.GetCurrentSessionAsync();
        }

        public async Task<Tuple<bool, string>> IsConnectionEstablished()
        {
            try
            {
                var session = await _client.Users.GetCurrentSessionAsync();

                // connection is established, if username of current session in not blank
                return string.IsNullOrWhiteSpace(session.Username)
                    ? new Tuple<bool, string>(false, "Username is null or empty!")
                    : new Tuple<bool, string>(true, "");
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }
        }

        public async Task<IList<JsonProject>> GetAllProjects()
        {
            var projects = new List<JsonProject>();
            var page = 1;
            while (true)
            {
                var pageProjects = await GetProjects(page);
                if (pageProjects.Count == 0)
                {
                    break;
                }

                projects.AddRange(pageProjects);
                page++;
            }

            return projects;
        }

        public async Task<IList<JsonProject>> GetAllProjectsAfter(DateTime dateTime)
        {
            var projects = new List<JsonProject>();
            var page = 1;
            while (true)
            {
                var pageProjects = await GetProjectsAfter(page, dateTime);
                if (pageProjects.Count == 0)
                {
                    break;
                }

                projects.AddRange(pageProjects);
                page++;
            }

            return projects;
        }

        public async Task<JsonProject> GetProjectById(int gitlabProjectId)
        {
            var jsonResponse = await _httpClient.GetStringAsync(
                $"api/v4/projects/{gitlabProjectId}?statistics=true"
            );
            return JsonConvert.DeserializeObject<JsonProject>(jsonResponse);
        }

        public async Task DownloadProjectRepositoryAsZip(int gitlabProjectId, string downloadPath)
        {
            var webClient = new WebClient {BaseAddress = _hostUrl};
            webClient.Headers.Add("PRIVATE-TOKEN", _authenticationToken);
            webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            
            LoggingService.LogD("START");
            // webClient.DownloadFileCompleted += (sender, args) =>
            // {
            //     LoggingService.LogD("COMPLETION!");
            // };
            
            await webClient.DownloadFileTaskAsync($"api/v4/projects/{gitlabProjectId}/repository/archive.zip", downloadPath);
            LoggingService.LogD("DONE");
        }


        private async Task<List<JsonProject>> GetProjects(int page)
        {
            var jsonResponse = await _httpClient.GetStringAsync(
                $"api/v4/projects?per_page=100&&page={page}&statistics=true&membership=true"
            );
            return JsonConvert.DeserializeObject<List<JsonProject>>(jsonResponse);
        }

        private async Task<List<JsonProject>> GetProjectsAfter(int page, DateTime dateTime)
        {
            var urlDate = dateTime.ToIso8691();
            var jsonResponse = await _httpClient.GetStringAsync(
                $"api/v4/projects?per_page=100&&page={page}&statistics=true&membership=true&last_activity_after={urlDate}"
            );
            return JsonConvert.DeserializeObject<List<JsonProject>>(jsonResponse);
        }

        public string GetAccessToken() => _authenticationToken;
    }
}