using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.BusinessLogic;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Gitlab;
using GitlabManager.Utils;

namespace GitlabManager.Model
{
    public class PageProjectsModel : AppModel
    {
        /*
         * Dependencies
         */
        private DatabaseService _databaseService;
        private IGitlabService _gitlabService;
        private GitlabProjectManager _gitlabProjectManager;

        /*
         * Internal properties
         */
        // all loaded projects
        private List<DbProject> _projects = new List<DbProject>();

        /*
         * Exposed properties for view model
         */
        // all projects that should be displayed
        public ReadOnlyCollection<DbProject> DisplayedProjectsSorted => GetDisplayedProjects();


        // currently entered search text
        public string SearchText { get; private set; }

        /*
         * Constructor
         */
        public PageProjectsModel(DatabaseService databaseService, IGitlabService gitlabService, GitlabProjectManager gitlabProjectManager)
        {
            _databaseService = databaseService;
            _gitlabService = gitlabService;
            _gitlabProjectManager = gitlabProjectManager;
        }

        /*
         * Init function
         */
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

        /*
         * Commands
         */
        public void RefreshSearch(string searchString)
        {
            SearchText = searchString;
            RaiseUpdateList();
        }

        /*
         * Utility methods
         */
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

            return collectedProjects
                .OrderByDescending(it => it.LastUpdated)
                .ToReadonlyCollection();
        }

        private void RaiseUpdateList()
        {
            RaisePropertyChanged(nameof(DisplayedProjectsSorted));
        }
    }
}