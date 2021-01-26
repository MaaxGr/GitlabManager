using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitlabManager.Enums;

namespace GitlabManager.Framework
{
    /// <summary>
    /// Interfaces, that each page of the main window implements with page meta data information
    /// </summary>
    public interface IApplicationContentView
    {
        /// <summary>
        /// Page Name that is displayed in the sidebar
        /// </summary>
        string PageName { get; }

        /// <summary>
        /// Section to which the page belongs
        /// Currently Operation or Administration
        /// </summary>
        /// <see cref="GitlabManager.Enums.AppNavigationSection"/>
        AppNavigationSection Section { get; }

        /// <summary>
        /// Indicates that screen is currently loading (responsible for displaying progress indicator)
        /// </summary>
        bool IsLoading { get; set; }

        /// <summary>
        /// Init function that is called when user clicks on page in sidebar.
        /// It can contain async calls.
        /// isLoading will be automatically set to true before Init() call and
        /// automatically set to false after Init() call is finished
        /// </summary>
        Task Init();
    }
}
