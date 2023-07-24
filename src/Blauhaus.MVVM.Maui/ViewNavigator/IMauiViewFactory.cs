using Blauhaus.Common.ValueObjects.Navigation;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewFactory
{
    Task<Page> GetViewAsync(ViewIdentifier viewIdentifier);
}