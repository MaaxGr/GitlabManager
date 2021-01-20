using System.Collections.ObjectModel;
using System.Linq;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Logging;
using Microsoft.EntityFrameworkCore;

namespace GitlabManager.Services.Database
{

    public class DatabaseService
    {

        private readonly DatabaseContext _context = new DatabaseContext();

        public ObservableCollection<Account> Accounts;

        public DatabaseService()
        {
            LoggingService.LogD("Create Database...");
            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void Init()
        {
            _context.Accounts.Load();
            Accounts = _context.Accounts.Local.ToObservableCollection();
        }

        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public void AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }
        
        //https://stackoverflow.com/questions/36856073/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-of-this-t
        public void UpdateAccount(Account account)
        {
            // get local
            var local = _context.Set<Account>()
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
        
    }
}