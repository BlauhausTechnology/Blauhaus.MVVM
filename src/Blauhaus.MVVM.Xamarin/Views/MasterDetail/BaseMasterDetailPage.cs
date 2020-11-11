using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.MasterDetail
{
    public abstract class BaseMasterDetailPage<TViewModel, TMasterView, TDetailView> : MasterDetailPage, IView
        where TViewModel : IViewModel
        where TMasterView : Page, IMasterView
        where TDetailView : Page, IView
    {
        protected readonly TViewModel ViewModel;

        protected BaseMasterDetailPage(
            TViewModel viewModel, 
            TMasterView masterView, 
            TDetailView detailView)
        {

            ViewModel = viewModel;
            BindingContext = ViewModel;

            MasterBehavior = MasterBehavior.Popover;

            masterView.SetContainer(this);

            Master = masterView;
            Detail = detailView;  
        }

    }
}