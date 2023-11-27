using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests
{
    public class ShowAndInitializeMainViewAsyncTests : BaseFormsNavigationServiceTest
    {
        
        private TestInitializingView _testView;
        private readonly Guid _parameter = Guid.NewGuid();

        public override  void Setup()
        {
            base.Setup();
             
            _testView = new TestInitializingView(new TestInitializingViewModel());
            MockNavigationLookup.Where_GetViewType_returns<TestInitializingViewModel>(typeof(TestInitializingView));
            MockServiceLocator.Where_Resolve_returns(_testView, typeof(TestInitializingView));
        }
         
        [Test]
        public async Task SHOULD_set_parameter()
        { 

            //Act
            await Sut.ShowAndInitializeMainViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            ClassicAssert.AreEqual(_parameter, ((TestInitializingViewModel)_testView.BindingContext).InitializedValue);
        }
        
        [Test]
        public async Task SHOULD_get_page_type_from_lookup_and_resolve_from_service_provider()
        {
            //Act
            await Sut.ShowAndInitializeMainViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            MockServiceLocator.Mock.Verify(x => x.Resolve(typeof(TestInitializingView)));
        }

        [Test]
        public async Task SHOULD_set_main_page_with_resolved_View()
        {
            //Act
            await Sut.ShowAndInitializeMainViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Assert
            MockFormsApplicationProxy.Mock.Verify(x => x.SetMainPage(It.Is<Page>(y=> 
                ((TestInitializingView) y).ViewId == _testView.ViewId)));
        }
        
        [Test]
        public async Task IF_view_lookup_returns_null_SHOULD_throw()
        {
            //Arrange
            await Sut.ShowAndInitializeMainViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowMainViewAsync<TestViewModel>(), "View not registered for ViewModel");
        }

        [Test]
        public async Task IF_service_provider_returns_null_SHOULD_throw()
        {
            //Arrange
            await Sut.ShowAndInitializeMainViewAsync<TestInitializingViewModel, Guid>(_parameter);

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowMainViewAsync<TestViewModel>(), "No View of type TestView has been registered with the Ioc container");

        }

        [Test]
        public async Task IF_resolved_view_cannot_be_converted_to_Page_SHOULD_throw()
        {
            //Arrange
            MockNavigationLookup.Where_GetViewType_returns<TestInitializingViewModel>(typeof(FakeView));
            MockServiceLocator.Where_Resolve_returns(new FakeView(), typeof(FakeView));

            //Act
            Assert.ThrowsAsync<NavigationException>(async () => await Sut.ShowAndInitializeMainViewAsync<TestInitializingViewModel, Guid>(_parameter), "View type FakeView is not a Xamarin.Forms Page");

        }
    }
}