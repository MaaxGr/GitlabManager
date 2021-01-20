using GitlabManager.Services.Logging;

namespace GitlabManager.Services.Dialog
{
    public class DialogServiceImpl : IDialogService
    {
        public void Test()
        {
            LoggingService.LogD("Test Dialog Service");
        }
    }
}