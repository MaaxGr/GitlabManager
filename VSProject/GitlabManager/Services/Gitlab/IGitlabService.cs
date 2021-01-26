using GitlabManager.Services.Gitlab.Client;

namespace GitlabManager.Services.Gitlab
{
    /// <summary>
    /// Services that provides a interface to get multiple connections to different gitlab accounts.
    /// </summary>
    public interface IGitlabService
    {

        public IGitlabAccountClient GetGitlabClient(string hostUrl, string authenticationToken);

    }
}