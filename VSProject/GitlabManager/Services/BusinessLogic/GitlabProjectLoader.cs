using System;
using System.IO;
using System.Threading.Tasks;
using GitlabManager.Services.Gitlab.Client;
using LibGit2Sharp;

namespace GitlabManager.Services.BusinessLogic
{
    /// <summary>
    /// Business Logic Class that handles Downloading of Git-Projects 
    /// </summary>
    public class GitlabProjectLoader
    {
        private readonly IGitlabAccountClient _gitlabAccountClient;
        
        /// <summary>
        /// Folder where All Gitlab-Projects should be stored
        /// </summary>
        public readonly string GitlabmanagerDocumentsFolder;
        
        public GitlabProjectLoader(IGitlabAccountClient gitlabAccountClient)
        {
            _gitlabAccountClient = gitlabAccountClient;
            var documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            GitlabmanagerDocumentsFolder = $"{documentFolder}/GitlabManagerProjects";
            
            CreateDirectoriesIfNotExist();
        }

        #region Public Methods

        /// <summary>
        /// Download a project for a given url to the specified folder
        /// </summary>
        /// <param name="httpsCloneUrl">URL to git repository</param>
        /// <param name="folderName">relative name of local folder where project should be cloned to</param>
        /// <returns>Whether cloning was successful</returns>
        public async Task<bool> DownloadGitlabProject(string httpsCloneUrl, string folderName)
        {
            var currentGitlabSession = await _gitlabAccountClient.GetCurrentSession();
            var username = currentGitlabSession.Username;
            var accessToken = _gitlabAccountClient.GetAccessToken();
            
            var urlWithCredentials = IncludeCredentialsInUrl(httpsCloneUrl, username, accessToken);
            
            return await Task.Run(() =>
            {
                var projectFolder = $"{GitlabmanagerDocumentsFolder}/{folderName}";
                var createdPath = Repository.Clone(urlWithCredentials, projectFolder);
                return createdPath != null;
            });
        }

        /// <summary>
        /// Add credentials to http(s) clone url as basic auth, so that no authentication error appears 
        /// </summary>
        /// <param name="cloneUrl">HTTP(S) clone url to gitlab project</param>
        /// <param name="userName">Username of the authenticated user</param>
        /// <param name="authToken">Private Token of the authenticated user</param>
        /// <returns></returns>
        public static string IncludeCredentialsInUrl(string cloneUrl, string userName, string authToken)
        {
            var index = cloneUrl.IndexOf("://", StringComparison.Ordinal);
            return cloneUrl.Insert(index + 3, $"{userName}:{authToken}@");
        }

        #endregion

        #region Private Utility Methods

        /// <summary>
        /// Create a directory if it doesnt exist before
        /// </summary>
        private void CreateDirectoriesIfNotExist()
        {
            if (!Directory.Exists(GitlabmanagerDocumentsFolder))
                Directory.CreateDirectory(GitlabmanagerDocumentsFolder);
            
        }

        #endregion
        
    }
}