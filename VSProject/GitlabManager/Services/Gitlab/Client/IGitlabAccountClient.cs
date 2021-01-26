using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabApiClient.Models.Users.Responses;
using GitlabManager.Services.Gitlab.Model;

namespace GitlabManager.Services.Gitlab.Client
{
    /// <summary>
    /// Service that is responsible for connecting to one particular gitlab rest api
    /// </summary>
    public interface IGitlabAccountClient
    {

        public string GetAccessToken();
        
        public Task<Tuple<bool, string>> IsConnectionEstablished();

        public Task<IList<JsonProject>> GetAllProjects();

        public Task<IList<JsonProject>> GetAllProjectsAfter(DateTime dateTime);

        public Task<JsonProject> GetProjectById(int gitlabProjectId);

        public Task DownloadProjectRepositoryAsZip(int gitlabProjectId, string downloadPath);

        public Task<Session> GetCurrentSession();

    }
}