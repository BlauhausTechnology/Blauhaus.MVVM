using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Abstractions.Navigation.Register;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Blauhaus.MVVM.Xamarin.Views.Navigation;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;
// ReSharper disable SuspiciousTypeConversion.Global

namespace Blauhaus.MVVM.Xamarin.Navigation
{
    public class FormsNavigationService : INavigationService
    {
        private readonly IAnalyticsLogger<FormsNavigationService> _logger;
        private readonly IServiceLocator _serviceLocator;
        private readonly INavigationRegister _navigationRegister;
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
            IAnalyticsLogger<FormsNavigationService> logger,
            IServiceLocator serviceLocator,
            INavigationRegister navigationRegister,
            IFormsApplicationProxy application, 
            IThreadService threadService)
        {
            _logger = logger;
            _serviceLocator = serviceLocator;
            _navigationRegister = navigationRegister;
            _application = application;
            _threadService = threadService;
        }

        public async Task SetMainViewAsNavigationRootAsync<TViewModel>(string navigationStackName = "") where TViewModel : class, IViewModel
        {
            var rootPage = GetPageForViewModel<Page>(typeof(TViewModel));

            if (rootPage.BindingContext is IAsyncInitializable initializable)
            {
                await initializable.InitializeAsync();
            }             

            var navigationView = new NavigationView(this, rootPage, navigationStackName);

            SetCurrentNavigationView(navigationView);            

            await _threadService.InvokeOnMainThreadAsync(() =>
            {
                _application.SetMainPage(navigationView);
            });

        }

        public async Task SetAndInitializeMainViewAsNavigationRootAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : class, IViewModel, IAsyncInitializable<T>
        {
            var rootPage = GetPageForViewModel<Page>(typeof(TViewModel));

            var viewModel = (IAsyncInitializable<T>) rootPage.BindingContext;
            await viewModel.InitializeAsync(parameter);

            var navigationView = new NavigationView(this, rootPage, navigationStackName);

            SetCurrentNavigationView(navigationView);            

            await _threadService.InvokeOnMainThreadAsync(() =>
            {
                _application.SetMainPage(navigationView);
            });
        }

        public Task ShowMainViewAsync<TViewModel>() where TViewModel : class, IViewModel
        {
            return ShowMainViewAsync(typeof(TViewModel));
        }

        public Task ShowMainViewAsync(Type viewModelType)
        {
            var page = GetPageForViewModel<Page>(viewModelType);
            return ShowMainPageAsync(page);
        }

        public Task ShowViewAsync<TViewModel>(string navigationStackName = "") where TViewModel : IViewModel
        {
            if (navigationStackName != "")
            {
                SetCurrentNavigationStack(navigationStackName);
            }

            var page = GetPageForViewModel<Page>(typeof(TViewModel));
            return NavigateToAsync(page);
        }

        public async Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : IViewModel, IAsyncInitializable<T>
        {
            if (navigationStackName != "")
            {
                SetCurrentNavigationStack(navigationStackName);
            }

            var page = GetPageForViewModel<Page>(typeof(TViewModel));

            var viewModel = (TViewModel)page.BindingContext;
            await viewModel.InitializeAsync(parameter);
            
            await NavigateToAsync(page);
        }

        public async Task ShowAndInitializeMainViewAsync<TViewModel, T>(T parameter) where TViewModel : IViewModel, IAsyncInitializable<T>
        {
            var page = GetPageForViewModel<Page>(typeof(TViewModel));

            var viewModel = (TViewModel)page.BindingContext;
            await viewModel.InitializeAsync(parameter);
            
            await ShowMainPageAsync(page);
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

        public void SetCurrentNavigationStack(string navigationStackName)
        {
            //todo consider registering view models with their navigation stack in the first place so we don't need to remember to do this all the time
            
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

        public async Task GoBackAsync()
        {
            if (CurrentNavigationPage != null)
            {
                if (CurrentNavigationPage.CurrentPage.BindingContext is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync();
                
                if (CurrentNavigationPage.CurrentPage.BindingContext is IDisposable disposable)
                    disposable.Dispose();

                await _threadService.InvokeOnMainThreadAsync(async () => 
                    await CurrentNavigationPage.PopAsync());
            };
        }

        public async Task GoBackToRootAsync()
        {
            if (CurrentNavigationPage != null)
            {
                foreach (var page in CurrentNavigationPage.Pages)
                {
                    if (page.BindingContext is IAsyncDisposable asyncDisposable)
                        await asyncDisposable.DisposeAsync();
                 
                    if (page.BindingContext is IDisposable disposable)
                        disposable.Dispose();
                }

                await _threadService.InvokeOnMainThreadAsync(async () =>
                {
                    await CurrentNavigationPage.PopToRootAsync();
                });
            };
        }

        public async Task GoBackToAsync<TViewModel>()
        {
            if (CurrentNavigationPage != null)
            {
                await _threadService.InvokeOnMainThreadAsync(async () =>
                {
                    var pages = CurrentNavigationPage.Pages.Reverse().ToArray();
                    var pageIsFound = false;

                    foreach (var page in pages)
                    {
                        var pageViewModelType = page.BindingContext.GetType();
                        if (pageViewModelType is TViewModel)
                        {
                            _logger.LogDebug("{ViewModelType} is {RequiredViewModel}", pageViewModelType, typeof(TViewModel));
                            pageIsFound = true;
                        }

                        if (pageIsFound)
                        {
                            _logger.LogDebug("Page already found, ignoring {ViewModelType}", pageViewModelType);
                        }
                        else
                        {

                            if (page.BindingContext is IAsyncDisposable asyncDisposable)
                                await asyncDisposable.DisposeAsync();

                            if (page.BindingContext is IDisposable disposable)
                                disposable.Dispose();

                            _logger.LogDebug("{ViewModelType} is not {RequiredViewModel}. Continue navigating back...", pageViewModelType, typeof(TViewModel));
                            await CurrentNavigationPage.PopAsync();
                        }
                    }
                });

            };
        }

        public void SetCurrentNavigationView(INavigationView navigationView)
        {
            navigationView.IsCurrent = true;
            _navigationViews[navigationView.StackName] = navigationView;
        }

        private async Task ShowMainPageAsync(Page page)
        {
            if (page.BindingContext is IAsyncInitializable initializable)
            {
                await initializable.InitializeAsync();
            }        
            
            await _threadService.InvokeOnMainThreadAsync(() =>
            {
                _application.SetMainPage(page);
            });
        }
        
        private async Task NavigateToAsync(Page page)
        {
            if (CurrentNavigationPage == null)
            {
                throw new InvalidOperationException("No NavigationPage has been set");
            }
            
            if (page.BindingContext is IAsyncInitializable initializable)
            {
                await initializable.InitializeAsync();
            } 

            await _threadService.InvokeOnMainThreadAsync(() =>
            {
                CurrentNavigationPage.PushAsync(page, true);
            });
        }
          
        private TPage GetPageForViewModel<TPage>(Type viewModelType) where TPage : Page
        {
            var viewType = _navigationRegister.GetViewType(viewModelType);
            if (viewType == null)
            {
                throw new NavigationException($"No view is registered for {viewModelType.Name}");
            }

            var view = _serviceLocator.Resolve(viewType);
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