using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Logging;
using GitlabManager.Utils;

namespace GitlabManager.Model
{
    /// <summary>
    /// Model class for the Account-Page
    /// </summary>
    public class PageAccountsModel : AppModel
    {
        /*
         * Dependencies
         */
        private readonly DatabaseService _databaseService;
        
        /*
         * Properties
         */
        // available accounts
        private List<DbAccount> _accounts = new List<DbAccount>();
        public ReadOnlyCollection<DbAccount> AccountsSorted => 
            _accounts.OrderBy(acc => acc.Identifier).ToReadonlyCollection();
        
        // currently selected account
        public DbAccount SelectedAccount { get; private set; }

        /**
         * Constructor
         */
        public PageAccountsModel(DatabaseService databaseService)
        {
            // init dependencies
            _databaseService = databaseService;
        }
        
        /*
         * Actions
         */
        public void Init()
        {
            // load accounts from db cache
            _accounts = _databaseService.Accounts.ToList();
            RaiseUpdateList();
            
            // set selected account empty
            NewAccount();
        }

        public void NewAccount()
        {
            SelectedAccount = ProvideNewEmptyAccount();
            RaiseSelectedAccountChange();
        }

        public void SetSelectedAccount(DbAccount account)
        {
            LoggingService.LogD($"Set 1: {account.Identifier}");
            SelectedAccount = account;
            RaiseSelectedAccountChange();
        }

        public void SaveAccount(DbAccount accountToSave)
        {
            // insert if id is 0, else update
            if (accountToSave.Id == 0)
            {
                AddAccount(accountToSave);
            }
            else
            {
                LoggingService.LogD($"Update xx {accountToSave.Identifier}");
                UpdateAccount(accountToSave);
            }
        }

        private void AddAccount(DbAccount accountToAdd)
        {
            // add entry to local model
            _accounts.Add(accountToAdd);
            RaiseUpdateList();

            // insert account to database
            Task.Run(() =>
            {
                _databaseService.AddAccount(accountToAdd);
                // update ui again, because id of inserted account has changed (not 0 any more)
                RaiseUpdateList();
            });
        }

        private void UpdateAccount(DbAccount accountToUpdate)
        {
            // update in local model
            SelectedAccount = accountToUpdate;
            _accounts.UpdateWhere(account => account.Id == accountToUpdate.Id, accountToUpdate);

            RaiseUpdateList();
            LoggingService.LogD("After update raise");

            // update in database
            Task.Run(() => _databaseService.UpdateAccount(accountToUpdate));
        }
        
        public void DeleteAccount(DbAccount accountToDelete)
        {
            // delete account in model
            _accounts.DeleteWhere(account => account.Id == accountToDelete.Id);
            RaiseUpdateList();
            
            // set current selected account to blank
            NewAccount();
            
            // delete account async in database
            Task.Run(() => _databaseService.DeleteAccount(accountToDelete));

            // logging
            LoggingService.LogD($"delete account! {accountToDelete.Id}");
        }
        
        private static DbAccount ProvideNewEmptyAccount()
        {
            var account = new DbAccount {Id = 0};
            return account;
        }

        private void RaiseUpdateList()
        {
            RaisePropertyChanged(nameof(AccountsSorted));
        }

        private void RaiseSelectedAccountChange() => RaisePropertyChanged(nameof(SelectedAccount));
    }
}