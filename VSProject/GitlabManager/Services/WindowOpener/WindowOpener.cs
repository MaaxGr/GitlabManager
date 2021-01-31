namespace GitlabManager.Services.WindowOpener
{
    /// <summary>
    /// Service that is responsible for opening other windows inside this application
    /// </summary>
    public interface IWindowOpener
    {
        
        /// <summary>
        /// Open the connection window
        /// </summary>
        /// <param name="hostUrl">Gitlab Instance HostURL</param>
        /// <param name="authenticationToken">Gitlab Private token</param>
        public void OpenConnectionWindow(string hostUrl, string authenticationToken);

        /// <summary>
        /// Open detail window for a specified project
        /// </summary>
        /// <param name="projectId">internal project id</param>
        public void OpenProjectDetailWindow(int projectId);

    }
}