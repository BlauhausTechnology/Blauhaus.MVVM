using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.Tabs
{ 
    public abstract class BaseTabbedPage<TViewModel> : TabbedPage, IView where TViewModel : IViewModel
    {
        protected readonly TViewModel ViewModel;

        protected BaseTabbedPage(TViewModel viewModel)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }
    }

}