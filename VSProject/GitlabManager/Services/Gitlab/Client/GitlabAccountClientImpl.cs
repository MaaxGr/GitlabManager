using System.Threading.Tasks;
using GitLabApiClient;

namespace GitlabManager.Services.Gitlab.Client
{
    public class GitlabAccountClientImpl : IGitlabAccountClient
    {

        private readonly GitLabClient _client;
        
        public GitlabAccountClientImpl(GitLabClient client)
        {
            _client = client;
        }

        
        public async Task<bool> IsConnectionEstablished()
        {
            // connection is established, if username of current session in not blank
            var session = await _client.Users.GetCurrentSessionAsync();
            return !string.IsNullOrWhiteSpace(session.Username);
        }
        
    }
}