using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager.Views.WindowConnection
{
    /// <summary>
    /// ConnectionWindow extends from <see cref="AdonisWindow"/>
    /// Only minimal code is present.
    /// * Set viewModel as DataContext
    /// * Provide method for initialization of ViewModel before the window opens
    /// </summary>
    public partial class ConnectionWindow : AdonisWindow
    {

        private readonly WindowConnectionViewModel _viewModel;
        
        public ConnectionWindow(WindowConnectionViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void Init(string hostUrl, string authenticationToken)
        {
            _viewModel.Init(hostUrl, authenticationToken);
        }
    }
}