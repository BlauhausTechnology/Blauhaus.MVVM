using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

public abstract class BaseTestAppViewModel : BaseViewModel, IIsExecuting
{
    private readonly IServiceLocator _serviceLocator;

    protected BaseTestAppViewModel(IServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
    }

    
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