using GitlabManager.Services.Gitlab;
using Xunit;

namespace GitlabManagerTest
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var instance = new GitlabServiceImpl();

            var projects = await instance.GetProjects();
            Assert.True(projects.Count > 0);
        }
    }
}
