using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services;


public class NavigatorMockBuilder : BaseMockBuilder<NavigatorMockBuilder, INavigator>
{
    public void VerifyNavigateAsync(NavigationTarget target)
    {
        Mock.Verify(x => x.NavigateAsync(target));
    }
}