using System.Windows;
using GitlabManager.Services.DI;

namespace GitlabManager.Views
{
    /// <summary>
    /// Codebehind Entrypoint XAML of Project.
    /// Initializes Dependency Injection and launches Main Window
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var dependencyInjection = new AppDependencyInjection();
            MainWindow = dependencyInjection.GetMainWindow();
            MainWindow!.Show();
        }

    }

}
