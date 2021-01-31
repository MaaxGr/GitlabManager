using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Services.BusinessLogic;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Utils;

namespace GitlabManager.Model
{
    /// <summary>
    /// Model for the Project-List Page
    /// </summary>
    public class PageProjectsModel : AppModel
    {

        #region Dependencies

        private DatabaseService _databaseService;
        private GitlabProjectManager _gitlabProjectManager;

        #endregion

        #region Internal Properties

        /// <summary>
        /// List of all available projects
        /// </summary>
        private List<DbProject> _projects = new List<DbProject>();


        #endregion


        #region Exposed Properties to View Model
        
        /// <summary>
        /// All projects ordered by selected sorting
        /// </summary>
        public ReadOnlyCollection<DbProject> DisplayedProjectsSorted => GetDisplayedProjects();

        /// <summary>
        /// Mode in which the projects will be sorted
        /// </summary>
        public ProjectListSorting SortingMode { get; private set; } = ProjectListSorting.LastActivity;


        /// <summary>
        /// Currently entered SearchText value
        /// </summary>
        public string SearchText { get; private set; }

        #endregion

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseService">Service to access database</param>
        /// <param name="gitlabProjectManager">Service to manage gitlab projects</param>
        public PageProjectsModel(DatabaseService databaseService, GitlabProjectManager gitlabProjectManager)
        {
            _databaseService = databaseService;
            _gitlabProjectManager = gitlabProjectManager;
        }

        #region Public Actions

        /// <summary>
        /// Init function to load all projects
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            // clear local cached projects
            _projects.Clear();

            // load accounts from db
            var databaseAccounts = _databaseService.Accounts.ToList();

            foreach (var account in databaseAccounts)
            {
                await _gitlabProjectManager.UpdateProjectsForAccount(account);

                _projects = _databaseService.Projects.ToList();
                RaiseUpdateList();
            }
        }

        /// <summary>
        /// Refresh the search with a provided search string
        /// </summary>
        /// <param name="searchString">String that should be contained in Project Name/Namespace</param>
        public void RefreshSearch(string searchString)
        {
            SearchText = searchString;
            RaiseUpdateList();
        }
        
        /// <summary>
        /// Update the sorting mode
        /// </summary>
        /// <param name="sorting">Desired sorting mode</param>
        public void SetSortingMode(ProjectListSorting sorting)
        {
            SortingMode = sorting;
            RaiseUpdateList();
            RaisePropertyChanged(nameof(SortingMode));
        }

        #endregion

        #region Private Utility Methods

        private ReadOnlyCollection<DbProject> GetDisplayedProjects()
        {
            var collectedProjects = new List<DbProject>();

            foreach (var project in _projects)
            {
                // display all if search text is blank
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    collectedProjects.Add(project);
                    continue;
                }

                // display project if search text is contained in project name or namespace
                if (project.NameWithNamespace.ToLower().Contains(SearchText.ToLower()))
                {
                    collectedProjects.Add(project);
                }
            }

            var sortingProjects = SortingMode switch
            {
                ProjectListSorting.Alphabetical => collectedProjects.OrderBy(it => it.NameWithNamespace),
                ProjectListSorting.LastActivity => collectedProjects.OrderByDescending(it => it.LastUpdated),
                _ => throw new ArgumentOutOfRangeException()
            };

            return sortingProjects.ToReadonlyCollection();
        }

        private void RaiseUpdateList()
        {
            RaisePropertyChanged(nameof(DisplayedProjectsSorted));
        }
        
        #endregion
    }
}