using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Navigation;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services;

public class ViewNavigatorMockBuilder : BaseMockBuilder<ViewNavigatorMockBuilder, IViewNavigator>
{
    public void VerifyNavigateAsync(IViewTarget viewTarget)
    {
        Mock.Verify(x => x.NavigateAsync(viewTarget));
    }   
    public void VerifyNavigateAsync(IViewTarget viewTarget, Times times)
    {
        Mock.Verify(x => x.NavigateAsync(viewTarget), times);
    }   
    public void VerifyNavigateAsync(ViewIdentifier viewIdentifier)
    {
        Mock.Verify(x => x.NavigateAsync(ViewTarget.Create(viewIdentifier)));
    }
    public void VerifyNavigateAsync(ViewIdentifier viewIdentifier, Times times)
    {
        Mock.Verify(x => x.NavigateAsync(ViewTarget.Create(viewIdentifier)), times);
    }
    public void VerifyGoBackAsync()
    {
        Mock.Verify(x => x.GoBackAsync());
    }
    public void VerifyGoBackAsync(Times times)
    {
        Mock.Verify(x => x.GoBackAsync(), times);
    }
}