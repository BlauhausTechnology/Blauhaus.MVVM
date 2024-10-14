using System;
using System.Threading.Tasks;
using Blauhaus.Errors;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Tests.Tests.AppLifecycleServiceTests.Base;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Tests.Tests.AppLifecycleServiceTests
{
    public class NotifyAppGoingToSleepTests : BaseAppLifecycleServiceTest
    {
        [Test]
        public async Task SHOULD_notify_allhandlers()
        {
            //Act
            Sut.NotifyAppGoingToSleep();
            await Task.Delay(10);
            
            //Assert
            MockHandlerOne.Mock.Verify(x => x.HandleAppStateChangeAsync(AppLifecycleState.GoingToSleep));
            MockHandlerTwo.Mock.Verify(x => x.HandleAppStateChangeAsync(AppLifecycleState.GoingToSleep));
        }
         
        [Test]
        public async Task IF_handler_returns_error_SHOULD_handle()
        {
            //Arrange
            MockHandlerOne.Mock.Setup(x => x.HandleAppStateChangeAsync(It.IsAny<AppLifecycleState>()))
                .ReturnsAsync(Response.Failure(Error.RequiredValue()));
            
            //Act
            Sut.NotifyAppGoingToSleep();
            await Task.Delay(10);
            
            //Assert
            MockErrorHandler.Mock.Verify(x => x.HandleErrorAsync(Error.RequiredValue()));
        }
        
        [Test]
        public async Task IF_handler_throws_SHOULD_handle_exception()
        {
            //Arrange
            var ex = new Exception("oopsydaisy");
            MockHandlerOne.Mock.Setup(x => x.HandleAppStateChangeAsync(It.IsAny<AppLifecycleState>())).ThrowsAsync(ex);
            
            //Act
            Sut.NotifyAppGoingToSleep();
            await Task.Delay(10);
            
            //Assert
            MockErrorHandler.Mock.Verify(x => x.HandleExceptionAsync(Sut, ex));
        }
    }
}