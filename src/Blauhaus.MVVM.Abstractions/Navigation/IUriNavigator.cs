using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.Navigation;

public interface IUriNavigator
{
    Task NavigateAsync(string uri);
}