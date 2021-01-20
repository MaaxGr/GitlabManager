using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Model;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.DI;
using GitlabManager.Services.Logging;
using GitlabManager.Utils;
using GitlabManager.Views.ConnectionWindow;

namespace GitlabManager.ViewModels
{
    public class PageAccountsViewModel : ViewModel, IApplicationContentView
    {
        /*
         * Page Meta
         */
        // Name of page in sidebar
        public string PageName => "Accounts";

        // Section in sidebar
        public AppNavigationSection Section => AppNavigationSection.Administration;

        // Page loading state
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /*
         * Dependencies
         */
        private readonly PageAccountsModel _pageModel;
        private readonly IDynamicDependencyProvider _dynamicDependencyProvider;

        /*
         * Properties
         */
        // All available accounts
        public ReadOnlyCollection<PageAccountsSingleAccountViewModel> Accounts =>
            _pageModel.Accounts.Select(CreateAccountViewModel).ToReadonlyCollection();

        // Currently selected account
        public PageAccountsSingleAccountViewModel SelectedAccountSidebar
        {
            get => FindSelectedViewModelInAccountList();
            set => _pageModel.SetSelectedAccount(value.StoredAccount);
        }

        public PageAccountsSingleAccountViewModel SelectedAccountDetail
            => FindSelectedViewModelInAccountList() ?? CreateAccountViewModel(_pageModel.SelectedAccount);

        /*
         * Commands
         */
        public ICommand NewAccountCommand { get; }

        /*
         * Constructor
         */
        public PageAccountsViewModel(PageAccountsModel pageModel, IDynamicDependencyProvider dynamicDependencyProvider)
        {
            // init dependencies
            _pageModel = pageModel;
            _pageModel.PropertyChanged += AccountModelPropertyChangedHandler;
            _dynamicDependencyProvider = dynamicDependencyProvider;
            
            // init commands
            NewAccountCommand = new AppDelegateCommand<object>(_ => NewAccountCommandExecutor());
        }

        /*
        * View Init Block (on view enter)
        */
        public Task Init()
        {
            // reload accounts from model
            _pageModel.Init();
            return Task.CompletedTask;
        }

        /*
         * Commands
         */
        private void NewAccountCommandExecutor()
        {
            _pageModel.NewAccount();
        }

        /*
         * Utilities
         */
        private void AccountModelPropertyChangedHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(PageAccountsModel.Accounts):
                    LoggingService.LogD("Update accounts in vm...");
                    RaisePropertyChanged(nameof(Accounts));
                    RaisePropertyChanged(nameof(SelectedAccountSidebar));
                    RaisePropertyChanged(nameof(SelectedAccountDetail));
                    break;
                case nameof(PageAccountsModel.SelectedAccount):
                    LoggingService.LogD("Update selected account in vm...");
                    RaisePropertyChanged(nameof(SelectedAccountSidebar));
                    RaisePropertyChanged(nameof(SelectedAccountDetail));
                    break;
            }
        }

        private PageAccountsSingleAccountViewModel CreateAccountViewModel(Account account)
        {
            
            var accountVm = _dynamicDependencyProvider.GetInstance<PageAccountsSingleAccountViewModel>();
            accountVm.StoredAccount = account;
            accountVm.Id = account.Id;
            accountVm.Identifier = account.Identifier;
            accountVm.Description = account.Description;
            accountVm.HostUrl = account.HostUrl;
            accountVm.AuthenticationToken = account.AuthenticationToken;
            return accountVm;
        }
        
        private PageAccountsSingleAccountViewModel FindSelectedViewModelInAccountList()
        {
            var sidebarAccount = Accounts.FirstOrDefault(vm => vm.Id == _pageModel.SelectedAccount.Id);
            LoggingService.LogD($"Sidebar: {sidebarAccount}");
            return sidebarAccount;
        }

        private void OpenConnectionCheckWindow()
        {
            var connectionWindow = _dynamicDependencyProvider.GetInstance<ConnectionWindow>();
            connectionWindow.Owner = Application.Current.MainWindow;
        }
    }
}