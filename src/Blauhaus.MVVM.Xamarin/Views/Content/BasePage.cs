using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Converters;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.Content
{
    public abstract class BasePage<TViewModel> : BaseUpdateContentPage, IView
    {
        protected readonly TViewModel ViewModel;

        protected BasePage(TViewModel viewModel) 
            : base(viewModel is IUpdate)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            if (ViewModel is IAppear appearViewModel)
            {
                appearViewModel.AppearCommand.Execute();
            }
        }

        protected override void OnDisappearing()
        {
            if (ViewModel is IDisappear disappearViewModel)
            {
                disappearViewModel.DisappearCommand.Execute();
            }
        }

           
    }
}