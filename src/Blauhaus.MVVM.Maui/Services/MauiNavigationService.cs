using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Abstractions.Navigation.Register;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Maui.Applications;
using Blauhaus.MVVM.Maui.Views;
using Microsoft.Extensions.Logging;
using IFlyoutView = Blauhaus.MVVM.Abstractions.Views.IFlyoutView;
using IView = Blauhaus.MVVM.Abstractions.Views.IView;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiNavigationService : INavigationService
{
      private readonly IAnalyticsLogger<MauiNavigationService> _logger;
        private readonly IServiceLocator _serviceLocator;
        private readonly INavigationRegister _navigationRegister;
        private readonly IMauiApplicationProxy _application;
        private readonly IThreadService _threadService;


        private readonly Dictionary<string, INavigationView> _navigationViews = new();

        protected NavigationPage? CurrentNavigationPage
        {
            get
            {
                var currentNavigationView = _navigationViews.Values.FirstOrDefault(x => x.IsCurrent);
                return (NavigationPage?) currentNavigationView;
            }
        }

        private IFlyoutView? _currentFlyoutPage;

        public MauiNavigationService(
            IAnalyticsLogger<MauiNavigationService> logger,
            IServiceLocator serviceLocator,
            INavigationRegister navigationRegister,
            IMauiApplicationProxy application, 
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

        public Task GoBackOrShowAsync<TViewModel>()
        {
            throw new NotImplementedException();
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

                while (CurrentNavigationPage.CurrentPage != CurrentNavigationPage.RootPage)
                {
                    var page = CurrentNavigationPage.CurrentPage;

                    if (page.BindingContext is IAsyncDisposable asyncDisposable)
                        await asyncDisposable.DisposeAsync();
                 
                    if (page.BindingContext is IDisposable disposable)
                        disposable.Dispose();

                    await _threadService.InvokeOnMainThreadAsync(async () =>
                    {
                        await CurrentNavigationPage.PopAsync();
                    });
                } 
            };
        }

     
        public async Task GoBackToAsync<TViewModel>()
        {
            if (CurrentNavigationPage != null)
            {
                while (CurrentNavigationPage.BindingContext.GetType() != typeof(TViewModel))
                {
                    var page = CurrentNavigationPage.CurrentPage;

                    if (page.BindingContext is IAsyncDisposable asyncDisposable)
                        await asyncDisposable.DisposeAsync();
                 
                    if (page.BindingContext is IDisposable disposable)
                        disposable.Dispose();

                    await _threadService.InvokeOnMainThreadAsync(async () =>
                    {
                        await CurrentNavigationPage.PopAsync();
                    });
                } 
            };
        }

        public Task ShowViewAndRemoveCurrent<TViewModel>()
        {
            throw new NotImplementedException();
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
}