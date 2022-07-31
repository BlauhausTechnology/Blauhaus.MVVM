using Blauhaus.MVVM.Abstractions.Views;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface INavigationContainerView : IView, IInitializable<ViewIdentifier>
{
    ViewIdentifier ViewIdentifier { get; }
    Task NavigateAsync(NavigationTarget target);
}