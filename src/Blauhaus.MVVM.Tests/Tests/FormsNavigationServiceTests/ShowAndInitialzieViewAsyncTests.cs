﻿using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base;
using Blauhaus.MVVM.Xamarin.Views;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests
{
    public class ShowAndInitialzieViewAsyncTests : BaseFormsNavigationServiceTest
    {

        private TestInitializingView _testView;
        private NavigationView _testNavigationView;
        private Guid _parameter;

        public override  void Setup()
        {
            base.Setup();

            _parameter = Guid.NewGuid();
            _testView = new TestInitializingView(new TestInitializingViewModel());
            _testNavigationView = new NavigationView(MockNavigationService.Object, _testView);

            MockNavigationLookup.Where_GetViewType_returns<TestInitializingViewModel>(typeof(TestInitializingView));
            MockServiceProvider.Where_GetService_returns(_testView, typeof(TestInitializingView));
        }

        [Test]
        public async Task SHOULD_get_page_type_from_lookup_and_resolve_from_service_provider()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            MockServiceProvider.Verify_GetService_was_called_with_Type(typeof(TestInitializingView));
        }

        [Test]
        public async Task SHOULD_set_current_navigation_page()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            Assert.AreEqual(_testNavigationView.CurrentPage as TestInitializingView, _testView);
        }
        
        [Test]
        public async Task IF_no_navigation_page_has_been_set_SHOULD_set_one()
        {
            //Act
            await Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            Assert.That(_testView.Navigation, Is.Not.Null);
        }


        [Test]
        public async Task SHOULD_set_parameter()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            Assert.AreEqual(_parameter, ((TestInitializingViewModel)_testView.BindingContext).InitializedValue);
        }
        
        [Test]
        public void IF_view_lookup_returns_null_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestInitializingViewModel>(null);

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await 
                Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter), "View not registered for ViewModel");
        }

        [Test]
        public void IF_service_provider_returns_null_SHOULD_throw()
        {
            //Arrange
            MockServiceProvider.Where_GetService_returns(null, typeof(TestInitializingView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await 
                Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter), "No View of type TestView has been registered with the Ioc container");

        }

        [Test]
        public void IF_resolved_view_cannot_be_converted_to_Page_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestInitializingViewModel>(typeof(FakeView));
            MockServiceProvider.Where_GetService_returns(new FakeView(), typeof(FakeView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await 
                Sut.ShowAndInitializeViewAsync<TestInitializingViewModel, Guid>(_parameter), "View type FakeView is not a Xamarin.Forms Page");

        }
    }
}