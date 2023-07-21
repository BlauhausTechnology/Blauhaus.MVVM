using Blauhaus.MVVM.Abstractions.TargetNavigation;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public interface IMauiViewNavigator : IViewNavigator
{
    void SetActive(IMauiViewContainer container);
}