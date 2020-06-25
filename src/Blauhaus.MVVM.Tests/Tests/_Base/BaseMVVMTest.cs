using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.DeviceServices.TestHelpers.MockBuilders;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.TestHelpers.MockBuilders.Services;
using Blauhaus.MVVM.Tests.MockBuilders;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Blauhaus.TestHelpers.BaseTests;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests._Base
{
    public abstract class BaseMvvmTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();

            var threadService = new ThreadServiceMockBuilder();
            threadService.Setup<Result>();

            Services.AddSingleton(x => MockNavigationLookup.Object);
            Services.AddSingleton(x => MockFormsApplicationProxy.Object);
            Services.AddSingleton(x => MockServiceProvider.Object);
            Services.AddSingleton(x => MockAnalyticsService.Object);
            Services.AddSingleton(x => MockErrorHandlingService.Object);
            Services.AddSingleton(x => threadService.Object);
            Services.AddSingleton(x => MockConnectivityService.Object);
        }

        protected NavigationServiceMockBuilder MockNavigationService => Mocks.AddMock<NavigationServiceMockBuilder, INavigationService>().Invoke();
        protected NavigationLookupMockBuilder MockNavigationLookup => Mocks.AddMock<NavigationLookupMockBuilder, INavigationLookup>().Invoke();
        protected FormsApplicationProxyMockBuilder MockFormsApplicationProxy => Mocks.AddMock<FormsApplicationProxyMockBuilder, IFormsApplicationProxy>().Invoke();
        protected ServiceProviderMockBuilder MockServiceProvider => Mocks.AddMock<ServiceProviderMockBuilder, IServiceProvider>().Invoke();
        protected AnalyticsServiceMockBuilder MockAnalyticsService => Mocks.AddMock<AnalyticsServiceMockBuilder, IAnalyticsService>().Invoke();
        protected ErrorHandlingServiceMockBuilder MockErrorHandlingService => Mocks.AddMock<ErrorHandlingServiceMockBuilder, IErrorHandlingService>().Invoke();
        protected ConnectivityServiceMockBuilder MockConnectivityService => Mocks.AddMock<ConnectivityServiceMockBuilder, IConnectivityService>().Invoke();
        protected DialogServiceMockBuilder MockDialogService => Mocks.AddMock<DialogServiceMockBuilder, IDialogService>().Invoke();
    }
}