using Blauhaus.MVVM.Abstractions.TargetNavigation;

namespace Blauhaus.MVVM.Maui.Services.TargetNavigation.Register;

public interface INavigationRegister
{
    
    Type GetViewType(NavigationTarget target);
    Type GetContainerType(NavigationTarget target);
}