using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GitlabManager.Framework;
using GitlabManager.Model;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for the entire Detail-Window for a single Project
    /// Project Detail Window is opened in an own window by design, so that the user can open multiple windows
    /// side by side.
    /// </summary>
    public class WindowProjectDetailViewModel : AppViewModel
    {
        #region Dependencies

        private readonly WindowProjectDetailModel _windowModel;

        #endregion

        #region State for View

        public string WindowTitle => _windowModel.ProjectNameWithNamespace;
        public string HeaderMainText => _windowModel.ProjectNameWithNamespace;
        public string HeaderTopText => _windowModel.AccountIdentifier;

        public string InfoDescription => _windowModel.Description;
        public int InfoCommitCount => _windowModel.CommitCount;
        public int InfoOpenIssuesCount => _windowModel.OpenIssueCount;
        public int InfoGitlabProjectId => _windowModel.GitlabProjectId;
        public int InfoStarCount => _windowModel.StarCount;
        public List<string> InfoTagList => _windowModel.TagList;

        public Visibility CloneLoadingVisibility =>
            _windowModel.ProjectDownloading ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ButtonCloneVisibility => !_windowModel.IsProjectDownloaded ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ButtonOpenInVisibility =>
            _windowModel.IsProjectDownloaded ? Visibility.Visible : Visibility.Collapsed;
        
        #endregion

        #region Commands in View

        public ICommand OpenInBrowserCommand { get; }
        public ICommand CloneProjectCommand { get; }

        public ICommand OpenInExplorerCommand { get;  }
        public ICommand OpenInVSCodeCommand { get;  }

        #endregion
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowModel">Model for Window</param>
        public WindowProjectDetailViewModel(WindowProjectDetailModel windowModel)
        {
            // init dependencies
            _windowModel = windowModel;
            _windowModel.PropertyChanged += ProjectDetailModelPropertyChangedHandler;
            
            // init commands
            OpenInBrowserCommand = new AppDelegateCommand<object>(_ => _windowModel.OpenProjectInBrowser());
            CloneProjectCommand = new AppDelegateCommand<object>(_ => _windowModel.CloneProject());

            OpenInExplorerCommand = new AppDelegateCommand<object>(_ => _windowModel.OpenInApp("explorer"));
            OpenInVSCodeCommand = new AppDelegateCommand<object>(_ => _windowModel.OpenInApp("vscode"));
        }

        /// <summary>
        /// init function
        /// </summary>
        /// <param name="projectId">internal project id</param>
        public void Init(int projectId)
        {
            _windowModel.Init(projectId);
        }

        #region Private Utility Functions

        private void ProjectDetailModelPropertyChangedHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(WindowProjectDetailModel.ProjectNameWithNamespace):
                    RaisePropertyChanged(nameof(WindowTitle));
                    RaisePropertyChanged(nameof(HeaderMainText));
                    break;
                case nameof(WindowProjectDetailModel.AccountIdentifier):
                    RaisePropertyChanged(nameof(HeaderTopText));
                    break;
                case nameof(WindowProjectDetailModel.Description):
                    RaisePropertyChanged(nameof(InfoDescription));
                    break;
                case nameof(WindowProjectDetailModel.CommitCount):
                    RaisePropertyChanged(nameof(InfoCommitCount));
                    break;
                case nameof(WindowProjectDetailModel.StarCount):
                    RaisePropertyChanged(nameof(InfoStarCount));
                    break;
                case nameof(WindowProjectDetailModel.OpenIssueCount):
                    RaisePropertyChanged(nameof(InfoOpenIssuesCount));
                    break;
                case nameof(WindowProjectDetailModel.GitlabProjectId):
                    RaisePropertyChanged(nameof(InfoGitlabProjectId));
                    break;
                case nameof(WindowProjectDetailModel.ProjectDownloading):
                    RaisePropertyChanged(nameof(CloneLoadingVisibility));
                    break;
                case nameof(WindowProjectDetailModel.IsProjectDownloaded):
                    RaisePropertyChanged(nameof(ButtonCloneVisibility));
                    RaisePropertyChanged(nameof(ButtonOpenInVisibility));
                    break;
                case nameof(WindowProjectDetailModel.TagList):
                    RaisePropertyChanged(nameof(InfoTagList));
                    break;
            }
        }

        #endregion
        
    
    }
}