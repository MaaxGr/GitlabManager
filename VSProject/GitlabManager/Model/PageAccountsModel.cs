using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.Database;
using GitlabManager.Services.Database.Model;
using GitlabManager.Utils;

namespace GitlabManager.Model
{
    /// <summary>
    /// Model class for the Account-Page
    /// </summary>
    public class PageAccountsModel : AppModel
    {

        #region Dependenceis

        private readonly DatabaseService _databaseService;

        #endregion

        #region Private Variables

        /// <summary>
        /// Backing field for all stored accounts
        /// </summary>
        private List<DbAccount> _accounts = new List<DbAccount>();
   
        #endregion

        #region Public Properties
        
        /// <summary>
        /// All accounts sorted by Identifier exposed to view model
        /// </summary>
        public ReadOnlyCollection<DbAccount> AccountsSorted => 
            _accounts.OrderBy(acc => acc.Identifier).ToReadonlyCollection();

        /// <summary>
        /// Current selected account exposed to view model
        /// </summary>
        public DbAccount SelectedAccount { get; private set; }

        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseService">Database Service to access persistence</param>
        public PageAccountsModel(DatabaseService databaseService)
        {
            // init dependencies
            _databaseService = databaseService;
        }


        #region Actions

        /// <summary>
        /// Init the model
        /// * loads accounts for database
        /// * sets selected account to empty
        /// </summary>
        public void Init()
        {
            // load accounts from db cache
            _accounts = _databaseService.Accounts.ToList();
            RaiseUpdateList();
            
            // set selected account empty
            NewAccount();
        }

        /// <summary>
        /// Executed on NEW-Button click.
        /// Empty account should be prepared to be saved
        /// </summary>
        public void NewAccount()
        {
            SelectedAccount = ProvideNewEmptyAccount();
            RaiseSelectedAccountChange();
        }

        /// <summary>
        /// Set an account the be the currently selected one
        /// </summary>
        /// <param name="account">Currently selected account</param>
        public void SetSelectedAccount(DbAccount account)
        {
            SelectedAccount = account;
            RaiseSelectedAccountChange();
        }

        /// <summary>
        /// Save an account (update/insert)
        /// </summary>
        /// <param name="accountToSave">Account that should be saved</param>
        public void SaveAccount(DbAccount accountToSave)
        {
            // insert if id is 0, else update
            if (accountToSave.Id == 0)
            {
                AddAccount(accountToSave);
            }
            else
            {
                UpdateAccount(accountToSave);
            }
        }

        #endregion

        #region Private Utility Methods

        /// <summary>
        /// Add a new account to the database
        /// (Database operation is done async)
        /// </summary>
        /// <param name="accountToAdd"></param>
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
        
        /// <summary>
        /// Update the handed over account in the database
        /// (Database operation is done async)
        /// </summary>
        /// <param name="accountToUpdate">Account that should be updated</param>
        private void UpdateAccount(DbAccount accountToUpdate)
        {
            // update in local model
            SelectedAccount = accountToUpdate;
            _accounts.UpdateWhere(account => account.Id == accountToUpdate.Id, accountToUpdate);

            RaiseUpdateList();

            // update in database
            Task.Run(() => _databaseService.UpdateAccount(accountToUpdate));
        }
        
        /// <summary>
        /// Delete a handed over account in the database
        /// (Database operation is done async)
        /// </summary>
        /// <param name="accountToDelete">Account that should be deleted</param>
        public void DeleteAccount(DbAccount accountToDelete)
        {
            // delete account in model
            _accounts.DeleteWhere(account => account.Id == accountToDelete.Id);
            RaiseUpdateList();
            
            // set current selected account to blank
            NewAccount();
            
            // delete account async in database
            Task.Run(() => _databaseService.DeleteAccount(accountToDelete));
        }
        
        /// <summary>
        /// Create and return an empty account
        /// </summary>
        /// <returns></returns>
        private static DbAccount ProvideNewEmptyAccount()
        {
            var account = new DbAccount {Id = 0};
            return account;
        }

        /// <summary>
        /// Raise the change of all available accounts to the view model
        /// </summary>
        private void RaiseUpdateList() => RaisePropertyChanged(nameof(AccountsSorted));

        /// <summary>
        /// Raise a change of the selected account to the view model
        /// </summary>
        private void RaiseSelectedAccountChange() => RaisePropertyChanged(nameof(SelectedAccount));

        #endregion


    }
}