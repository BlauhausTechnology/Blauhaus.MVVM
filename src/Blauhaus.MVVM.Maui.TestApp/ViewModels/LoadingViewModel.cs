using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Maui.TestApp.Navigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;
using Blauhaus.MVVM.Maui.TestApp.Views;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class LoadingViewModel : BaseTestAppViewModel, IAppearingViewModel
{
    public LoadingViewModel(
        IServiceLocator serviceLocator,
        INavigator navigator) 
            : base(serviceLocator, navigator)
    {
        Title = "Loading...";

        AppearCommand=serviceLocator.Resolve<AsyncExecutingCommand>()
            .WithExecute(HandleAppearingAsync);
        
        NavigateFullScreenCommand = Navigate(AppNavigation.FullScreen);
        NavigateContainerCommand = Navigate(AppNavigation.Container);

    }

    public string Status
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public IExecutingCommand AppearCommand { get; }

    public IExecutingCommand NavigateContainerCommand { get; }
    public IExecutingCommand NavigateFullScreenCommand { get; }
     

    private async Task HandleAppearingAsync()
    {
        Status = "Loading";
        await Task.Delay(2000);
        Status = "3";
        await Task.Delay(1000);
        Status = "2";
        await Task.Delay(1000);
        Status = "1";
        await Task.Delay(1000);
        Status = "Ready";
    }

}