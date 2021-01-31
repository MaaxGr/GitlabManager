using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Model;
using GitlabManager.Notifications;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.DI;
using GitlabManager.Services.Logging;
using GitlabManager.Utils;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for the complete Accounts-Page
    /// </summary>
    public class PageAccountsViewModel : AppViewModel, IApplicationContentView
    {

        #region Page Meta Data

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

        #endregion

        #region Dependencies

        private readonly PageAccountsModel _pageModel;
        private readonly IDynamicDependencyProvider _dynamicDependencyProvider;

        #endregion

        #region Public Properties for View

        /// <summary>
        /// All available accounts
        /// </summary>
        public ReadOnlyCollection<PageAccountsSingleAccountViewModel> Accounts =>
            _pageModel.AccountsSorted.Select(CreateAccountViewModel).ToReadonlyCollection();

        /// <summary>
        /// Currently selected account in sidebar
        /// </summary>
        public PageAccountsSingleAccountViewModel SelectedAccountSidebar
        {
            get => FindSelectedViewModelInAccountList();
            set => _pageModel.SetSelectedAccount(value.StoredAccount);
        }

        /// <summary>
        /// Currently selected account in detail area
        /// </summary>
        public PageAccountsSingleAccountViewModel SelectedAccountDetail
            => FindSelectedViewModelInAccountList() ?? CreateAccountViewModel(_pageModel.SelectedAccount);

        
        /// <summary>
        /// Command that is executed if NEW-Button was pressed
        /// </summary>
        public ICommand NewAccountCommand { get; }

        #endregion
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageModel">Page Model</param>
        /// <param name="dynamicDependencyProvider">Service get dependencies dynamically</param>
        public PageAccountsViewModel(PageAccountsModel pageModel, IDynamicDependencyProvider dynamicDependencyProvider)
        {
            // init dependencies
            _pageModel = pageModel;
            _pageModel.PropertyChanged += AccountModelPropertyChangedHandler;
            _dynamicDependencyProvider = dynamicDependencyProvider;

            // init commands
            NewAccountCommand = new AppDelegateCommand<object>(_ => NewAccountCommandExecutor());

            // init notification handlers
            MessengerInstance.Register<DeleteAccountNotification>(
                this, notification => _pageModel.DeleteAccount(notification.Account)
            );
            MessengerInstance.Register<SaveAccountNotification>(
                this, notification => _pageModel.SaveAccount(notification.Account)
            );
        }

        #region Public Methods
        
        /// <summary>
        /// Init page
        /// </summary>
        /// <returns></returns>
        public Task Init()
        {
            // reload accounts from model
            _pageModel.Init();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Pass new account message to model
        /// </summary>
        private void NewAccountCommandExecutor()
        {
            _pageModel.NewAccount();
        }
        
        #endregion


        #region Private Utility Methods

        
        private void AccountModelPropertyChangedHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(PageAccountsModel.AccountsSorted):
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

        private PageAccountsSingleAccountViewModel CreateAccountViewModel(DbAccount account)
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

        #endregion
        
        
    }
}