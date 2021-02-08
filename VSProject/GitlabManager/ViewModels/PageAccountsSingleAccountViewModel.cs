using System;
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

        /// <summary>
        /// Service to open dialogs
        /// </summary>
        private readonly IDialogService _dialogService;
        
        /// <summary>
        /// Service to open other windows
        /// </summary>
        private readonly IWindowOpener _windowOpener;

        #endregion

        #region Properties for View
        
        /// <summary>
        /// Currently stored model account
        /// </summary>
        public DbAccount StoredAccount { get; set; }

        /// <summary>
        /// Id of account
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Current Identifier Text-Input value
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Current Description Text-Input  value
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Current HostURL Text-Input  value
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Current AuthenticationToken Text-Input  value
        /// </summary>
        public string AuthenticationToken { get; set; }
        
        /// <summary>
        /// Command that is called on delete account button click
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Command that is called on save account button click
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command that is called on test connection button click
        /// </summary>
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
            var result = _dialogService.ShowMessageBox(
                $"Are you sure that the account '{Identifier}' should be removed?",
                $"Remove Account: {Identifier}",
                MessageBoxImage.Question,
                MessageBoxButtons.OkCancel()
            );

            if (result == MessageBoxResult.OK)
            {
                MessengerInstance.Send(new DeleteAccountNotification(StoredAccount));
            }
        }

        /// <summary>
        /// Executor that is called on save button click
        /// </summary>
        private void SaveCommandExecutor()
        {
            try
            {
                PatchAccount();
                MessengerInstance.Send(new SaveAccountNotification(StoredAccount));
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Executor that is called, when test button was pressed
        /// </summary>
        private void TestTokenCommandExecutor()
        {
            try
            {
                _windowOpener.OpenConnectionWindow(HostUrl, AuthenticationToken);
            }
            catch (Exception e)
            {
                _dialogService.ShowErrorBox(e.Message);
                throw;
            }
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
        /// String representation of view model
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Account VM {Id}; {Identifier}";
        }
    }
}