using System.Threading.Tasks;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Services.Logging;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for the complete Issue-Page
    /// TODO not implemented atm
    /// </summary>
    public class PageIssuesViewModel
        : AppViewModel, IApplicationContentView
    {
        #region Page Meta data

        public string PageName => "Issues";
        public AppNavigationSection Section => AppNavigationSection.Operation;

        private bool _isLoading;
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion
        
        
        
        public Task Init()
        {
            LoggingService.LogD("Hallo!");
            return Task.CompletedTask;
        }
    }
}