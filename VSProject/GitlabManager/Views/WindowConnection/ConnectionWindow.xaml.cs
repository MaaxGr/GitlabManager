using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager.Views.ConnectionWindow
{
    public partial class ConnectionWindow : AdonisWindow
    {

        private readonly ConnectionWindowViewModel _viewModel;
        
        public ConnectionWindow(ConnectionWindowViewModel viewModel)
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