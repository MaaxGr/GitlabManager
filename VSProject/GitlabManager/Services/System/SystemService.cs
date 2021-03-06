﻿using System.Diagnostics;

namespace GitlabManager.Services.System
{
    /// <summary>
    /// Default implementation of <see cref="ISystemService"/>
    /// Calls external applications via Process.Start
    /// </summary>
    public class SystemService : ISystemService
    {
        public void OpenBrowser(string url)
        {
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start (psi);
        }

        public void OpenFolderInVsCode(string folder)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "code",
                UseShellExecute = true,
                Arguments = folder
            };
            Process.Start (psi);
        }
        
        public void OpenFolderInExplorer(string folder)
        {
            var psi = new ProcessStartInfo
            {
                FileName = folder,
                UseShellExecute = true
            };
            Process.Start (psi);
        }
        
    }
}