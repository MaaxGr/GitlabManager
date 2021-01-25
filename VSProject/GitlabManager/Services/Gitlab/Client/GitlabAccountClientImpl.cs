using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabApiClient.Models.Projects.Responses;

namespace GitlabManager.Services.Gitlab.Client
{
    public class GitlabAccountClientImpl : IGitlabAccountClient
    {
        private readonly GitLabClient _client;

        public GitlabAccountClientImpl(GitLabClient client)
        {
            _client = client;
        }


        public async Task<Tuple<bool, string>> IsConnectionEstablished()
        {
            try
            {
                var session = await _client.Users.GetCurrentSessionAsync();
                
                // connection is established, if username of current session in not blank
                return string.IsNullOrWhiteSpace(session.Username)
                    ? new Tuple<bool, string>(false, "Username is null or empty!")
                    : new Tuple<bool, string>(true, "");
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }
        }

        public async Task<IList<Project>> GetAllProjects()
        {
            return await _client.Projects.GetAsync(x => { x.IsMemberOf = true; });
        }

        public async Task<IList<Project>> GetAllProjectsAfter(DateTime dateTime)
        {
            return await _client.Projects.GetAsync(x =>
            {
                x.IsMemberOf = true;
                x.LastActivityAfter = dateTime;
            });
        }
        
    }
}