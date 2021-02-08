using System.Collections.Generic;
using System.Windows.Input;
using GitlabManager.Framework;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.WindowOpener;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for a single Project Entry in the Projects-Page List.
    /// </summary>
    public class PageProjectsSingleProjectViewModel
    {

        #region Dependencies

        private readonly IWindowOpener _windowOpener;

        #endregion
        
        #region Public Properties
        
        public DbProject DbProject { get; set; }

        public string NameWithNamespace { get; set; }
        
        public string Description { get; set; }
        
        public string LastUpdatedAgo { get; set; }
        
        public List<string> TagList { get; set; }

        public ICommand ItemDoubleClickCommand { get; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowOpener">Service to open other windows</param>
        public PageProjectsSingleProjectViewModel(IWindowOpener windowOpener)
        {
            // init dependencies
            _windowOpener = windowOpener;
            
            // init commands
            ItemDoubleClickCommand = new AppDelegateCommand<object>(_ => OpenProjectDetailWindowExecutor());
        }
        
        /// <summary>
        /// Executor to open the detail window
        /// </summary>
        public void OpenProjectDetailWindowExecutor()
        {
            _windowOpener.OpenProjectDetailWindow(DbProject.Id);
        }
    }
    

}