using System;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.Tests.MockBuilders
{
    public class NavigationLookupMockBuilder : BaseMockBuilder<NavigationLookupMockBuilder, INavigationLookup>
    {
        public NavigationLookupMockBuilder Where_GetViewType_returns(Type type)
        {
            Mock.Setup(x => x.GetViewType(It.IsAny<Type>())).Returns(type);
            return this;
        }

        public NavigationLookupMockBuilder Where_GetViewType_returns<TViewModel>(Type type)
        {
            Mock.Setup(x => x.GetViewType(typeof(TViewModel))).Returns(type);
            return this;
        }
    }
}