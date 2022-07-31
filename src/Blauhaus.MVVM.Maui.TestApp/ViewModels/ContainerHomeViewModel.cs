using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class ContainerHomeViewModel : BaseTestAppViewModel
{
    public ContainerHomeViewModel(IServiceLocator serviceLocator, INavigator targetNavigator) : base(serviceLocator, targetNavigator)
    {
    }
}