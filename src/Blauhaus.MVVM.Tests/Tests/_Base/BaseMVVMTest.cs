using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.DeviceServices.TestHelpers.MockBuilders;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.TestHelpers.MockBuilders.Services;
using Blauhaus.MVVM.Tests.MockBuilders;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Blauhaus.TestHelpers.BaseTests;
using CSharpFunctionalExtensions;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests._Base
{
    public abstract class BaseMvvmTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            
            Config = BuildConfig.Release;

            var threadService = new ThreadServiceMockBuilder();
            threadService.Setup<Result>();

            AddService(x => MockNavigationLookup.Object);
            AddService(x => MockFormsApplicationProxy.Object);
            AddService(x => MockServiceProvider.Object);
            AddService(x => MockAnalyticsService.Object);
            AddService(x => MockErrorHandler.Object);
            AddService(x => threadService.Object);
            AddService(x => MockConnectivityService.Object);
            AddService(x => MockDialogService.Object);
            AddService(x => Config);

        }

        protected IBuildConfig Config;

        protected NavigationServiceMockBuilder MockNavigationService => Mocks.AddMock<NavigationServiceMockBuilder, INavigationService>().Invoke();
        protected NavigationLookupMockBuilder MockNavigationLookup => Mocks.AddMock<NavigationLookupMockBuilder, INavigationLookup>().Invoke();
        protected FormsApplicationProxyMockBuilder MockFormsApplicationProxy => Mocks.AddMock<FormsApplicationProxyMockBuilder, IFormsApplicationProxy>().Invoke();
        protected ServiceProviderMockBuilder MockServiceProvider => Mocks.AddMock<ServiceProviderMockBuilder, IServiceProvider>().Invoke();
        protected AnalyticsServiceMockBuilder MockAnalyticsService => Mocks.AddMock<AnalyticsServiceMockBuilder, IAnalyticsService>().Invoke();
        protected ErrorHandlerMockBuilder MockErrorHandler => Mocks.AddMock<ErrorHandlerMockBuilder, IErrorHandler>().Invoke();
        protected ConnectivityServiceMockBuilder MockConnectivityService => Mocks.AddMock<ConnectivityServiceMockBuilder, IConnectivityService>().Invoke();
        protected DialogServiceMockBuilder MockDialogService => Mocks.AddMock<DialogServiceMockBuilder, IDialogService>().Invoke();
    }
}