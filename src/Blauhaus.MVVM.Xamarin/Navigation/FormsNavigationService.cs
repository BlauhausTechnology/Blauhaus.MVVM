using System;
using System.Threading.Tasks;
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

        private NavigationPage _currentNavigationPage;
        protected NavigationPage CurrentNavigationPage => _currentNavigationPage ??= new NavigationPage();

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

        public async Task ShowViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            var page = GetPageForViewModel<Page>(typeof(TViewModel));
            await CurrentNavigationPage.PushAsync(page, true);
        }

        public async Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter) where TViewModel : IViewModel, IInitialize<T>
        {
            var page = GetPageForViewModel<Page>(typeof(TViewModel));

            var viewModel = (TViewModel)page.BindingContext;
            await viewModel.InitializeAsync(parameter);

            await CurrentNavigationPage.PushAsync(page, true);
        }

        public async Task GoBackAsync()
        {
            if (_currentNavigationPage != null)
            {
                //have not worked out how to test this
                await _currentNavigationPage.PopAsync();
            }
        }

        public void SetCurrentNavigationView(INavigationView navigationView)
        {
            _currentNavigationPage = (NavigationPage) navigationView;
        }

        private async Task ShowMainPageAsync(Page page)
        {
            await _threadService.InvokeOnMainThreadAsync(() =>
            {
                _application.SetMainPage(page);
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