﻿using System.Windows;
using GitlabManager.Services.DI;
using GitlabManager.Views.ConnectionWindow;

namespace GitlabManager.Services.WindowOpener
{
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
        
    }
}