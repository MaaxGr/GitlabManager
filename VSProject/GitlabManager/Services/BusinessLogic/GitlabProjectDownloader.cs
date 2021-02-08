using System;
using System.IO;
using System.Threading.Tasks;
using GitlabManager.Services.Database;
using GitlabManager.Services.Gitlab.Client;
using LibGit2Sharp;

namespace GitlabManager.Services.BusinessLogic
{
    /// <summary>
    /// Business Logic Class that handles Downloading of Git-Projects 
    /// </summary>
    public class GitlabProjectDownloader
    {
        /// <summary>
        /// Setting key that points to the path of the directory in which git project folders will be created
        /// (Is by default: Documents/GitlabManagerProjects)
        /// </summary>
        private const string SettingKeyDefaultProjectDir = "default_project_dir";


        private IGitlabAccountClient _gitlabAccountClient;
        private readonly DatabaseService _databaseService;

        /// <summary>
        /// Folder where All Gitlab-Projects should be stored
        /// </summary>
        public string ProjectsDefaultFolder { get; private set; }

        public GitlabProjectDownloader(DatabaseService databaseService)
        {
            // init dependencies
            _databaseService = databaseService;
        }

        #region Public Methods

        /// <summary>
        /// Init without a specific gitlab account client (e.g. settings page) 
        /// </summary>
        public void InitCommon()
        {
            // set initial directory
            var documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var initialDirectory = $"{documentFolder}/GitlabManagerProjects";

            // find out default directory
            var persistedDefaultDir = _databaseService.GetSettingsValue(SettingKeyDefaultProjectDir);
            ProjectsDefaultFolder = persistedDefaultDir ?? initialDirectory;

            CreateDirectoriesIfNotExist();
        }

        /// <summary>
        /// Init connection to gitlab account for specific project
        /// </summary>
        /// <param name="gitlabAccountClient"></param>
        public void InitForClient(IGitlabAccountClient gitlabAccountClient)
        {
            InitCommon();
            _gitlabAccountClient = gitlabAccountClient;
        }

        /// <summary>
        /// Update default folder for projects (database setting will be updated also)
        /// </summary>
        /// <param name="path">Path to default folder</param>
        public void UpdateProjectsDefaultFolder(string path)
        {
            ProjectsDefaultFolder = path;
            _databaseService.WriteSettingValue(SettingKeyDefaultProjectDir, path);
        }

        /// <summary>
        /// Download a project for a given url to the specified folder
        /// </summary>
        /// <param name="httpsCloneUrl">URL to git repository</param>
        /// <param name="folderPath">absolute path to folder where project should be cloned to</param>
        /// <returns>Whether cloning was successful</returns>
        public async Task<bool> DownloadGitlabProject(string httpsCloneUrl, string folderPath)
        {
            var currentGitlabSession = await _gitlabAccountClient.GetCurrentSession();
            var username = currentGitlabSession.Username;
            var accessToken = _gitlabAccountClient.GetAccessToken();

            var urlWithCredentials = IncludeCredentialsInUrl(httpsCloneUrl, username, accessToken);

            // re-clone, if folder already exist  
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
     

            return await Task.Run(() =>
            {
                var createdPath = Repository.Clone(urlWithCredentials, folderPath);
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
            if (!Directory.Exists(ProjectsDefaultFolder))
                Directory.CreateDirectory(ProjectsDefaultFolder);
        }

        #endregion
    }
}