using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;

namespace Blauhaus.MVVM.Abstractions.Views;

public interface INavigableView : IView, IAsyncInitializable<IViewTarget>
{
    IViewTarget ViewTarget { get; }
}