using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services;

public class ViewNavigatorMockBuilder : BaseMockBuilder<ViewNavigatorMockBuilder, IViewNavigator>
{
    public void VerifyNavigateAsync(IViewTarget viewTarget)
    {
        Mock.Verify(x => x.NavigateAsync(viewTarget));
    }   
    public void VerifyNavigateAsync(ViewIdentifier viewIdentifier)
    {
        Mock.Verify(x => x.NavigateAsync(ViewTarget.Create(viewIdentifier)));
    }
    public void VerifyGoBackAsync()
    {
        Mock.Verify(x => x.GoBackAsync());
    }
}