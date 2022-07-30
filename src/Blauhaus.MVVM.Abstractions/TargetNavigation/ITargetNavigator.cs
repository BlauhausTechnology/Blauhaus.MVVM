using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface ITargetNavigator
{
    Task NavigateAsync(NavigationTarget target);
}