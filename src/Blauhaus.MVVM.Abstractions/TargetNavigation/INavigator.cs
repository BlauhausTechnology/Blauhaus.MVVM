using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface INavigator
{
    Task NavigateAsync(NavigationTarget target);
}