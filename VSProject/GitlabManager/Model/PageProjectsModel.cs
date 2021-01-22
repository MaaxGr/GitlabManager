using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Framework;
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

        /*
         * Internal properties
         */
        // all loaded projects
        private List<Project> _projects = new List<Project>();

        /*
         * Exposed properties for view model
         */
        // all projects that should be displayed
        public ReadOnlyCollection<Project> DisplayedProjectsSorted => GetDisplayedProjects();


        // currently entered search text
        public string SearchText { get; private set; }

        /*
         * Constructor
         */
        public PageProjectsModel(DatabaseService databaseService, IGitlabService gitlabService)
        {
            _databaseService = databaseService;
            _gitlabService = gitlabService;
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
                var gitlabClient = _gitlabService.GetGitlabClient(account.HostUrl, account.AuthenticationToken);
                var (success, errorMessage) = await gitlabClient.IsConnectionEstablished();
                if (!success) continue; // ignore accounts with invalid connection

                var apiProjects = await gitlabClient.GetProjects();

                foreach (var apiProject in apiProjects)
                {
                    var project = BuildProject(account.Id, apiProject);
                    _projects.Add(project);
                }

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
        private Project BuildProject(int accountId, GitLabApiClient.Models.Projects.Responses.Project apiProject)
        {
            var lastUpdatedUnixTime = DateTimeUtils.CreateDateTimeFromIso8691(apiProject.LastActivityAt)
                .ToUnixTimestamp();
            
            return new Project()
            {
                AccountId = accountId,
                Description = apiProject.Description,
                NameWithNamespace = apiProject.NameWithNamespace,
                LastUpdated = lastUpdatedUnixTime,
                TagList = apiProject.TagList.ToArray()
            };
        }

        private ReadOnlyCollection<Project> GetDisplayedProjects()
        {
            var collectedProjects = new List<Project>();

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