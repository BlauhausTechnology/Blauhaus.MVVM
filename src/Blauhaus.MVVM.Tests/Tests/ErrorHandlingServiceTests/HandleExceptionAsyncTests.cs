using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ErrorHandlingServiceTests
{
    public class HandleExceptionAsyncTests : BaseMvvmTest<ErrorHandlingService>
    {
        protected override ErrorHandlingService ConstructSut()
        {
            return new ErrorHandlingService(
                MockDialogService.Object,
                MockAnalyticsService.Object);
        }

        [Test]
        public async Task SHOULD_log_exception()
        {
            //Arrange
            var exception = new Exception("failio");

            //Act
            await Sut.HandleExceptionAsync(this, exception);

            //Assert
            MockAnalyticsService.VerifyLogException(exception);
        }

        [Test] public async Task SHOULD_show_alert()
        {
            //Arrange
            var exception = new Exception("failio");

            //Act
            await Sut.HandleExceptionAsync(this, exception);

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", "An unexpected error has occured", "OK"));
        }
    }

}