using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class MainContainerViewModel : BaseTestAppViewModel
{
    public MainContainerViewModel(IServiceLocator serviceLocator, INavigator navigator) 
        : base(serviceLocator, navigator)
    {
        Title = "Start!";
    }
}