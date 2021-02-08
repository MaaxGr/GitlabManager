using GitlabManager.Services.Gitlab.Client;

namespace GitlabManager.Services.Gitlab
{
    /// <summary>
    /// Default implementation of <see cref="IGitlabService"/>
    /// </summary>
    public class GitlabServiceImpl : IGitlabService
    {

        public IGitlabAccountClient GetGitlabClient(string hostUrl, string authenticationToken)
        {
            return new GitlabAccountClientImpl(hostUrl, authenticationToken);
        }

    }
}