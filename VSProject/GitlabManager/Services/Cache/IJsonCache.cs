using GitlabManager.Services.Gitlab.Model;

namespace GitlabManager.Services.Cache
{
    /// <summary>
    /// Interface for a service that provides communication with the json cache in filesystem
    /// </summary>
    public interface IJsonCache
    {
        public void WriteProject(int projectId, JsonProject jsonProject);
        
        public JsonProject ReadProject(int projectId);
    }
}