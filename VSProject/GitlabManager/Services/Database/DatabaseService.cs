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

        public void Init()
        {
            //_context.Database.EnsureDeleted();
            CreateProd();
        }

        private void CreateProd()
        {
            _context.Database.EnsureCreated();
            _context.Accounts.Load();
            _context.Projects.Load();
            
            Accounts = _context.Accounts.Local.ToObservableCollection();
            Projects = _context.Projects.Local.ToObservableCollection();
        }

        public void DeleteAccount(DbAccount account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public void AddAccount(DbAccount account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }
        
        //https://stackoverflow.com/questions/36856073/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-of-this-t
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

        public DbProject GetProjectById(int projectId)
        {
            return Projects.First(pr => pr.Id == projectId);
        }
        
        public DbProject GetProject(int accountId, int gitlabProjectId)
        {
            return Projects
                .FirstOrDefault(pr => pr.Account.Id == accountId && pr.GitlabProjectId == gitlabProjectId);
        }

        public async Task<int> InsertProject(DbProject project)
        {
            _context.Add(project);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProject(DbProject project)
        {
            _context.Update(project);
            return await _context.SaveChangesAsync();
        }

        public void UpdateAccountLastProjectUpdate(int accountId, long lastProjectUpdateAt)
        {
            var account = Accounts.First(ac => ac.Id == accountId);
            account.LastProjectUpdateAt = lastProjectUpdateAt;
            UpdateAccount(account);
        }

        public void UpdateLocalFolderForProject(DbProject project, string folderName)
        {
            project.LocalFolder = folderName;
            _context.Update(project);
            _context.SaveChanges();
        }

    }
}