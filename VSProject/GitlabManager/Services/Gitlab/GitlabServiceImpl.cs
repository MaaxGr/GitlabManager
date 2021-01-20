using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabApiClient.Models.Projects.Responses;
using GitlabManager.Services.Gitlab.Client;

namespace GitlabManager.Services.Gitlab
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class GitlabServiceImpl : IGitlabService
    {

        public IGitlabAccountClient GetGitlabClient(string hostUrl, string authenticationToken)
        {
            return new GitlabAccountClientImpl(new GitLabClient(hostUrl, authenticationToken));
        }
        

        public async Task<IList<Project>> GetProjects()
        {
            var client = new GitLabClient("https://gitlab.maax.gr", "d3e1mKUsJJ8g2SNREYVz");
            return await client.Projects.GetAsync();
        }

    }
}