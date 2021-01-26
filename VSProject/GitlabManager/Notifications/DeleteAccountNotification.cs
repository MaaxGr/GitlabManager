using GitlabManager.Services.Database.Model;

namespace GitlabManager.Notifications
{
    /// <summary>
    /// Notification Message to communicate a DeleteAccount-Event from
    /// <see cref="GitlabManager.ViewModels.PageAccountsSingleAccountViewModel"/>
    /// to <see cref="GitlabManager.ViewModels.PageAccountsViewModel"/>
    /// </summary>
    public class DeleteAccountNotification
    {
        public DbAccount Account { get; }

        public DeleteAccountNotification(DbAccount account)
        {
            Account = account;
        }
    }
}