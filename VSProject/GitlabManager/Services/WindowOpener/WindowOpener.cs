namespace GitlabManager.Services.WindowOpener
{
    public interface IWindowOpener
    {
        
        public void OpenConnectionWindow(string hostUrl, string authenticationToken);
        
    }
}