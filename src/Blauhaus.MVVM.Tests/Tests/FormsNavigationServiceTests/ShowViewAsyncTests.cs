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
            MockServiceProvider.Where_GetService_returns(_testView, typeof(TestView));
        }

        [Test]
        public async Task SHOULD_get_page_type_from_lookup_and_resolve_from_service_provider()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowViewAsync<TestViewModel>();

            //Assert
            MockServiceProvider.Verify_GetService_was_called_with_Type(typeof(TestView));
        }

        [Test]
        public async Task SHOULD_current_navigation_page()
        {
            //Arrange
            Sut.SetCurrentNavigationView(_testNavigationView);

            //Act
            await Sut.ShowViewAsync<TestViewModel>();

            //Assert
            Assert.AreEqual(_testNavigationView.CurrentPage as TestView, _testView);
        }
        
        [Test]
        public async Task IF_no_navigation_page_has_been_set_SHOULD_set_one()
        {
            //Act
            await Sut.ShowViewAsync<TestViewModel>();

            //Assert
            Assert.That(_testView.Navigation, Is.Not.Null);
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
            MockServiceProvider.Where_GetService_returns(null, typeof(TestView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowViewAsync<TestViewModel>(), "No View of type TestView has been registered with the Ioc container");

        }

        [Test]
        public void IF_resolved_view_cannot_be_converted_to_Page_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(typeof(FakeView));
            MockServiceProvider.Where_GetService_returns(new FakeView(), typeof(FakeView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowViewAsync<TestViewModel>(), "View type FakeView is not a Xamarin.Forms Page");

        }
    }
}