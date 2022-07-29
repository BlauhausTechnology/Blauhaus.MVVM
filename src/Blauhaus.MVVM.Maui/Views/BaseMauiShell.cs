using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Maui.Views;

public abstract class BaseMauiShell<TViewModel> : Shell
{
    protected readonly TViewModel ViewModel;

    protected BaseMauiShell(TViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

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