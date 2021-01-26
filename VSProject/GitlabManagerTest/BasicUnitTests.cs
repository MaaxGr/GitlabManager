using System;
using GitlabManager.Services.BusinessLogic;
using GitlabManager.Services.Gitlab;
using GitlabManager.Services.Gitlab.Client;
using Xunit;

namespace GitlabManagerTest
{
    public class BasicUnitTests
    {

        [Fact]
        public async void TestDownload()
        {
            var accessToken = Environment.GetEnvironmentVariable("GITLABMANAGER_TEST_ACCESSTOKEN");
            
            var gitlabClient = new GitlabAccountClientImpl(
                "https://gitlab.timolia.de", accessToken
            );

            var instance = new GitlabProjectLoader(gitlabClient);
            
            await instance.DownloadGitlabProject("https://gitlab.timolia.de/timolia/TCommon.git", "TCommon");
        }

        [Fact]
        public void InsertIncludeCredentialsInUrl()
        {
            var newUrl = GitlabProjectLoader.IncludeCredentialsInUrl(
                "https://gitlab.timolia.de/timolia/TCommon.git",
                "Max", "1234"
                );

            const string expectedUrl = "https://Max:1234@gitlab.timolia.de/timolia/TCommon.git";
            
            Assert.Equal(expectedUrl, newUrl);
        }
    }
}