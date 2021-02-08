using System.Collections.Generic;
using System.Linq;
using AdonisUI.Controls;
using Ookii.Dialogs.Wpf;

namespace GitlabManager.Services.Dialog
{
    /// <summary>
    /// Default implementation of <see cref="IDialogService"/>
    /// TODO implement
    /// </summary>
    public class DialogServiceImpl : IDialogService
    {
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

        public MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxImage icon,
            IEnumerable<IMessageBoxButtonModel> buttons)
        {
            var messageBox = new MessageBoxModel
            {
                Text = text,
                Caption = caption,
                Icon = icon,
                Buttons = buttons.ToArray(),
            };
            return MessageBox.Show(messageBox);
        }

        public void ShowErrorBox(string errorMessage)
        {
            var messageBox = new MessageBoxModel
            {
                Text = $"Unfortunately an error occurred: {errorMessage}",
                Caption = "Ups?",
                Icon = MessageBoxImage.Error,
                Buttons = new []{ MessageBoxButtons.Ok() },
            };
            MessageBox.Show(messageBox);
        }
    }
}