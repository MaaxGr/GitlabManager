using GitlabManager.Framework;
using GitlabManager.Services.Logging;

namespace GitlabManager.ViewModels
{
    public class WindowProjectDetailViewModel : AppViewModel
    {

        private bool _initialized;
        private int _projectId;
        
        public string WindowTitle => GetWindowTitle();

        //public string ProjectNameWithNameSpace => _project.NameWithNamespace ?? "";

        public WindowProjectDetailViewModel()
        {
            _initialized = false;
        }

        private string GetWindowTitle()
        {
            return $"Project: {_projectId}";
        }

        public void Init(int projectId)
        {
            _projectId = projectId;
            LoggingService.LogD($"Iinit {projectId}");
            
            //RaisePropertyChanged(ProjectNameWithNameSpace);
            RaisePropertyChanged(WindowTitle);
        }
    }
}