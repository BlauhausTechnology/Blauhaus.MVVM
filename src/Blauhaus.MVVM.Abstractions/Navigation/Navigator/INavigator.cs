using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.Navigation.Navigator;

public interface INavigator
{
    Task NavigateAsync(NavigationTarget target);
    Task NavigateBackAsync();
}