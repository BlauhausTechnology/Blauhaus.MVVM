using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

public abstract class BaseTestAppViewModel : BaseViewModel, IIsExecuting
{
    protected readonly IServiceLocator ServiceLocator;
    protected readonly INavigator Navigator;

    protected T Resolve<T>() where T : class => ServiceLocator.Resolve<T>();


    protected BaseTestAppViewModel(
        IServiceLocator serviceLocator,
        INavigator navigator)
    {
        ServiceLocator = serviceLocator;
        Navigator = navigator;
    }

    protected IExecutingCommand Navigate(NavigationTarget target) => Resolve<AsyncExecutingCommand>()
        .WithExecute(async () => await Navigator.NavigateAsync(target));
    
    public string Title
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public bool IsExecuting
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }
}