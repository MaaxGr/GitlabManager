using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GitlabManager.Framework;
using GitlabManager.Model;
using GitlabManager.Services.Dialog;

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
        private readonly IDialogService _dialogService;

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
        public ICommand CloneProjectToDefaultFolderCommand { get; }
        public ICommand CloneProjectToCustomFolderCommand { get; }

        public ICommand OpenInExplorerCommand { get;  }
        public ICommand OpenInVsCodeCommand { get;  }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowModel">Model for Window</param>
        /// <param name="dialogService">Service to open dialogs</param>
        public WindowProjectDetailViewModel(WindowProjectDetailModel windowModel, IDialogService dialogService)
        {
            // init dependencies
            _dialogService = dialogService;
            _windowModel = windowModel;
            _windowModel.PropertyChanged += ProjectDetailModelPropertyChangedHandler;
            
            // init commands
            OpenInBrowserCommand = new AppDelegateCommand<object>(_ => OpenInBrowserCommandExecutor());
            CloneProjectToDefaultFolderCommand = new AppDelegateCommand<object>(_ => CloneProjectToDefaultFolderExecutor());
            CloneProjectToCustomFolderCommand = new AppDelegateCommand<object>(_ => CloneProjectToCustomFolderCommandExecutor());

            OpenInExplorerCommand = new AppDelegateCommand<object>(_ => OpenInExplorerCommandExecutor());
            OpenInVsCodeCommand = new AppDelegateCommand<object>(_ => OpenInVsCodeCommandExecutor());
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

        private void OpenInBrowserCommandExecutor()
        {
            try
            {
                _windowModel.OpenProjectInBrowser();
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                Console.WriteLine(e);
            }
        }

        private void CloneProjectToDefaultFolderExecutor()
        {
            try
            {
                _windowModel.CloneProjectToDefaultFolder();
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                Console.WriteLine(e);
            }
        }

        private void CloneProjectToCustomFolderCommandExecutor()
        {
            try
            {
                _windowModel.CloneProjectToCustomFolder();
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                Console.WriteLine(e);
            }
        }

        private void OpenInExplorerCommandExecutor()
        {
            try
            {
                _windowModel.OpenInApp("explorer");
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                Console.WriteLine(e);
            }
        }

        private void OpenInVsCodeCommandExecutor()
        {
            try
            {
                _windowModel.OpenInApp("vscode");
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                Console.WriteLine(e);
            }
        }
        
        

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