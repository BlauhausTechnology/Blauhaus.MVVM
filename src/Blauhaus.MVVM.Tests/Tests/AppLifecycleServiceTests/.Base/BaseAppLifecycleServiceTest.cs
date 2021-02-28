using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Tests.Tests.Base;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blauhaus.MVVM.Tests.Tests.AppLifecycleServiceTests.Base
{
    public abstract class BaseAppLifecycleServiceTest : BaseMvvmTest<AppLifecycleService>
    {
        protected MockBuilder<IAppLifecycleHandler> MockHandlerOne = null!;
        protected MockBuilder<IAppLifecycleHandler> MockHandlerTwo = null!;

        public override void Setup()
        {
            base.Setup();

            MockHandlerOne = new MockBuilder<IAppLifecycleHandler>();
            MockHandlerOne.Mock.Setup(x => x.HandleAppStateChangeAsync(It.IsAny<AppLifecycleState>())).ReturnsAsync(Response.Success);
            MockHandlerTwo = new MockBuilder<IAppLifecycleHandler>();
            MockHandlerTwo.Mock.Setup(x => x.HandleAppStateChangeAsync(It.IsAny<AppLifecycleState>())).ReturnsAsync(Response.Success);
            
            Services.AddSingleton(MockHandlerOne.Object);
            Services.AddSingleton(MockHandlerTwo.Object);
        }
    }
}