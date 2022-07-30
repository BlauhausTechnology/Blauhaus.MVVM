using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.Navigation.UriNavigation;

public interface IUriNavigator
{
    Task NavigateAsync(string uri);
}