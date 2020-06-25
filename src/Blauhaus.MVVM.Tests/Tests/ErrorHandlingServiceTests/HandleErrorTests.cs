using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ErrorHandlingServiceTests
{
    public class HandleErrorTests : BaseMvvmTest<ErrorHandlingService>
    {
        protected override ErrorHandlingService ConstructSut()
        {
            return new ErrorHandlingService(
                MockDialogService.Object,
                MockAnalyticsService.Object);
        }


        [Test] public async Task SHOULD_show_alert()
        {
            //Act
            await Sut.HandleErrorAsync("oops");

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", "oops", "OK"));
        }
    }

}