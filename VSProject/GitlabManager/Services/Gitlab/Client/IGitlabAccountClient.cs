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

        /// <summary>
        /// Expose gitlab access token
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken();
        
        /// <summary>
        /// Check if connection can be established (tupel with success-Boolean and error-Message)
        /// </summary>
        /// <returns></returns>
        public Task<Tuple<bool, string>> IsConnectionEstablished();

        /// <summary>
        /// Fetch all projects from gitlab api
        /// </summary>
        /// <returns></returns>
        public Task<IList<JsonProject>> GetAllProjects();

        /// <summary>
        /// Fetch all projects with lastUpdated greater dateTime
        /// </summary>
        /// <param name="dateTime">Minimum updated date</param>
        /// <returns></returns>
        public Task<IList<JsonProject>> GetAllProjectsAfter(DateTime dateTime);

        /// <summary>
        /// Fetch single project from gitlab api
        /// </summary>
        /// <param name="gitlabProjectId"></param>
        /// <returns></returns>
        public Task<JsonProject> GetProjectById(int gitlabProjectId);
        
        /// <summary>
        /// Get info of the current gitlab session (e.g. Username)
        /// </summary>
        /// <returns></returns>
        public Task<Session> GetCurrentSession();

    }
}