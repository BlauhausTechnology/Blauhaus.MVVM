using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

public abstract class BaseTestAppViewModel : BaseViewModel
{
    protected readonly IServiceLocator ServiceLocator;
    protected readonly ITargetNavigator TargetNavigator;

    protected BaseTestAppViewModel(
        IServiceLocator serviceLocator,
        ITargetNavigator targetNavigator)
    {
        ServiceLocator = serviceLocator;
        TargetNavigator = targetNavigator;
    }
}