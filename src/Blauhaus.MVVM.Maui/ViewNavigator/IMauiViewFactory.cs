using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewFactory
{
    Task<Page> GetViewAsync(ViewIdentifier viewIdentifier);
}