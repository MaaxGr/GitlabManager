using System.Windows;
using System.Windows.Controls;
using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();

            TitleBarContent = BuildMenu();
        }

        private Menu BuildMenu()
        {
            var mainMenu = new Menu
            {
                MaxWidth = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Background = TitleBarBackground,
                Margin = new Thickness(5, 0, 0, 1),
                Items = { BuildFileItem(), BuildHelpItem() }
            };

            static MenuItem BuildFileItem()
            {
                var exitItem = new MenuItem(){ Header = "Exit"};

                return new MenuItem()
                {
                    Header = "File",
                    Items = {exitItem}
                };
            }

            static MenuItem BuildHelpItem()
            {
                var aboutItem = new MenuItem {Header = "About"};
                var tutorialItem = new MenuItem {Header = "Tutorial"};
                
                return new MenuItem
                {
                    Header = "Help",
                    Items = { aboutItem, tutorialItem }
                };
            }

            return mainMenu;
        }
        
    }
}
