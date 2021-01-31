using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Model;
using GitlabManager.Services.DI;
using GitlabManager.Services.Logging;
using GitlabManager.Utils;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for the entire Projects-Page in the MainWindow.
    /// </summary>
    public class PageProjectsViewModel
        : AppViewModel, IApplicationContentView
    {

        #region Page Meta data

        public string PageName => "Projects";
        public AppNavigationSection Section => AppNavigationSection.Operation;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        /*
         * Dependencies
         */

        #region Dependencies

        private readonly PageProjectsModel _pageModel;
        private readonly IDynamicDependencyProvider _dynamicDependencyProvider;

        #endregion


        #region Public Properties

        /// <summary>
        /// list of displayed projects
        /// </summary>
        public List<PageProjectsSingleProjectViewModel> Projects => GetAllProjects();

        /// <summary>
        /// currently selected project
        /// </summary>
        public PageProjectsSingleProjectViewModel SelectedProject { get; set; }
        
        /// <summary>
        /// currently entered search text
        /// </summary>
        public string SearchText
        {
            get => _pageModel.SearchText;
            set => _pageModel.RefreshSearch(value);
        }

        /// <summary>
        /// currently selected project sorting
        /// </summary>
        public ProjectListSorting ProjectSorting
        {
            get => _pageModel.SortingMode;
            set => _pageModel.SetSortingMode(value);
        }

        /// <summary>
        /// Command that is executed on double-click / enter pressed
        /// </summary>
        public ICommand EnterPressedCommand { get; }

        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageModel">Page model</param>
        /// <param name="dynamicDependencyProvider">Service to resolve dependencies dynamically</param>
        public PageProjectsViewModel(PageProjectsModel pageModel, IDynamicDependencyProvider dynamicDependencyProvider)
        {
            // init dependencies
            _pageModel = pageModel;
            _pageModel.PropertyChanged += ProjectsModelPropertyChangedHandler;
            _dynamicDependencyProvider = dynamicDependencyProvider;

            // init commands
            EnterPressedCommand = new AppDelegateCommand<object>(o => EnterPressedCommandExecutor());
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
        private void ProjectsModelPropertyChangedHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(PageProjectsModel.DisplayedProjectsSorted):
                    LoggingService.LogD("Update projects in vm...");
                    RaisePropertyChanged(nameof(Projects));
                    break;
            }
        }

        /// <summary>
        /// get all projects from model and convert them to viewmodel objects
        /// </summary>
        /// <returns></returns>
        private List<PageProjectsSingleProjectViewModel> GetAllProjects()
        {
            return _pageModel.DisplayedProjectsSorted.Select(modelProject =>
                {
                    var projectVm = _dynamicDependencyProvider.GetInstance<PageProjectsSingleProjectViewModel>();
                    projectVm.DbProject = modelProject;
                    projectVm.NameWithNamespace = modelProject.NameWithNamespace;
                    projectVm.Description = modelProject.Description;
                    projectVm.LastUpdatedAgo = DateTimeUtils.UnixTimestampAgoHumanReadable(modelProject.LastUpdated);
                    projectVm.TagList = modelProject.TagList.ToList();
                    return projectVm;
                }
            ).ToList();
        }

        private void EnterPressedCommandExecutor()
        {
            SelectedProject?.OpenProjectDetailWindowExecutor();
        }


        #endregion
        
   
    }
}