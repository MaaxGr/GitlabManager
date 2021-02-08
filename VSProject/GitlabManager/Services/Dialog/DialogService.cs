using System.Collections.Generic;
using AdonisUI.Controls;

namespace GitlabManager.Services.Dialog
{
    /// <summary>
    /// Service for creating and presenting dialogs to the user
    /// </summary>
    public interface IDialogService
    {
        
        public string SelectFolderDialog(string description);

        public MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxImage icon,
            IEnumerable<IMessageBoxButtonModel> buttons);

        public void ShowErrorBox(string errorMessage);

    }
}