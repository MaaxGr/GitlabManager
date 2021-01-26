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
using GitlabManager.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.ViewModels
{
    
    /// <summary>
    /// ViewModel for the entire Main Window that includes multiple pages
    /// (TODO Probably a lot of that stuff can be simplified or extracted in a model)
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
                    LoggingService.LogD("Before load");
                    Task.Run(async () =>
                    {
                        LoggingService.LogD("before isloading");
                        value.IsLoading = true;
                        LoggingService.LogD("before isloading 2");
                        try
                        {
                            await value.Init();
                            LoggingService.LogD("after init 2");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }

                        LoggingService.LogD("After init");
                    }).ContinueWith(task =>
                    {
                        LoggingService.LogD("disable loading");
                        value.IsLoading = false;
                    });

                }

                SetProperty(ref _selectedPage, value);
                LoggingService.LogD($"Group: {SelectedNavigationGroup}");
                LoggingService.LogD("After raise change");
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

        private bool _isReadOnly;

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetProperty(ref _isReadOnly, value);
        }

        private bool _isDeveloperMode;

        public bool IsDeveloperMode
        {
            get => _isDeveloperMode;
            set
            {
                SetProperty(ref _isDeveloperMode, value);
                PagesCollectionView.Refresh();
                NavigationGroupsCollectionView.Refresh();
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
            yield return serviceProvider.GetRequiredService<PageProjectsViewModel>();
            yield return new PageIssuesViewModel();
            yield return serviceProvider.GetRequiredService<PageAccountsViewModel>();
        }

        private bool FilterPages(object item)
        {
            var page = (IApplicationContentView)item;
            return true;
        }

        private bool FilterPagesInSelectedGroup(object item)
        {
            var page = (IApplicationContentView)item;

            if (SelectedPage == null)
                return false;

            return page.Section == SelectedPage.Section && FilterPages(page);
        }

        private bool FilterNavigationGroups(object item)
        {
            var group = (AppNavigationSection)item;

            return true;
        }

    }
}