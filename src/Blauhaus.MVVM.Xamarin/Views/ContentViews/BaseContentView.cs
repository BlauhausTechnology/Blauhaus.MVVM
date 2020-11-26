using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Views.Content;

namespace Blauhaus.MVVM.Xamarin.Views.ContentViews
{
    public class BaseContentView<TViewModel> : BaseUpdateContentView, IView
    {
        protected readonly TViewModel ViewModel;

        protected BaseContentView(TViewModel viewModel) 
            : base(viewModel is IUpdate)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }
         

    }
}