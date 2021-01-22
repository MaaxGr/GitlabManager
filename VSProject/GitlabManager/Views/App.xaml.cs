using System.Windows;
using GitlabManager.Services.DI;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDependencyInjection.Init();
            
            MainWindow = AppDependencyInjection.ServiceProvider.GetRequiredService<WindowMain.MainWindow>();
            MainWindow.Show();
        }

    }

}
