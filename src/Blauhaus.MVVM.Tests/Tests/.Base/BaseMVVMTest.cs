using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.DeviceServices.TestHelpers.MockBuilders;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.TestHelpers.MockBuilders.Services;
using Blauhaus.MVVM.Tests.MockBuilders;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.BaseTests;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.Base
{
    public abstract class BaseMvvmTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            
            Config = BuildConfig.Release;

            var threadService = new ThreadServiceMockBuilder();
            threadService.Setup<Response>();
            Services.AddServiceLocator();

            AddService(x => MockLogger.Object);
            AddService(x => MockNavigationLookup.Object);
            AddService(x => MockFormsApplicationProxy.Object);
            AddService(x => MockAnalyticsService.Object);
            AddService(x => MockErrorHandler.Object);
            AddService(x => threadService.Object);
            AddService(x => MockConnectivityService.Object);
            AddService(x => MockDialogService.Object);
            AddService(x => Config);

            MockLogger.Mock.Setup(x => x.BeginTimedScope(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns(MockLogger.MockScopeDisposable.Object);

        }

        protected IBuildConfig Config = null!;

        protected NavigationServiceMockBuilder MockNavigationService => Mocks.AddMock<NavigationServiceMockBuilder, INavigationService>().Invoke();
        protected NavigationLookupMockBuilder MockNavigationLookup => Mocks.AddMock<NavigationLookupMockBuilder, INavigationLookup>().Invoke();
        protected FormsApplicationProxyMockBuilder MockFormsApplicationProxy => Mocks.AddMock<FormsApplicationProxyMockBuilder, IFormsApplicationProxy>().Invoke();
        protected AnalyticsServiceMockBuilder MockAnalyticsService => Mocks.AddMock<AnalyticsServiceMockBuilder, IAnalyticsService>().Invoke();
        protected ErrorHandlerMockBuilder MockErrorHandler => Mocks.AddMock<ErrorHandlerMockBuilder, IErrorHandler>().Invoke();
        protected ConnectivityServiceMockBuilder MockConnectivityService => Mocks.AddMock<ConnectivityServiceMockBuilder, IConnectivityService>().Invoke();
        protected DialogServiceMockBuilder MockDialogService => Mocks.AddMock<DialogServiceMockBuilder, IDialogService>().Invoke();
        protected AnalyticsLoggerMockBuilder<TSut> MockLogger => Mocks.AddMock<AnalyticsLoggerMockBuilder<TSut>, IAnalyticsLogger<TSut>>().Invoke();
    }
}