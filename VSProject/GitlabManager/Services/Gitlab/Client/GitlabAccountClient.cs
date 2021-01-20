using System.Threading.Tasks;

namespace GitlabManager.Services.Gitlab.Client
{
    public interface IGitlabAccountClient
    {

        public Task<bool> IsConnectionEstablished();

    }
}