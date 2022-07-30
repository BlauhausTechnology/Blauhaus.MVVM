using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Maui.Views;

public abstract class BaseMauiShell<TViewModel> : Shell, IView<TViewModel> where TViewModel : class, IViewModel
{
    protected BaseMauiShell(TViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

    public TViewModel ViewModel { get; }

    protected override void OnAppearing()
    {
        if (ViewModel is IAppearingViewModel appearViewModel)
        {
            if (appearViewModel.AppearCommand.CanExecute(null))
            {
                appearViewModel.AppearCommand.Execute();
            }
        }
    }

    protected override void OnDisappearing()
    {
        if (ViewModel is IDisappearingViewModel disappearViewModel)
        {
            if (disappearViewModel.DisappearCommand.CanExecute(null))
            {
                disappearViewModel.DisappearCommand.Execute();
            }
        }
    }

}