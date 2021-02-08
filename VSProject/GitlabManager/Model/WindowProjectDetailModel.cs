using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.BusinessLogic;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Dialog;
using GitlabManager.Services.Gitlab.Client;
using GitlabManager.Services.Gitlab.Model;
using GitlabManager.Services.System;

namespace GitlabManager.Model
{
    /// <summary>
    /// Model for the Project-Detail Window
    /// </summary>
    public class WindowProjectDetailModel : AppModel
    {
        #region Dependencies

        private readonly GitlabProjectManager _gitlabProjectManager;
        private readonly ISystemService _systemService;
        private readonly DatabaseService _databaseService;
        private readonly GitlabProjectDownloader _gitlabProjectDownloader;
        private readonly IDialogService _dialogService;

        #endregion

        #region Private Internal State

        private DbProject _dbProject;
        private JsonProject _jsonProject;

        #endregion

        #region Public Properties exposed to ViewModel

        public string ProjectNameWithNamespace => _dbProject.NameWithNamespace;
        public string Description => _dbProject.Description;
        public string AccountIdentifier => _dbProject.Account.Identifier;
        public int CommitCount => _jsonProject.Statistics.CommitCount;
        public int GitlabProjectId => _jsonProject.Id;
        public int TagCount => _jsonProject.TagList.Count;
        public int StarCount => _jsonProject.StarCount;
        public int OpenIssueCount => _jsonProject.OpenIssuesCount;
        public string WebUrl => _jsonProject.WebUrl;
        public List<string> TagList => _jsonProject.TagList;
        public bool IsProjectDownloaded => !string.IsNullOrWhiteSpace(_dbProject.LocalFolder);
        public bool ProjectDownloading { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gitlabProjectManager">Service to access gitlab projects</param>
        /// <param name="systemService">Service to access system functionality</param>
        /// <param name="databaseService">Service to access database</param>
        /// <param name="gitlabProjectDownloader">Service that cares about downloading git projects</param>
        /// <param name="dialogService">Service to open dialogs</param>
        public WindowProjectDetailModel(GitlabProjectManager gitlabProjectManager, ISystemService systemService,
            DatabaseService databaseService, GitlabProjectDownloader gitlabProjectDownloader,
            IDialogService dialogService)
        {
            _gitlabProjectManager = gitlabProjectManager;
            _systemService = systemService;
            _databaseService = databaseService;
            _gitlabProjectDownloader = gitlabProjectDownloader;
            _dialogService = dialogService;
        }

        #region Public Actions

        /// <summary>
        /// Init ViewModel for projectId
        /// </summary>
        /// <param name="projectId">Internal ProjectId (Database) that should be opened</param>
        public async void Init(int projectId)
        {
            // get project info from database
            _dbProject = _gitlabProjectManager.GetProject(projectId);

            // get json project meta data
            _jsonProject = await _gitlabProjectManager.GetCachedProjectMeta(_dbProject);

            // init download for specific gitlab account
            _gitlabProjectDownloader.InitForClient(
                new GitlabAccountClientImpl(_dbProject.Account.HostUrl, _dbProject.Account.AuthenticationToken)
            );
            // delete local folder in database if folder not present on client
            if (!string.IsNullOrEmpty(_dbProject.LocalFolder) && !Directory.Exists(_dbProject.LocalFolder))
            {
                _databaseService.UpdateLocalFolderForProject(_dbProject, "");
            }
            
            RaiseUpdateProject();
        }

        /// <summary>
        /// Open a project in the web browser
        /// </summary>
        public void OpenProjectInBrowser()
        {
            _systemService.OpenBrowser(WebUrl);
        }

        /// <summary>
        /// Clone a Git-project to local directory
        /// </summary>
        public void CloneProjectToDefaultFolder()
        {
            // build folder name and path
            var folderName = _jsonProject.NameWithNamespace
                .Replace("/", "-")
                .Replace(" ", "")
                .Replace("ß", "ss")
                .ToLower();
            var folderPath = $"{_gitlabProjectDownloader.ProjectsDefaultFolder}/{folderName}";
            CloneProjectToFolder(folderPath);
        }

        public void CloneProjectToCustomFolder()
        {
            // build folder name and path
            var folderPath = _dialogService.SelectFolderDialog(
                description: "Select folder in which project should be cloned"
            );
            CloneProjectToFolder(folderPath);
        }

        private void CloneProjectToFolder(string folderPath)
        {
            Task.Run(async () =>
            {
                // set ui loading
                SetProjectDownloadingStatus(true);

                // download project
                await _gitlabProjectDownloader.DownloadGitlabProject(_jsonProject.HttpUrlToRepo, folderPath);

                // set path to database
                _databaseService.UpdateLocalFolderForProject(_dbProject, folderPath);

                // set ui not loading any more
                SetProjectDownloadingStatus(false);
                RaisePropertyChanged(nameof(IsProjectDownloaded));
            });
        }

        /// <summary>
        /// Open project in App
        /// </summary>
        /// <param name="appName">Name of app (currently explorer or vscode)</param>
        public void OpenInApp(string appName)
        {
            switch (appName)
            {
                case "explorer":
                    _systemService.OpenFolderInExplorer(_dbProject.LocalFolder);
                    break;
                case "vscode":
                    _systemService.OpenFolderInVsCode(_dbProject.LocalFolder);
                    break;
            }
        }

        #endregion


        #region Private Utility Methods

        private void RaiseUpdateProject()
        {
            RaisePropertyChanged(nameof(ProjectNameWithNamespace));
            RaisePropertyChanged(nameof(Description));
            RaisePropertyChanged(nameof(AccountIdentifier));
            RaisePropertyChanged(nameof(CommitCount));
            RaisePropertyChanged(nameof(GitlabProjectId));
            RaisePropertyChanged(nameof(TagCount));
            RaisePropertyChanged(nameof(StarCount));
            RaisePropertyChanged(nameof(OpenIssueCount));
            RaisePropertyChanged(nameof(WebUrl));
            RaisePropertyChanged(nameof(TagList));
            RaisePropertyChanged(nameof(IsProjectDownloaded));
        }

        private void SetProjectDownloadingStatus(bool downloading)
        {
            ProjectDownloading = downloading;
            RaisePropertyChanged(nameof(ProjectDownloading));
        }

        #endregion
    }
}