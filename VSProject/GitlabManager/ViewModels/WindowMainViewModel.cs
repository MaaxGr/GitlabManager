using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using GitlabManager.Enums;
using GitlabManager.Framework;
using GitlabManager.Services.Database;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.ViewModels
{
    
    /// <summary>
    /// ViewModel for the entire Main Window that includes multiple pages
    ///
    /// Based on Adonis UI Demo Template (#LOC)
    /// <see cref="https://github.com/benruehl/adonis-ui/blob/master/src/AdonisUI.Demo/ViewModels/ApplicationViewModel.cs"/>
    /// </summary>
    public class WindowMainViewModel : AppViewModel
    {
        
        private readonly ObservableCollection<IApplicationContentView> _pages;

        public ReadOnlyObservableCollection<IApplicationContentView> Pages { get; }

        public ICollectionView PagesCollectionView { get; }
        
        public ICollectionView PagesInSelectedGroupCollectionView { get; }

        private IApplicationContentView _selectedPage;

        public IApplicationContentView SelectedPage
        {
            get => _selectedPage;
            set
            {
                value ??= Pages.FirstOrDefault();

                if (value != null && !value.IsLoading)
                {
                    Task.Run(async () =>
                    {
                        value.IsLoading = true;
                        try
                        {
                            await value.Init();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }).ContinueWith(task =>
                    {
                        value.IsLoading = false;
                    });

                }

                SetProperty(ref _selectedPage, value);
            }
        }

        private readonly ObservableCollection<AppNavigationSection> _navigationGroups;

        public ReadOnlyObservableCollection<AppNavigationSection> NavigationGroups { get; }

        public ICollectionView NavigationGroupsCollectionView { get; }

        public AppNavigationSection SelectedNavigationGroup
        {
            get => _selectedPage.Section;
            set
            {
                SelectedPage = Pages.FirstOrDefault(p => p.Section == value);
                PagesInSelectedGroupCollectionView.Refresh();
            }
        }

        private IServiceProvider serviceProvider;
        
        public WindowMainViewModel(IServiceProvider serviceProvider, DatabaseService service)
        {
            this.serviceProvider = serviceProvider;
            
            _pages = new ObservableCollection<IApplicationContentView>(CreateAllPages());
            Pages = new ReadOnlyObservableCollection<IApplicationContentView>(_pages);
            PagesCollectionView = CollectionViewSource.GetDefaultView(Pages);
            PagesCollectionView.Filter = FilterPages;
            PagesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(IApplicationContentView.Section)));
            PagesInSelectedGroupCollectionView = new CollectionViewSource { Source = Pages }.View;
            PagesInSelectedGroupCollectionView.Filter = FilterPagesInSelectedGroup;

            service.Init();
            
            SelectedPage = Pages.FirstOrDefault();

            _navigationGroups = new ObservableCollection<AppNavigationSection>(Enum.GetValues(typeof(AppNavigationSection)).Cast<AppNavigationSection>());
            NavigationGroups = new ReadOnlyObservableCollection<AppNavigationSection>(_navigationGroups);
            NavigationGroupsCollectionView = CollectionViewSource.GetDefaultView(NavigationGroups);
            NavigationGroupsCollectionView.Filter = FilterNavigationGroups;
        }

        private IEnumerable<IApplicationContentView> CreateAllPages()
        {
            // yield all page view models here
            yield return serviceProvider.GetRequiredService<PageProjectsViewModel>();
            yield return serviceProvider.GetRequiredService<PageAccountsViewModel>();
            yield return serviceProvider.GetRequiredService<PageSettingsViewModel>();
        }

        private static bool FilterPages(object item)
        {
            return true;
        }

        private bool FilterPagesInSelectedGroup(object item)
        {
            var page = (IApplicationContentView)item;

            if (SelectedPage == null)
                return false;

            return page.Section == SelectedPage.Section && FilterPages(page);
        }

        private static bool FilterNavigationGroups(object item)
        {
            return true;
        }

    }
}