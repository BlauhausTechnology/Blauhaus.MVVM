﻿using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.DeviceServices.TestHelpers.MockBuilders;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.TestHelpers;
using Blauhaus.MVVM.Tests.Tests.Base;
using Blauhaus.MVVM.Xamarin.Navigation;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base
{
    public class BaseFormsNavigationServiceTest: BaseMvvmTest<FormsNavigationService>
    {
        protected ServiceLocatorMockBuilder MockServiceLocator => AddMock<ServiceLocatorMockBuilder, IServiceLocator>().Invoke();

        public override void Setup()
        {
            base.Setup();
            
            AddService(x => MockServiceLocator.Object);
        }

        protected override FormsNavigationService ConstructSut()
        {

            var thread = new ThreadServiceMockBuilder();

            return new FormsNavigationService(
                new AnalyticsLoggerMockBuilder<FormsNavigationService>().Object,
                MockServiceLocator.Object,
                MockNavigationLookup.Object,
                MockFormsApplicationProxy.Object,
                thread.Object);

        }
        
    }
}