using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager.Views.WindowMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindow(WindowMainViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
        
    }
}
