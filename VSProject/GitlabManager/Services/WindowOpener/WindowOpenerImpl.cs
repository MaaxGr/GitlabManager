using System.Windows;
using GitlabManager.Services.DI;
using GitlabManager.Views.WindowConnection;
using GitlabManager.Views.WindowProjectDetail;

namespace GitlabManager.Services.WindowOpener
{
    /// <summary>
    /// Default implementation of <see cref="IWindowOpener"/>.
    /// Window-Instances get created via <see cref="IDynamicDependencyProvider"/> service.
    /// </summary>
    public class WindowOpenerImpl : IWindowOpener
    {
        private readonly IDynamicDependencyProvider _dynamicDependencyProvider;
        
        public WindowOpenerImpl(IDynamicDependencyProvider dynamicDependencyProvider)
        {
            _dynamicDependencyProvider = dynamicDependencyProvider;
        }

        public void OpenConnectionWindow(string hostUrl, string authenticationToken)
        {
            var window = _dynamicDependencyProvider.GetInstance<ConnectionWindow>();
            window.Owner = Application.Current.MainWindow;
            window.Init(hostUrl, authenticationToken);
            window.Show();
        }

        public void OpenProjectDetailWindow(int projectId)
        {
            var window = _dynamicDependencyProvider.GetInstance<WindowProjectDetail>();
            window.Init(projectId);
            window.Show();
        }
    }
}