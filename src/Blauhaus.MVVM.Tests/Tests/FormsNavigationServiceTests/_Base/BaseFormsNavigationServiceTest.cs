using Blauhaus.DeviceServices.TestHelpers.MockBuilders;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.MVVM.Xamarin.Navigation;
using Xamarin.Forms.Mocks;

namespace Blauhaus.MVVM.Tests.Tests.FormsNavigationServiceTests._Base
{
    public class BaseFormsNavigationServiceTest: BaseMvvmTest<FormsNavigationService>
    {

        protected override FormsNavigationService ConstructSut()
        {

            var thread = new ThreadServiceMockBuilder();

            return new FormsNavigationService(
                MockServiceProvider.Object,
                MockNavigationLookup.Object,
                MockFormsApplicationProxy.Object,
                thread.Object);

        }
        
    }
}