using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Model;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for the entire Projects-Page in the MainWindow.
    /// </summary>
    public class PageSettingsViewModel
        : AppViewModel, IApplicationContentView
    {

        #region Page Meta data

        public string PageName => "Settings";
        public AppNavigationSection Section => AppNavigationSection.Administration;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        #region Dependencies

        private readonly PageSettingsModel _pageModel;

        #endregion
        
        #region Public Properties

        public string DefaultProjectsDirectory => _pageModel.CurrentDirectory;

        /// <summary>
        /// Command that is executed on button click to change default project directory
        /// </summary>
        public ICommand ChangeDefaultProjectsDirectoryCommand { get; }

        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageModel">Page model</param>
        public PageSettingsViewModel(PageSettingsModel pageModel)
        {
            // init dependencies
            _pageModel = pageModel;
            _pageModel.PropertyChanged += SettingsModelPropertyChangedHandler;

            // init commands
            ChangeDefaultProjectsDirectoryCommand = new AppDelegateCommand<object>(o => _pageModel.ChangeDefaultGitDirectory());
        }
        
        /// <summary>
        /// Async method to init page (Loading animation is present, while loading)
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            await _pageModel.Init();
        }

        #region Private Utility Methods

        /// <summary>
        /// handle property changes from page model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SettingsModelPropertyChangedHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(PageSettingsModel.CurrentDirectory):
                    RaisePropertyChanged(nameof(DefaultProjectsDirectory));
                    break;
            }
        }

        #endregion


    }
}