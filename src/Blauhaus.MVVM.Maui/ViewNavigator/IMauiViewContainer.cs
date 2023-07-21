using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewContainer : INavigableView, IAsyncInitializable<ViewIdentifier>
{
    Task PushAsync(Page page);
    Task PopAsync();
}