using GitlabManager.Services.Gitlab.Model;

namespace GitlabManager.Services.Cache
{
    /// <summary>
    /// Interface for a service that provides communication with the json cache in filesystem
    /// </summary>
    public interface IJsonCache
    {
        /// <summary>
        /// Write project to json cache
        /// </summary>
        /// <param name="projectId">internal project id</param>
        /// <param name="jsonProject">JsonProject that was fetched from gitlab api</param>
        public void WriteProject(int projectId, JsonProject jsonProject);
        
        /// <summary>
        /// Read project metadata by internal project id from cache
        /// </summary>
        /// <param name="projectId">internal project id</param>
        /// <returns>JsonProject object</returns>
        public JsonProject ReadProject(int projectId);
    }
}