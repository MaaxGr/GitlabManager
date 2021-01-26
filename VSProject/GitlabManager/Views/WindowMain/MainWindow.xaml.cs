﻿using System.Windows;
using System.Windows.Controls;
using AdonisUI.Controls;
using GitlabManager.ViewModels;

namespace GitlabManager.Views.WindowMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// TODO Menu content should definitely moved to xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindow(WindowMainViewModel viewModel)
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
