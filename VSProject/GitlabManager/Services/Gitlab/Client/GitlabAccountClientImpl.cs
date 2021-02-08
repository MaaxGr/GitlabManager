using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabApiClient.Models.Users.Responses;
using GitlabManager.Services.Gitlab.Model;
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

        private string _errorMessage;
        
        private HttpClient _httpClient;

        public GitlabAccountClientImpl(string hostUrl, string authenticationToken)
        {
            _hostUrl = hostUrl;
            _authenticationToken = authenticationToken;
            
            try
            {
                _client = new GitLabClient(hostUrl, authenticationToken);
                _errorMessage = "";
                
                _httpClient = new HttpClient {BaseAddress = new Uri(_hostUrl)};
                _httpClient.DefaultRequestHeaders.Add("PRIVATE-TOKEN", _authenticationToken);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                _errorMessage = e.Message;
            }
        }

        public async Task<Session> GetCurrentSession()
        {
            return await _client.Users.GetCurrentSessionAsync();
        }

        public async Task<Tuple<bool, string>> IsConnectionEstablished()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_errorMessage))
                {
                    return new Tuple<bool, string>(false, _errorMessage);
                }
                
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