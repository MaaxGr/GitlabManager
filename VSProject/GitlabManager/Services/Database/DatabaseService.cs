using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Services.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace GitlabManager.Services.Database
{
    
    /// <summary>
    /// DatabaseService stores a copy of the database entries in memory,
    /// so that the current data can be accessed synchronously.
    ///
    /// DatabaseService is the only class that is able to write to the database.
    /// For that it provides utility methods that can be called from other classes.
    /// </summary>
    public class DatabaseService
    {

        private readonly DatabaseContext _context = new DatabaseContext();

        public ObservableCollection<DbAccount> Accounts;
        public ObservableCollection<DbProject> Projects;

        /// <summary>
        /// Init database service
        /// </summary>
        public void Init()
        {
            //_context.Database.EnsureDeleted();
            CreateProd();
        }

        /// <summary>
        /// ensure database is created
        /// </summary>
        private void CreateProd()
        {
            _context.Database.EnsureCreated();
            _context.Accounts.Load();
            _context.Projects.Load();
            
            Accounts = _context.Accounts.Local.ToObservableCollection();
            Projects = _context.Projects.Local.ToObservableCollection();
        }

        /// <summary>
        /// Delete a specified account in the database
        /// </summary>
        /// <param name="account">Account that should be deleted</param>
        public void DeleteAccount(DbAccount account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        /// <summary>
        /// Add a new account the database
        /// </summary>
        /// <param name="account">Account that should be added</param>
        public void AddAccount(DbAccount account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }
        
        /// <summary>
        /// Update a existing account in the database
        ///
        /// Source: Stackoverflow
        /// https://stackoverflow.com/questions/36856073/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-of-this-t
        /// #LOC
        /// </summary>
        /// <param name="account">Account that should be updated</param>
        public void UpdateAccount(DbAccount account)
        {
            // get local
            var local = _context.Set<DbAccount>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(account.Id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                _context.Entry(local).State = EntityState.Detached;
            }
                
            // set Modified flag in your entry
            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();

        }

        /// <summary>
        /// Get a project by internal project id from cached database projects
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DbProject GetProjectById(int projectId)
        {
            return Projects.First(pr => pr.Id == projectId);
        }
        
        /// <summary>
        /// Get a project by accountId and external gitlab project id from cached projects
        /// </summary>
        /// <param name="accountId">Internal account id</param>
        /// <param name="gitlabProjectId">External gitlab project id</param>
        /// <returns></returns>
        public DbProject GetProject(int accountId, int gitlabProjectId)
        {
            return Projects
                .FirstOrDefault(pr => pr.Account.Id == accountId && pr.GitlabProjectId == gitlabProjectId);
        }

        /// <summary>
        /// Async method to insert project into database  
        /// </summary>
        /// <param name="project">Project to save</param>
        /// <returns></returns>
        public async Task<int> InsertProject(DbProject project)
        {
            _context.Add(project);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Async method to update project in database
        /// </summary>
        /// <param name="project">Project to save</param>
        /// <returns></returns>
        public async Task<int> UpdateProject(DbProject project)
        {
            _context.Update(project);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update the last account last updated unix timestamp by accountId
        /// </summary>
        /// <param name="accountId">accountId target account</param>
        /// <param name="lastProjectUpdateAt">UNIX timestamp</param>
        public void UpdateAccountLastProjectUpdate(int accountId, long lastProjectUpdateAt)
        {
            var account = Accounts.First(ac => ac.Id == accountId);
            account.LastProjectUpdateAt = lastProjectUpdateAt;
            UpdateAccount(account);
        }

        /// <summary>
        /// Update local project folder
        /// </summary>
        /// <param name="project">Project that should be updated</param>
        /// <param name="folderName">Local name of folder</param>
        public void UpdateLocalFolderForProject(DbProject project, string folderName)
        {
            project.LocalFolder = folderName;
            _context.Update(project);
            _context.SaveChanges();
        }

    }
}