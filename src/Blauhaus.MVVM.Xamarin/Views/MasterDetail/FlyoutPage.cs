using System;
using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.MasterDetail
{
    public class FlyoutPage<TViewModel, TMenuView, TDetailView> : MasterDetailPage, IFlyoutView
        where TViewModel : IViewModel
        where TMenuView : Page, IMasterView
        where TDetailView : Page, IView
    {
        protected readonly TViewModel ViewModel;

        public FlyoutPage(
            TViewModel viewModel, 
            INavigationService navigationService,
            TMenuView masterView, 
            TDetailView detailView)
        {

            ViewModel = viewModel;
            BindingContext = ViewModel;

            MasterBehavior = MasterBehavior.Popover;

            masterView.SetContainer(this);

            Master = masterView;
            Detail = detailView;  

            navigationService.SetCurrentFlyoutView(this);
        }
         
        public void ShowDetail<TView>(TView view) where TView : IView
        {
            if (view is Page page)
            {
                Detail = page;
                IsPresented = false;
            }
            else throw new ArgumentException("View must be a Page to be set as the current Detail view");
        }
    }
}