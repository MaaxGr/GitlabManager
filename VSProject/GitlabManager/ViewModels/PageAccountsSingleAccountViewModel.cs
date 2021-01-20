using System;
using System.Linq;
using System.Windows.Input;
using AdonisUI.Controls;
using GitlabManager.Framework;
using GitlabManager.Model;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.Dialog;
using GitlabManager.Services.Logging;
using GitlabManager.Services.WindowOpener;

namespace GitlabManager.ViewModels
{
    public class PageAccountsSingleAccountViewModel : ViewModel
    {
        /*
         * Dependencies
         */
        private PageAccountsModel _pageAccountsModel;
        private IDialogService _dialogService;
        private IWindowOpener _windowOpener;
        
        /*
         * Properties
         */
        public Account StoredAccount { get; set; }
        
        public int Id { get; set; }

        public string Identifier { get; set; }
        
        public string Description { get; set;  }
        
        public string HostUrl { get; set; }
        
        public string AuthenticationToken { get; set; }
        
        public ICommand DeleteCommand { get; }
        
        public ICommand SaveCommand { get;  }
        
        public ICommand TestTokenCommand { get; }

        public PageAccountsSingleAccountViewModel(PageAccountsModel pageAccountsModel, IWindowOpener windowOpener, IDialogService dialogService)
        {
            // init dependencies
            _pageAccountsModel = pageAccountsModel;
            _dialogService = dialogService;
            _windowOpener = windowOpener;
            
            // init commands
            this.DeleteCommand = new AppDelegateCommand<object>(DeleteCommandExecutor);
            this.SaveCommand = new AppDelegateCommand<string>(SaveCommandExecutor);
            this.TestTokenCommand = new AppDelegateCommand<object>(_ => TestTokenCommandExecutor());
            
            dialogService.Test();
        }

        private void DeleteCommandExecutor(Object o)
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
                LoggingService.LogD("DELTE Bla");
                _pageAccountsModel.DeleteAccount(StoredAccount);
            }
        }

        private void SaveCommandExecutor(Object o)
        {
            LoggingService.LogD($"Save: {Id} {Identifier}");

            PatchAccount();
            _pageAccountsModel.SaveAccount(StoredAccount);
        }

        private void TestTokenCommandExecutor()
        {
            _windowOpener.OpenConnectionWindow(HostUrl, AuthenticationToken);
        }

        public void PatchAccount()
        {
            StoredAccount.Identifier = Identifier;
            StoredAccount.Description = Description;
            StoredAccount.HostUrl = HostUrl;
            StoredAccount.AuthenticationToken = AuthenticationToken;
        }

        public override bool Equals(object? obj)
        {
            // LoggingService.LogD($"Equality check: {obj} current: {this}");
            
            if (obj is PageAccountsSingleAccountViewModel vm)
            {
                return vm.Id == Id;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Account VM {Id}; {Identifier}";
        }
    }
}