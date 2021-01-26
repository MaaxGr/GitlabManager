namespace GitlabManager.Services.System
{
    /// <summary>
    /// Services that is responsible for calling external system services.
    /// Such es opening web browser or other programs 
    /// </summary>
    public interface ISystemService
    {

        public void OpenBrowser(string url);

        public void OpenFolderInVsCode(string folder);

        public void OpenFolderInExplorer(string folder);


    }
}