using Blauhaus.MVVM.Abstractions.Navigator;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewNavigator : IViewNavigator
{
    void SetActive(IMauiViewContainer container);
}