using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base;
using Blauhaus.MVVM.Xamarin.Views.Navigation;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests
{
    public class ShowViewAsyncTests : BaseFormsNavigationServiceTest
    {

        private TestView _testView;
        private NavigationView _testNavigationView;

        public override  void Setup()
        {
            base.Setup();

            _testView = new TestView(new TestViewModel());
            _testNavigationView = new NavigationView(MockNavigationService.Object, _testView);

            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(typeof(TestView));
            MockServiceLocator.Where_Resolve_returns(_testView, typeof(TestView));
        }
        
        [Test]
        public async Task IF_ViewModel_is_IAsyncInitializable_SHOULD_initialize()
        {
            //Arrange 
            Sut.SetCurrentNavigationView(_testNavigationView);
            var testAsyncInitializableView = new TestAsyncInitializableView(new TestAsyncInitializableViewModel());
            MockNavigationLookup.Where_GetViewType_returns<TestAsyncInitializableViewModel>(typeof(TestAsyncInitializableView));
            MockServiceLocator.Where_Resolve_returns(testAsyncInitializableView, typeof(TestAsyncInitializableView));

            //Act
            await Sut.ShowViewAsync<TestAsyncInitializableViewModel>();

            //Assert
            Assert.That(((TestAsyncInitializableViewModel)testAsyncInitializableView.BindingContext).IsInitialized, Is.True);
        }
        
        [Test]
        public async Task SHOULD_get_page_type_from_lookup_and_resolve_from_service_provider()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowViewAsync<TestViewModel>();

            //Assert
            MockServiceLocator.Mock.Verify(x => x.Resolve(typeof(TestView)));
        }

        [Test]
        public async Task SHOULD_set_currentPage_on_current_navigation_page()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowViewAsync<TestViewModel>();

            //Assert
            ClassicAssert.AreEqual(_testNavigationView.CurrentPage as TestView, _testView);
        }
        
        [Test]
        public void IF_no_navigation_page_has_been_set_SHOULD_throw()
        {
            //Act
            Assert.ThrowsAsync<InvalidOperationException>(async () => await Sut.ShowViewAsync<TestViewModel>());

        }
        
        [Test]
        public void IF_view_lookup_returns_null_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(null);

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowViewAsync<TestViewModel>(), "View not registered for ViewModel");
        }

        [Test]
        public void IF_service_provider_returns_null_SHOULD_throw()
        {
            //Arrange
            MockServiceLocator.Where_Resolve_returns(null, typeof(TestView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowViewAsync<TestViewModel>(), "No View of type TestView has been registered with the Ioc container");

        }

        [Test]
        public void IF_resolved_view_cannot_be_converted_to_Page_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(typeof(FakeView));
            MockServiceLocator.Where_Resolve_returns(new FakeView(), typeof(FakeView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowViewAsync<TestViewModel>(), "View type FakeView is not a Xamarin.Forms Page");

        }
    }
}