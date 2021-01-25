using GitlabManager.Services.Database.Model;

namespace GitlabManager.Notifications
{
    public class DeleteAccountNotification
    {
        public DbAccount Account { get; }

        public DeleteAccountNotification(DbAccount account)
        {
            Account = account;
        }
    }
}