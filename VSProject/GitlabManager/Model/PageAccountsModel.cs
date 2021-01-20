using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Gitlab;
using GitlabManager.Services.Logging;
using GitlabManager.Utils;

namespace GitlabManager.Model
{
    public class PageAccountsModel : AppModel
    {
        /*
         * Dependencies
         */
        private readonly DatabaseService _databaseService;
        private readonly IGitlabService _gitlabService;
        
        /*
         * Properties
         */
        // available accounts
        private List<Account> _accounts = new List<Account>();
        public ReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
        public ReadOnlyCollection<Account> AccountsSorted => 
            _accounts.OrderBy(acc => acc.Identifier).ToReadonlyCollection();
        
        // currently selected account
        public Account SelectedAccount { get; private set; }

        /**
         * Constructor
         */
        public PageAccountsModel(DatabaseService databaseService, IGitlabService gitlabService)
        {
            // init dependencies
            _databaseService = databaseService;
            _gitlabService = gitlabService;
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

        public void SetSelectedAccount(Account account)
        {
            LoggingService.LogD($"Set 1: {account.Identifier}");
            SelectedAccount = account;
            RaiseSelectedAccountChange();
        }

        public void SaveAccount(Account accountToSave)
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

        private void AddAccount(Account accountToAdd)
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

        private void UpdateAccount(Account accountToUpdate)
        {
            // update in local model
            SelectedAccount = accountToUpdate;
            _accounts.UpdateWhere(account => account.Id == accountToUpdate.Id, accountToUpdate);

            RaiseUpdateList();
            LoggingService.LogD("After update raise");

            // update in database
            Task.Run(() => _databaseService.UpdateAccount(accountToUpdate));
        }
        
        public void DeleteAccount(Account accountToDelete)
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
        
        private static Account ProvideNewEmptyAccount()
        {
            var account = new Account {Id = 0};
            return account;
        }

        private void RaiseUpdateList()
        {
            RaisePropertyChanged(nameof(Accounts));
            RaisePropertyChanged(nameof(AccountsSorted));
        }

        private void RaiseSelectedAccountChange() => RaisePropertyChanged(nameof(SelectedAccount));
    }
}