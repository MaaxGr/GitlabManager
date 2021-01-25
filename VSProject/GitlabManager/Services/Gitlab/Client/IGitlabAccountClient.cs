using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabApiClient.Models.Projects.Responses;

namespace GitlabManager.Services.Gitlab.Client
{
    public interface IGitlabAccountClient
    {

        public Task<Tuple<bool, string>> IsConnectionEstablished();

        public Task<IList<Project>> GetAllProjects();

        public Task<IList<Project>> GetAllProjectsAfter(DateTime dateTime);

    }
}