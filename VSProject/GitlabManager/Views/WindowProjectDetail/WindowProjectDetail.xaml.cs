using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager.Views.WindowProjectDetail
{
    /// <summary>
    /// Codebehind ProjectDetail Window with minimal logic
    /// * Initialize DataContext
    /// * Provide Method for Window initialization before window will be opened
    /// </summary>
    public partial class WindowProjectDetail : AdonisWindow
    {

        private WindowProjectDetailViewModel _windowVM;
        
        public WindowProjectDetail(WindowProjectDetailViewModel windowVm)
        {
            DataContext = windowVm;
            _windowVM = windowVm;
        }

        public void Init(int projectId)
        {
            _windowVM.Init(projectId);
            InitializeComponent();
        }
        
    }
}