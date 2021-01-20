using System;
using GitlabManager.Model;
using GitlabManager.Services.Database;
using GitlabManager.Services.Dialog;
using GitlabManager.Services.Gitlab;
using GitlabManager.Services.Resources;
using GitlabManager.Services.WindowOpener;
using GitlabManager.ViewModels;
using GitlabManager.Views.ConnectionWindow;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.Services.DI
{
    public static class AppDependencyInjection
    {
        
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            // create service collection object
            var serviceCollection = new ServiceCollection();
            
            // create dynamic dependency provider impl
            var dynamicDependencyProvider = new DynamicDependencyProviderImpl();
            
            // configure all services
            ConfigureServices(serviceCollection, dynamicDependencyProvider);
            
            // create service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();
            
            // set service provider to dynamic dependency provider
            dynamicDependencyProvider.ServiceProvider = ServiceProvider;
        }
        
        private static void ConfigureServices(IServiceCollection services, IDynamicDependencyProvider provider)
        {
            // Services
            services.AddSingleton(provider);
            services.AddSingleton<IDialogService>(new DialogServiceImpl());
            services.AddSingleton<IGitlabService>(new GitlabServiceImpl());
            services.AddSingleton(typeof(DatabaseService));
            services.AddSingleton<IWindowOpener>(new WindowOpenerImpl(provider));
            services.AddSingleton<IResources>(new ResourcesImpl());

            // Models
            services.AddTransient(typeof(ConnectionWindowModel));
            services.AddSingleton(typeof(PageAccountsModel)); // has to be singleton (shared with child vm)

            // ViewModels
            services.AddTransient(typeof(PageAccountsSingleAccountViewModel));
            services.AddTransient(typeof(PageAccountsViewModel));
            services.AddTransient(typeof(ConnectionWindowViewModel));
            services.AddSingleton(typeof(MainWindowViewModel));
            services.AddSingleton(typeof(ConnectionWindowViewModel));
            
            // Window
            services.AddTransient(typeof(MainWindow));
            services.AddTransient(typeof(ConnectionWindow));
        }
        
    }
}