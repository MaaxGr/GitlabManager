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
        /*
         * Page Meta
         */
        public string PageName => "Projects";
        public AppNavigationSection Section => AppNavigationSection.Operation;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /*
         * Dependencies
         */
        private readonly PageProjectsModel _pageModel;
        private readonly IDynamicDependencyProvider _dynamicDependencyProvider;

        /*
         * Properties
         */
        // list of displayed projects
        public List<PageProjectsSingleProjectViewModel> Projects => GetAllProjects();

        // currently selected project
        public PageProjectsSingleProjectViewModel SelectedProject { get; set; }

        // currently entered search text
        public string SearchText
        {
            get => _pageModel.SearchText;
            set => _pageModel.RefreshSearch(value);
        }

        public ProjectListSorting ProjectSorting
        {
            get => _pageModel.SortingMode;
            set => _pageModel.SetSortingMode(value);
        }

        /*
         * Commands
         */
        public ICommand EnterPressedCommand { get; }

        public PageProjectsViewModel(PageProjectsModel pageModel, IDynamicDependencyProvider dynamicDependencyProvider)
        {
            // init dependencies
            _pageModel = pageModel;
            _pageModel.PropertyChanged += ProjectsModelPropertyChangedHandler;
            _dynamicDependencyProvider = dynamicDependencyProvider;

            // init commands
            EnterPressedCommand = new AppDelegateCommand<object>(o => EnterPressedCommandExecutor());
        }

        public async Task Init()
        {
            await _pageModel.Init();
        }


        /*
        * Utilities
        */
        // handle property changes from page model
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

        // get all projects from model and convert them to viewmodel objects
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
            SelectedProject?.OpenProjectDetailWindow();
        }
        
    }
}