using System.Threading.Tasks;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface INavigator
{
    Task NavigateAsync(NavigationTarget target);

    Task<INavigationContainerView> GetContainerViewAsync(NavigationTarget target, ViewIdentifier viewIdentifier);
    Task<IView> GetViewAsync(NavigationTarget target, ViewIdentifier viewIdentifier);
}