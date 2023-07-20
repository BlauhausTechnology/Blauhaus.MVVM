using Blauhaus.MVVM.Abstractions.Views;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface INavigationContainerView : IView, IInitializable<ViewIdentifier>
{
    ViewIdentifier ContainerViewIdentifier { get; }
    Task NavigateAsync(NavigationTarget target, IView view);
}