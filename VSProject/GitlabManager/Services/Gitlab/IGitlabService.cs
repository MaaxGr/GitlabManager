using GitlabManager.Services.Gitlab.Client;

namespace GitlabManager.Services.Gitlab
{
    /// <summary>
    /// Services that provides a interface to get multiple connections to different gitlab accounts.
    /// </summary>
    public interface IGitlabService
    {

        /// <summary>
        /// Get IGitlabAccountClient Instance for specified credentials
        /// </summary>
        /// <param name="hostUrl">Host URL of Gitlab instance</param>
        /// <param name="authenticationToken">Private gitlab token</param>
        /// <returns></returns>
        public IGitlabAccountClient GetGitlabClient(string hostUrl, string authenticationToken);

    }
}