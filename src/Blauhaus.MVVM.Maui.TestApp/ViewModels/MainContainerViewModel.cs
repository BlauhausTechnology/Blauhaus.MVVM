using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class MainContainerViewModel : BaseTestAppViewModel
{
    public MainContainerViewModel(
        IServiceLocator serviceLocator, 
        INavigator targetNavigator) 
            : base(serviceLocator, targetNavigator)
    {
    }
}