using System;
using System.Threading.Tasks;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Errors;
using Blauhaus.MVVM.Tests.Tests.Base;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ErrorHandlingServiceTests
{
    public class HandleExceptionAsyncTests : BaseMvvmTest<ErrorHandler>
    { 
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

        [Test] 
        public async Task IF_Release_SHOULD_Show_generic_message()
        {
            //Arrange
            Config = BuildConfig.Release;
            var exception = new Exception("failio");

            //Act
            await Sut.HandleExceptionAsync(this, exception);

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", "An unexpected error has occured", "OK"));
        }

        [Test] 
        public async Task IF_Debug_SHOULD_Show_exception_message()
        {
            //Arrange
            Config = BuildConfig.Debug;
            var exception = new Exception("failio");

            //Act
            await Sut.HandleExceptionAsync(this, exception);

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", "failio", "OK"));
        }

        [Test] public async Task IF_Exception_is_ErrorException_SHOULD_show_Description()
        {
            //Arrange
            var exception = new ErrorException(AuthErrors.NotAuthorized);

            //Act
            await Sut.HandleExceptionAsync(this, exception);

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", AuthErrors.NotAuthorized.Description, "OK"));
        }
    }

}