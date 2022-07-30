using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.Navigator;

public interface INavigator
{
    Task NavigateAsync(NavigationTarget target);
    Task NavigateBackAsync();
}