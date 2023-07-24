using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewContainer : INavigableView
{
    Task PushAsync(Page page);
    Task PopAsync();
}