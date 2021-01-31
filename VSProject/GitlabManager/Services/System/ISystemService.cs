namespace GitlabManager.Services.System
{
    /// <summary>
    /// Services that is responsible for calling external system services.
    /// Such es opening web browser or other programs 
    /// </summary>
    public interface ISystemService
    {

        /// <summary>
        /// Open specified URL in default web browser 
        /// </summary>
        /// <param name="url">URL to open</param>
        public void OpenBrowser(string url);

        /// <summary>
        /// Open Folder in VSCode App
        /// </summary>
        /// <param name="folder">Folder that should be opened</param>
        public void OpenFolderInVsCode(string folder);

        /// <summary>
        /// Open Folder in Windows-Explorer
        /// </summary>
        /// <param name="folder">Folder that should be opened</param>
        public void OpenFolderInExplorer(string folder);


    }
}