using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base;
using Blauhaus.MVVM.Xamarin.Views.MasterDetail;
using Blauhaus.MVVM.Xamarin.Views.Navigation;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests
{
    public class ShowDetailViewAsyncTests : BaseFormsNavigationServiceTest
    {
        private TestFlyoutViewModel _testFlyoutViewModel;
        private TestViewModel _testViewModel;
        private TestView _testView;
        private FlyoutPage<TestFlyoutViewModel, TestMenuView, TestView> _testFlyoutPage;
        private TestMenuView _testMenuView;

        public override  void Setup()
        {
            base.Setup();

            MockForms.Init();

            _testViewModel = new TestViewModel();
            _testView = new TestView(new TestViewModel());
            
            _testMenuView = new TestMenuView(_testViewModel);
            _testFlyoutViewModel = new TestFlyoutViewModel();
            _testFlyoutPage = new FlyoutPage<TestFlyoutViewModel, TestMenuView, TestView>(_testFlyoutViewModel, MockNavigationService.Object, _testMenuView, _testView);

            MockNavigationLookup.Where_GetViewType_returns<TestViewModel>(typeof(TestView));
            MockServiceLocator.Where_Resolve_returns(_testView, typeof(TestView));
        }

        [Test]
        public async Task SHOULD_get_page_type_from_lookup_and_resolve_from_service_provider()
        {
            //Arrange
            Sut.SetCurrentFlyoutView(_testFlyoutPage);

            //Act
            await Sut.ShowDetailViewAsync<TestViewModel>();

            //Assert
            MockServiceLocator.Mock.Verify(x => x.Resolve(typeof(TestView)));
        }

        [Test]
        public async Task SHOULD_set_currentPage_on_current_navigation_page()
        {
            //Arrange
            Sut.SetCurrentFlyoutView(_testFlyoutPage);
            _testFlyoutPage.Detail = new ContentPage();

            //Act
            await Sut.ShowDetailViewAsync<TestViewModel>();


            //Assert
            Assert.AreEqual(_testFlyoutPage.Detail as TestView, _testView);
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