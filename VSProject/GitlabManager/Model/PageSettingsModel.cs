using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.BusinessLogic;
using GitlabManager.Services.Dialog;

namespace GitlabManager.Model
{
    /// <summary>
    /// Model for the Project-List Page
    /// </summary>
    public class PageSettingsModel : AppModel
    {
        #region Dependencies

        private GitlabProjectDownloader _gitlabProjectDownloader;
        private readonly IDialogService _dialogService;

        #endregion

        #region Internal Properties

        #endregion
        
        #region Exposed Properties to View Model

        public string CurrentDirectory { get; private set; }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectDownloader">Service that cares about downloading projects</param>
        /// <param name="dialogService">Service that is responsible for opening dialogs</param>
        public PageSettingsModel(GitlabProjectDownloader projectDownloader, IDialogService dialogService)
        {
            _gitlabProjectDownloader = projectDownloader;
            _dialogService = dialogService;
        }

        #region Public Actions

        /// <summary>
        /// Init function 
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            // init dependencies
            _gitlabProjectDownloader.InitCommon();
            
            // load and raise current default directory
            CurrentDirectory = _gitlabProjectDownloader.ProjectsDefaultFolder;
            RaisePropertyChanged(nameof(CurrentDirectory));
        }

        /// <summary>
        /// Action for button click on git directory change
        /// </summary>
        public void ChangeDefaultGitDirectory()
        {
            var selectedFolder = _dialogService.SelectFolderDialog(
                description: "Folder in which your git projects will be downloaded"
            );

            CurrentDirectory = selectedFolder;
            _gitlabProjectDownloader.UpdateProjectsDefaultFolder(selectedFolder);
            RaisePropertyChanged(nameof(CurrentDirectory));
        }

        #endregion

        #region Private Utility Methods

        #endregion
    }
}