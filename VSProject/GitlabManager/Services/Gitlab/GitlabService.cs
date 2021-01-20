using GitlabManager.Services.Gitlab.Client;

namespace GitlabManager.Services.Gitlab
{
    public interface IGitlabService
    {

        public IGitlabAccountClient GetGitlabClient(string hostUrl, string authenticationToken);

    }
}