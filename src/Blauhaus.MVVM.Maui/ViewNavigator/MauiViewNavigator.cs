using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public class MauiViewNavigator : IViewNavigator
{
    public Task<Response> PushAsync(IReadOnlyList<ViewIdentifier> viewTarget)
    {
        throw new NotImplementedException();
    }

}