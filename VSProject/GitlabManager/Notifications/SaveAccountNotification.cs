using GitlabManager.Services.Database.Model;

namespace GitlabManager.Notifications
{
    public class SaveAccountNotification
    {
        public Account Account { get; }

        public SaveAccountNotification(Account account)
        {
            Account = account;
        }
    }
}