using GitlabManager.Services.Logging;

namespace GitlabManager.Services.Dialog
{
    /// <summary>
    /// Default implementation of <see cref="IDialogService"/>
    /// TODO implement
    /// </summary>
    public class DialogServiceImpl : IDialogService
    {
        public void Test()
        {
            LoggingService.LogD("Test Dialog Service");
        }
    }
}