using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class LoadingViewModel : BaseTestAppViewModel, IAppearingViewModel
{
    public LoadingViewModel(
        IServiceLocator serviceLocator) 
            : base(serviceLocator)
    {
        Title = "Loading...";

        AppearCommand=serviceLocator.Resolve<AsyncExecutingCommand>()
            .WithExecute(HandleAppearingAsync);
    }

    public string Status
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public IExecutingCommand AppearCommand { get; }

    
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