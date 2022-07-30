using Blauhaus.MVVM.Abstractions.Navigation.UriNavigation;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services;

public class UriNavigatorMockBuilder : BaseMockBuilder<UriNavigatorMockBuilder, IUriNavigator>
{
    public void VerifyNavigateAsync(string route, int? times = null)
    {
        if (times is null)
        {
            Mock.Verify(x => x.NavigateAsync(route));
        }
        else
        {
            Mock.Verify(x => x.NavigateAsync(route), Times.Exactly(times.Value));
        }
    }
}