namespace GitlabManager.Services.WindowOpener
{
    /// <summary>
    /// Service that is responsible for opening other windows inside this application
    /// </summary>
    public interface IWindowOpener
    {
        
        public void OpenConnectionWindow(string hostUrl, string authenticationToken);

        public void OpenProjectDetailWindow(int projectId);

    }
}