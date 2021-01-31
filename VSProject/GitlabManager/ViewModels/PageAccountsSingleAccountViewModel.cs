using System.Linq;
using System.Windows.Input;
using AdonisUI.Controls;
using GitlabManager.Framework;
using GitlabManager.Notifications;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Dialog;
using GitlabManager.Services.WindowOpener;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel that represents a Single Account in the Accounts-Page.
    /// </summary>
    public class PageAccountsSingleAccountViewModel : AppViewModel
    {
        #region Dependencies

        private IDialogService _dialogService;
        private IWindowOpener _windowOpener;

        #endregion

        #region Properties for View

        public DbAccount StoredAccount { get; set; }
        
        public int Id { get; set; }

        public string Identifier { get; set; }
        
        public string Description { get; set;  }
        
        public string HostUrl { get; set; }
        
        public string AuthenticationToken { get; set; }
        
        public ICommand DeleteCommand { get; }
        
        public ICommand SaveCommand { get;  }
        
        public ICommand TestTokenCommand { get; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowOpener">Service to open other windows</param>
        /// <param name="dialogService">Service to open dialogs</param>
        public PageAccountsSingleAccountViewModel(IWindowOpener windowOpener, IDialogService dialogService)
        {
            // init dependencies
            _dialogService = dialogService;
            _windowOpener = windowOpener;
            
            // init commands
            DeleteCommand = new AppDelegateCommand<object>(_ => DeleteCommandExecutor());
            SaveCommand = new AppDelegateCommand<string>(_ => SaveCommandExecutor());
            TestTokenCommand = new AppDelegateCommand<object>(_ => TestTokenCommandExecutor());
        }

        /// <summary>
        /// Executor that is called on delete button click
        /// </summary>
        private void DeleteCommandExecutor()
        {
            var messageBox = new MessageBoxModel
            {
                Text = $"Are you sure that the account '{Identifier}' should be removed?",
                Caption = $"Remove Account: {Identifier}",
                Icon = MessageBoxImage.Question,
                Buttons = MessageBoxButtons.OkCancel().ToArray(),
            };
            var x = MessageBox.Show(messageBox);

            if (x == MessageBoxResult.OK)
            {
                MessengerInstance.Send(new DeleteAccountNotification(StoredAccount));
            }
        }

        /// <summary>
        /// Executor that is called on save button click
        /// </summary>
        private void SaveCommandExecutor()
        {
            PatchAccount();
            MessengerInstance.Send(new SaveAccountNotification(StoredAccount));
        }

        /// <summary>
        /// Executor that is called, when test button was pressed
        /// </summary>
        private void TestTokenCommandExecutor()
        {
            _windowOpener.OpenConnectionWindow(HostUrl, AuthenticationToken);
        }

        /// <summary>
        /// Update stored account properties with properties of view model
        /// </summary>
        private void PatchAccount()
        {
            StoredAccount.Identifier = Identifier;
            StoredAccount.Description = Description;
            StoredAccount.HostUrl = HostUrl;
            StoredAccount.AuthenticationToken = AuthenticationToken;
        }

        /// <summary>
        /// Compares the quality of to view models
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            // LoggingService.LogD($"Equality check: {obj} current: {this}");
            
            if (obj is PageAccountsSingleAccountViewModel vm)
            {
                return vm.Id == Id;
            }

            return false;
        }

        /// <summary>
        /// String representation of view model
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Account VM {Id}; {Identifier}";
        }
    }
}