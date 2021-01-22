using GitlabManager.Services.Database.Model;

namespace GitlabManager.Notifications
{
    public class DeleteAccountNotification
    {
        public Account Account { get; }

        public DeleteAccountNotification(Account account)
        {
            Account = account;
        }
    }
}