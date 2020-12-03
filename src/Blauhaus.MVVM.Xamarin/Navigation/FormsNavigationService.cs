using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Navigation
{
    public class FormsNavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationLookup _navigationLookup;
        private readonly IFormsApplicationProxy _application;
        private readonly IThreadService _threadService;


        private readonly Dictionary<string, INavigationView> _navigationViews = new Dictionary<string, INavigationView>();

        protected NavigationPage? CurrentNavigationPage
        {
            get
            {
                var currentNavigationView = _navigationViews.Values.FirstOrDefault(x => x.IsCurrent);
                return (NavigationPage?) currentNavigationView;
            }
        }

        private IFlyoutView? _currentFlyoutPage;

        public FormsNavigationService(
            IServiceProvider serviceProvider,
            INavigationLookup navigationLookup,
            IFormsApplicationProxy application, 
            IThreadService threadService)
        {
            _serviceProvider = serviceProvider;
            _navigationLookup = navigationLookup;
            _application = application;
            _threadService = threadService;
        }

        public async Task ShowMainViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            var page = GetPageForViewModel<Page>(typeof(TViewModel));
            await ShowMainPageAsync(page);
        }

        public Task ShowViewAsync<TViewModel>(string navigationStackName = "") where TViewModel : IViewModel
        {
            if (navigationStackName != "")
            {
                SetCurrentNavigationView(navigationStackName);
            }

            var page = GetPageForViewModel<Page>(typeof(TViewModel));
            return NavigateToAsync(page);
        }

        public async Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : IViewModel, IAsyncInitializable<T>
        {
            
            if (navigationStackName != "")
            {
                SetCurrentNavigationView(navigationStackName);
            }

            var page = GetPageForViewModel<Page>(typeof(TViewModel));

            var viewModel = (TViewModel)page.BindingContext;
            await viewModel.InitializeAsync(parameter);
            
            await NavigateToAsync(page);
        }

        public Task ShowDetailViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            if (_currentFlyoutPage == null)
            {
                throw new InvalidOperationException("No Flyout Page has been set");
            }
            
            var page = GetPageForViewModel<Page>(typeof(TViewModel));
            if (page is IView view)
            {
                return _threadService.InvokeOnMainThreadAsync(() => 
                    _currentFlyoutPage.ShowDetail(view));
            }

            throw new InvalidOperationException("Detail Page must implement IView");
        }

        public void SetCurrentNavigationView(string navigationStackName)
        {
            if (!_navigationViews.ContainsKey(navigationStackName))
            {
                throw new InvalidOperationException("No navigation page exists for " + navigationStackName);
            }

            foreach (var navigationView in _navigationViews.Values)
            {
                navigationView.IsCurrent = navigationView.StackName == navigationStackName;
            }

        }

        public void SetCurrentFlyoutView(IFlyoutView flyoutView)
        {
            _currentFlyoutPage = flyoutView;
        }

        public Task GoBackAsync()
        {
            if (CurrentNavigationPage != null)
            {
                return _threadService.InvokeOnMainThreadAsync(async () => 
                    await CurrentNavigationPage.PopAsync());
            };
            return Task.CompletedTask;
        }

        public Task GoBackToRootAsync()
        {
            if (CurrentNavigationPage != null)
            {
                return _threadService.InvokeOnMainThreadAsync(async () =>
                {
                    await CurrentNavigationPage.PopToRootAsync();
                });
            };
            return Task.CompletedTask;
        }

        public void SetCurrentNavigationView(INavigationView navigationView)
        {
            navigationView.IsCurrent = true;
            _navigationViews[navigationView.StackName] = navigationView;
        }

        private Task ShowMainPageAsync(Page page)
        {
            return _threadService.InvokeOnMainThreadAsync(() =>
            {
                _application.SetMainPage(page);
            });
        }
        private Task NavigateToAsync(Page page)
        {
            if (CurrentNavigationPage == null)
            {
                throw new InvalidOperationException("No NavigationPage has been set");
            }
            return _threadService.InvokeOnMainThreadAsync(() =>
            {
                CurrentNavigationPage.PushAsync(page, true);
            });
        }
          
        private TPage GetPageForViewModel<TPage>(Type viewModelType) where TPage : Page
        {
            var viewType = _navigationLookup.GetViewType(viewModelType);
            if (viewType == null)
            {
                throw new NavigationException($"No view is registered for {viewModelType.Name}");
            }

            var view = _serviceProvider.GetService(viewType);
            if (view == null)
            {
                throw new NavigationException($"No View of type {viewType.Name} has been registered with the Ioc container");
            }
            
            if (!(view is TPage page))
            {
                throw new NavigationException($"View type {viewType.Name} is not a {typeof(TPage).Name}");
            }

            return page;
        }


        #region Maybe
        
        


        
        //public async Task ShowMasterDetailViewAsync<TMasterViewModel, TDetailViewModel>() where TMasterViewModel : IViewModel where TDetailViewModel : IViewModel
        //{ 
        //    var masterDetail = new MasterDetailPage
        //    {
        //        Master =  GetPageForViewModel<Page>(typeof(TMasterViewModel)),
        //        Detail =  GetPageForViewModel<Page>(typeof(TDetailViewModel)),
        //        MasterBehavior = MasterBehavior.Popover
        //    };
        //    await ShowMainPageAsync(masterDetail);
        //    _currentMasterDetail = masterDetail;
        //}

        ////does not retain the gucking master FFS
        //public void ChangeCurrentDetailView<TDetailViewModel>() where TDetailViewModel : IViewModel
        //{
        //    var detail = GetPageForViewModel<Page>(typeof(TDetailViewModel));
        //    _currentMasterDetail.Detail = detail;
        //    _currentMasterDetail.IsPresented = false;
        //}

        //public async Task ShowMasterDetailTabsViewAsync<TMasterViewModel>(IList<Type> tabViewModelTypes) where TMasterViewModel : IViewModel
        //{
        //    var tabbedPage = new TabbedView();

        //    foreach (var tabViewModelType in tabViewModelTypes)
        //    {
        //        var tabRootPage = GetPageForViewModel<Page>(tabViewModelType);
        //        var tabNavigationRoot = new NavigationView(tabRootPage);
        //        tabbedPage.Children.Add(tabNavigationRoot);
        //    }

        //    var masterDetail = new MasterDetailPage
        //    {
        //        Master =  GetPageForViewModel<Page>(typeof(TMasterViewModel)),
        //        Detail =  tabbedPage,
        //        MasterBehavior = MasterBehavior.Popover
        //    };

        //    await ShowMainPageAsync(masterDetail);
        //    _currentMasterDetail = masterDetail;
        //}

        //public void SetCurrentNavigationView(INavigationView navigationView)
        //{
        //    _currentNavigation = (NavigationPage) navigationView;
        //}
        
        //public Task NavigateAsync<TViewModel>() where TViewModel : IViewModel
        //{
        //    var viewToShow = GetPageForViewModel<TViewModel, Page>();
        //    return _currentNavigation.PushAsync(viewToShow, true);
        //}


        //public void ShowMasterDetailNavigationView<TMasterViewModel, TDetailViewModel>() where TMasterViewModel : IViewModel where TDetailViewModel : IViewModel
        //{ 
        //    var navigationDetailView = new NavigationPage(GetPageForViewModel<TDetailViewModel, Page>());
        //    var masterDetail = new MasterDetailPage
        //    {
        //        Master =  GetPageForViewModel<TMasterViewModel, Page>(),
        //        Detail =  navigationDetailView,
        //        MasterBehavior = MasterBehavior.Popover
        //    };
        //    ShowMainPage(masterDetail);
            
        //    _currentNavigation = navigationDetailView;
        //    _currentMasterDetail = masterDetail;
        //}

        #endregion

    }
}