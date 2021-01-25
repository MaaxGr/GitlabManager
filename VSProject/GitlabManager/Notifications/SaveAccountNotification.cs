using GitlabManager.Services.Database.Model;

namespace GitlabManager.Notifications
{
    public class SaveAccountNotification
    {
        public DbAccount Account { get; }

        public SaveAccountNotification(DbAccount account)
        {
            Account = account;
        }
    }
}