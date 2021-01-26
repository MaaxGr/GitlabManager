using System;
using System.Windows;
using GitlabManager.Model;
using GitlabManager.Services.BusinessLogic;
using GitlabManager.Services.Cache;
using GitlabManager.Services.Database;
using GitlabManager.Services.Dialog;
using GitlabManager.Services.Gitlab;
using GitlabManager.Services.Resources;
using GitlabManager.Services.System;
using GitlabManager.Services.WindowOpener;
using GitlabManager.ViewModels;
using GitlabManager.Views.WindowConnection;
using GitlabManager.Views.WindowMain;
using GitlabManager.Views.WindowProjectDetail;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.Services.DI
{
    
    /// <summary>
    /// Class that is responsible for creating all dependencies in the right order.
    /// </summary>
    public class AppDependencyInjection
    {
        
        private readonly ServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDynamicDependencyProvider _dynamicDependencyProvider;
        
        /// <summary>
        /// Setup Dependency Injection
        /// - Singleton-Services have to be registered before MVVM-Classes because they depend on Services
        /// - DynamicDependencyProvider is a Service that allows dynamic access to all declared dependencies
        ///     USE WITH CAUTION! Global Dependency Locator is a bad practice, because you can't
        ///                       see in the constructor what the dependency of a class are.
        /// </summary>
        public AppDependencyInjection()
        {
            // create service collection object
            _services = new ServiceCollection();
            
            // create dynamic dependency provider impl
            var dynamicDependencyProvider = new DynamicDependencyProviderImpl();
            _dynamicDependencyProvider = dynamicDependencyProvider;

            // configure instantiation of classes
            ConfigureSingletonServices();
            ConfigureTransientMvvm();
            
            // create service provider
            _serviceProvider = _services.BuildServiceProvider();
            
            // set service provider to dynamic dependency provider
            dynamicDependencyProvider.ServiceProvider = _serviceProvider;
        }

        /// <summary>
        /// Method that provides main window out of service provider
        /// (instance is needed directly after app started)
        /// </summary>
        public Window GetMainWindow() => _serviceProvider.GetService<MainWindow>();

        /// <summary>
        /// Configure Services. Each Service only has one instance at runtime.
        /// </summary>
        private void ConfigureSingletonServices()
        {
            _services.AddSingleton(_dynamicDependencyProvider);
            _services.AddSingleton<IDialogService>(new DialogServiceImpl());
            _services.AddSingleton<IGitlabService>(new GitlabServiceImpl());
            _services.AddSingleton(typeof(DatabaseService));
            _services.AddSingleton<IWindowOpener>(new WindowOpenerImpl(_dynamicDependencyProvider));
            _services.AddSingleton<IResources>(new ResourcesImpl());
            _services.AddSingleton(typeof(GitlabProjectManager));
            _services.AddSingleton<IJsonCache>(new JsonCache());
            _services.AddSingleton<ISystemService>(new SystemService());
        }

        /// <summary>
        /// Configure MVVM Classes
        /// - Models have to be configured before view models, because VMs use models as dependencies
        /// - ViewModels have to be configured before windows because windows use view models as dependencies
        /// - Pages in Main Window are not backed by C#-View-Classes 
        /// </summary>
        private void ConfigureTransientMvvm()
        {
            ConfigureModels();
            ConfigureViewModels();
            ConfigureWindows();

            void ConfigureModels()
            {
                _services.AddTransient(typeof(ConnectionWindowModel));
                _services.AddTransient(typeof(PageAccountsModel));
                _services.AddTransient(typeof(PageProjectsModel));
                _services.AddTransient(typeof(WindowProjectDetailModel));
            }

            void ConfigureViewModels()
            {
                _services.AddTransient(typeof(PageAccountsSingleAccountViewModel));
                _services.AddTransient(typeof(PageAccountsViewModel));
                _services.AddTransient(typeof(WindowConnectionViewModel));
                _services.AddTransient(typeof(WindowMainViewModel));
                _services.AddTransient(typeof(WindowConnectionViewModel));
                _services.AddTransient(typeof(PageProjectsViewModel));
                _services.AddTransient(typeof(PageProjectsSingleProjectViewModel));
                _services.AddTransient(typeof(WindowProjectDetailViewModel));
            }

            void ConfigureWindows()
            {
                _services.AddTransient(typeof(MainWindow));
                _services.AddTransient(typeof(ConnectionWindow));
                _services.AddTransient(typeof(WindowProjectDetail));
            }
            
        }
        
    }
}