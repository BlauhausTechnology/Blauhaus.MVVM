using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests
{
    public class ShowMainViewTests : BaseFormsNavigationServiceTest
    {

        private TestView _testView;

        public override  void Setup()
        {
            base.Setup();

            _testView = new TestView(new TestViewModel());
            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(typeof(TestView));
            MockServiceLocator.Where_Resolve_returns(_testView, typeof(TestView));
        }
        
        [Test]
        public async Task IF_ViewModel_is_IAsyncInitializable_SHOULD_initialize()
        {
            //Arrange 
            var testAsyncInitializableView = new TestAsyncInitializableView(new TestAsyncInitializableViewModel());
            MockNavigationLookup.Where_GetViewType_returns<TestAsyncInitializableViewModel>(typeof(TestAsyncInitializableView));
            MockServiceLocator.Where_Resolve_returns(testAsyncInitializableView, typeof(TestAsyncInitializableView));

            //Act
            await Sut.ShowMainViewAsync<TestAsyncInitializableViewModel>();

            //Assert
            Assert.That(((TestAsyncInitializableViewModel)testAsyncInitializableView.BindingContext).IsInitialized, Is.True);
        }
        
        [Test]
        public async Task SHOULD_get_page_type_from_lookup_and_resolve_from_service_provider()
        {
            //Act
            await Sut.ShowMainViewAsync<TestViewModel>();

            //Assert
            MockServiceLocator.Mock.Verify(x => x.Resolve(typeof(TestView)));
        }

        [Test]
        public async Task SHOULD_set_main_page_with_resolved_View()
        {
            //Act
            await Sut.ShowMainViewAsync<TestViewModel>();

            //Assert
            MockFormsApplicationProxy.Mock.Verify(x => x.SetMainPage(It.Is<Page>(y=> 
                ((TestView) y).ViewId == _testView.ViewId)));
        }
        
        [Test]
        public async Task IF_view_lookup_returns_null_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(null);

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowMainViewAsync<TestViewModel>(), "View not registered for ViewModel");
        }

        [Test]
        public async Task IF_service_provider_returns_null_SHOULD_throw()
        {
            //Arrange
            MockServiceLocator.Where_Resolve_returns(null, typeof(TestView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowMainViewAsync<TestViewModel>(), "No View of type TestView has been registered with the Ioc container");

        }

        [Test]
        public async Task IF_resolved_view_cannot_be_converted_to_Page_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(typeof(FakeView));
            MockServiceLocator.Where_Resolve_returns(new FakeView(), typeof(FakeView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowMainViewAsync<TestViewModel>(), "View type FakeView is not a Xamarin.Forms Page");

        }
    }
}