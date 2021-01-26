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
        public readonly string GitlabmanagerDocumentsFolder;
        
        public GitlabProjectLoader(IGitlabAccountClient gitlabAccountClient)
        {
            _gitlabAccountClient = gitlabAccountClient;
            var documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            GitlabmanagerDocumentsFolder = $"{documentFolder}/GitlabManagerProjects";
            
            CreateDirectoriesIfNotExist();
        }

        private void CreateDirectoriesIfNotExist()
        {
            if (!Directory.Exists(GitlabmanagerDocumentsFolder))
                Directory.CreateDirectory(GitlabmanagerDocumentsFolder);
            
        }

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

        public static string IncludeCredentialsInUrl(string cloneUrl, string userName, string authToken)
        {
            var index = cloneUrl.IndexOf("://", StringComparison.Ordinal);
            return cloneUrl.Insert(index + 3, $"{userName}:{authToken}@");
        }
    }
}