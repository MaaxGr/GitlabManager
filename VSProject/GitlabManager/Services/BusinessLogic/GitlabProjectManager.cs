using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitLabApiClient.Models.Projects.Responses;
using GitlabManager.Services.Cache;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Gitlab;
using GitlabManager.Services.Gitlab.Model;
using GitlabManager.Utils;

namespace GitlabManager.Services.BusinessLogic
{
    /// <summary>
    /// BusinessLogic class that handles loading, updating and caching projects
    /// that come from the Gitlab API and will be stored
    /// - in the local sqlite database (Only base data plus meta data from this application)
    /// - on the file system as json files (Complete download of project endpoint)
    /// </summary>
    public class GitlabProjectManager
    {
        /*
         * Dependencies
         */
        // communication to sqlite database
        private readonly DatabaseService _databaseService;
        
        // communication to gitlab api
        private readonly IGitlabService _gitlabService;
        
        // communication with json cache in file system
        private readonly IJsonCache _jsonCache;
        
        /*
         * Constructor
         */
        public GitlabProjectManager(DatabaseService databaseService, IGitlabService gitlabService, IJsonCache jsonCache)
        {
            _databaseService = databaseService;
            _gitlabService = gitlabService;
            _jsonCache = jsonCache;
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
                .Max(project => /*DateTimeUtils.CreateDateTimeFromIso8691()*/ project.LastActivityAt.ToUnixTimestamp());
            
            
            // update last update stamp in accounts table
            _databaseService.UpdateAccountLastProjectUpdate(account.Id, greatestUpdateStamp);
        }

        private async Task UpdateProjectForAccount(DbAccount account, JsonProject gitlabProject)
        {
            var oldDbProject = _databaseService.GetProject(account.Id, gitlabProject.Id);

            if (oldDbProject == null)
            {
                await CreateDbProject(account, gitlabProject);
            }
            else
            {
                await PatchDbProject(oldDbProject, gitlabProject);
            }
        }

        private async Task<IList<JsonProject>> GetProjectsToUpdate(DbAccount account)
        {
            var gitlabClient = _gitlabService.GetGitlabClient(account.HostUrl, account.AuthenticationToken);

            var lastUpdatedProjectsMillis = account.LastProjectUpdateAt;
            var lastUpdatedProjectsDateTime = DateTimeUtils.UnixMillisToDateTime(lastUpdatedProjectsMillis);

            return lastUpdatedProjectsMillis == 0
                ? await gitlabClient.GetAllProjects()
                : await gitlabClient.GetAllProjectsAfter(lastUpdatedProjectsDateTime);
        }

        private async Task PatchDbProject(DbProject dbProject, JsonProject gitlabProject)
        {
            var lastUpdatedUnixTime = gitlabProject.LastActivityAt.ToUnixTimestamp();
            
            dbProject.LastUpdated = lastUpdatedUnixTime;
            dbProject.Description = gitlabProject.Description;
            dbProject.NameWithNamespace = gitlabProject.NameWithNamespace;
            dbProject.TagList = gitlabProject.TagList.ToArray();

            var projectId = await _databaseService.UpdateProject(dbProject);
            _jsonCache.WriteProject(projectId, gitlabProject);
        }

        private async Task CreateDbProject(DbAccount account, JsonProject gitlabProject)
        {
            
            var lastUpdatedUnixTime = gitlabProject.LastActivityAt.ToUnixTimestamp();

            var dbProject = new DbProject
            {
                Account = account,
                GitlabProjectId = gitlabProject.Id,
                Description = gitlabProject.Description,
                LastUpdated = lastUpdatedUnixTime,
                NameWithNamespace = gitlabProject.NameWithNamespace,
                TagList = gitlabProject.TagList.ToArray()
            };
            var projectId = await _databaseService.InsertProject(dbProject);
            _jsonCache.WriteProject(projectId, gitlabProject);
        }

        public DbProject GetProject(int projectId)
        {
            return _databaseService.GetProjectById(projectId);
        }

        public async Task<JsonProject> GetCachedProjectMeta(DbProject project)
        {
            // read from cache
            var cachedProjectMeta = _jsonCache.ReadProject(project.GitlabProjectId);
            
            // return if in cache
            if (cachedProjectMeta != null) 
                return cachedProjectMeta;
            
            // write into cache
            var account = project.Account;
            var gitlabClient = _gitlabService.GetGitlabClient(account.HostUrl, account.AuthenticationToken);

            var gitlabProject = await gitlabClient.GetProjectById(project.GitlabProjectId);
            _jsonCache.WriteProject(gitlabProject.Id, gitlabProject);
            
            // read again
            return _jsonCache.ReadProject(project.GitlabProjectId);
        }
        
        
    }
}