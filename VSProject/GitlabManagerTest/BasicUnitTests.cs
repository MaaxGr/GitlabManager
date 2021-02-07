using GitlabManager.Services.BusinessLogic;
using Xunit;

namespace GitlabManagerTest
{
    public class BasicUnitTests
    {

        /// <summary>
        /// Test insert include credentials in URL
        /// </summary>
        [Fact]
        public void InsertIncludeCredentialsInUrl()
        {
            var newUrl = GitlabProjectDownloader.IncludeCredentialsInUrl(
                "https://gitlab.timolia.de/timolia/TCommon.git",
                "Max", "1234"
                );

            const string expectedUrl = "https://Max:1234@gitlab.timolia.de/timolia/TCommon.git";
            
            Assert.Equal(expectedUrl, newUrl);
        }
    }
}