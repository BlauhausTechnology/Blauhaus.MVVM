using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views
{
    public abstract class BaseContentPage<TViewModel> : ContentPage, IView
    {
        protected readonly TViewModel ViewModel;

        protected BaseContentPage(TViewModel viewModel)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }
    }
}