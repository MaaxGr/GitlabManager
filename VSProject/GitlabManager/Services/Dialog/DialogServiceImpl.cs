using GitlabManager.Services.Logging;
using Ookii.Dialogs.Wpf;

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

        public string SelectFolderDialog(string description)
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = description, UseDescriptionForTitle = true
            };

            var result = dialog.ShowDialog();

            if (result == null || !(bool) result) return null;
            
            return dialog.SelectedPath;
        }
    }
}