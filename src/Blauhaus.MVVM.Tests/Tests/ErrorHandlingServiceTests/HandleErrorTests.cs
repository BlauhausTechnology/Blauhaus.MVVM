using System.Threading.Tasks;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ErrorHandlingServiceTests
{
    public class HandleErrorTests : BaseMvvmTest<ErrorHandler>
    { 
        [Test] public async Task SHOULD_show_alert()
        {
            //Act
            await Sut.HandleErrorAsync("oops");

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", "oops", "OK"));
        }

        [Test] public async Task IF_error_message_is_Error_SHOULD_show_Description_in_alert()
        {
            //Act
            await Sut.HandleErrorAsync(AuthErrors.NotAuthenticated.ToString());

            //Assert
            MockDialogService.Mock.Verify(x => x.DisplayAlertAsync("Error", AuthErrors.NotAuthenticated.Description, "OK"));
        }
    }

}