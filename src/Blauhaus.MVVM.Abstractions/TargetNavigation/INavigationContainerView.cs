using Blauhaus.MVVM.Abstractions.Views;
using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface INavigationContainerView : IView
{
    Task NavigateAsync(NavigationTarget target);
}