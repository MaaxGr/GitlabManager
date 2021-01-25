using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitLabApiClient.Models.Projects.Responses;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Gitlab;
using GitlabManager.Utils;

namespace GitlabManager.Services.BusinessLogic
{
    public class GitlabProjectManager
    {
        /*
         * Dependencies
         */
        private DatabaseService _databaseService;
        private IGitlabService _gitlabService;

        /*
         * Constructor
         */
        public GitlabProjectManager(DatabaseService databaseService, IGitlabService gitlabService)
        {
            _databaseService = databaseService;
            _gitlabService = gitlabService;
        }

        public async Task UpdateProjectsForAccount(DbAccount account)
        {
            // find projects to update
            var projectsToUpdate = await GetProjectsToUpdate(account);

            // no projects to update
            if (projectsToUpdate.Count == 0) return;

            // update each project
            foreach (var gitlabProject in projectsToUpdate)
            {
                await UpdateProjectForAccount(account, gitlabProject);
            }

            // find greatest update stamp
            var greatestUpdateStamp = projectsToUpdate
                .Max(project => DateTimeUtils.CreateDateTimeFromIso8691(project.LastActivityAt))
                .ToUnixTimestamp();
            
            // update last update stamp in accounts table
            _databaseService.UpdateAccountLastProjectUpdate(account.Id, greatestUpdateStamp);
        }

        private async Task UpdateProjectForAccount(DbAccount account, Project gitlabProject)
        {
            var oldDbProject = _databaseService.GetProject(account.Id, gitlabProject.Id);

            if (oldDbProject == null)
            {
                await CreateDbProject(account.Id, gitlabProject);
            }
            else
            {
                await PatchDbProject(oldDbProject, gitlabProject);
            }
        }

        private async Task<IList<Project>> GetProjectsToUpdate(DbAccount account)
        {
            var gitlabClient = _gitlabService.GetGitlabClient(account.HostUrl, account.AuthenticationToken);

            var lastUpdatedProjectsMillis = account.LastProjectUpdateAt;
            var lastUpdatedProjectsDateTime = DateTimeUtils.UnixMillisToDateTime(lastUpdatedProjectsMillis);

            return lastUpdatedProjectsMillis == 0
                ? await gitlabClient.GetAllProjects()
                : await gitlabClient.GetAllProjectsAfter(lastUpdatedProjectsDateTime);
        }

        private async Task<int> PatchDbProject(DbProject dbProject, Project gitlabProject)
        {
            var lastUpdatedUnixTime = DateTimeUtils.CreateDateTimeFromIso8691(gitlabProject.LastActivityAt)
                .ToUnixTimestamp();

            dbProject.LastUpdated = lastUpdatedUnixTime;
            dbProject.Description = gitlabProject.Description;
            dbProject.NameWithNamespace = gitlabProject.NameWithNamespace;
            dbProject.TagList = gitlabProject.TagList.ToArray();

            return await _databaseService.UpdateProject(dbProject);
        }

        private async Task<int> CreateDbProject(int accountId, Project gitlabProject)
        {
            var lastUpdatedUnixTime = DateTimeUtils.CreateDateTimeFromIso8691(gitlabProject.LastActivityAt)
                .ToUnixTimestamp();

            var dbProject = new DbProject()
            {
                AccountId = accountId,
                GitlabProjectId = gitlabProject.Id,
                Description = gitlabProject.Description,
                LastUpdated = lastUpdatedUnixTime,
                NameWithNamespace = gitlabProject.NameWithNamespace,
                TagList = gitlabProject.TagList.ToArray()
            };
            return await _databaseService.InsertProject(dbProject);
        }
    }
}