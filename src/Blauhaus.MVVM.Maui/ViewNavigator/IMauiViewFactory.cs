using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewFactory
{
    Task<Page> GetViewAsync(IViewTarget viewTarget);
}