using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager.Views.WindowProjectDetail
{
    public partial class WindowProjectDetail : AdonisWindow
    {

        private WindowProjectDetailViewModel _windowVM;
        
        public WindowProjectDetail(WindowProjectDetailViewModel windowVm)
        {
            _windowVM = windowVm;
        }

        public void Init(int projectId)
        {
            _windowVM.Init(projectId);
            InitializeComponent();
        }
        
    }
}