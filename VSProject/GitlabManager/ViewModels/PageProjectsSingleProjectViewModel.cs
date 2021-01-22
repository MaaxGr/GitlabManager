using System.Collections.Generic;
using System.Windows.Input;
using GitlabManager.Framework;
using GitlabManager.Services.Database.Model;
using GitlabManager.Services.WindowOpener;

namespace GitlabManager.ViewModels
{
    public class PageProjectsSingleProjectViewModel
    {
        /*
         * Dependencies
         */
        private IWindowOpener _windowOpener;

        public Project Project { get; set; }

        public string NameWithNamespace { get; set; }
        
        public string Description { get; set; }
        
        public string LastUpdatedAgo { get; set; }
        
        public List<string> TagList { get; set; }

        public ICommand ItemDoubleClickCommand { get; }

        public PageProjectsSingleProjectViewModel(IWindowOpener windowOpener)
        {
            // init dependencies
            _windowOpener = windowOpener;
            
            // init commands
            ItemDoubleClickCommand = new AppDelegateCommand<object>(_ => OpenProjectDetailWindow());
        }
        
        /*
         * Actions
         */
        public void OpenProjectDetailWindow()
        {
            _windowOpener.OpenProjectDetailWindow(Project.Id);
        }
    }
    

}