using System.Threading.Tasks;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Services.Logging;

namespace GitlabManager.ViewModels
{
    public class PageIssuesViewModel
        : AppViewModel, IApplicationContentView
    {
        public string PageName => "Issues";
        public AppNavigationSection Section => AppNavigationSection.Operation;

        private bool _isLoading;
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        
        public Task Init()
        {
            LoggingService.LogD("Hallo!");
            return Task.CompletedTask;
        }
    }
}