using GitlabManager.Services.Database.Model;

namespace GitlabManager.Notifications
{
    
    /// <summary>
    /// Notification Message to communicate a SaveAccount-Event from
    /// <see cref="GitlabManager.ViewModels.PageAccountsSingleAccountViewModel"/>
    /// to <see cref="GitlabManager.ViewModels.PageAccountsViewModel"/>
    /// </summary>
    public class SaveAccountNotification
    {
        public DbAccount Account { get; }

        public SaveAccountNotification(DbAccount account)
        {
            Account = account;
        }
    }
}