using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GitLabApiClient.Models.Projects.Responses;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Services.Gitlab;

namespace GitlabManager.ViewModels
{
    public class PageProjectsViewModel
        : ViewModel, IApplicationContentView
    {
        
        // Page definition
        public string PageName => "Projects";
        public AppNavigationSection Section => AppNavigationSection.Operation;
        
        // Loading state
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        // List of projects
        private ObservableCollection<Project> _listOfProjects;

        public ObservableCollection<Project> ListOfProjects
        {
            get => _listOfProjects;
            set => SetProperty(ref _listOfProjects, value);
        }

        public async Task Init()
        {
            var gitlabRepo = new GitlabServiceImpl();
            
            var projects = await gitlabRepo.GetProjects();

            var localList = new ObservableCollection<Project>();

            var index = 0;
            
            foreach (var project in projects)
            {
                localList.Add(project);
                index++;

                if (index == 10)
                {
                    break;
                }
            }
            ListOfProjects = localList;
                
        }
    }
}