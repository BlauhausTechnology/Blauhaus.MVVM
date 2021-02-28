using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Tests.Tests.AppLifecycleServiceTests.Base;
using Blauhaus.Responses;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.AppLifecycleServiceTests
{
    public class NotifyAppWakingUpTests : BaseAppLifecycleServiceTest
    {
        [Test]
        public async Task SHOULD_notify_allhandlers()
        {
            //Act
            Sut.NotifyAppWakingUp();
            await Task.Delay(10);
            
            //Assert
            MockHandlerOne.Mock.Verify(x => x.HandleAppStateChangeAsync(AppLifecycleState.WakingUp));
            MockHandlerTwo.Mock.Verify(x => x.HandleAppStateChangeAsync(AppLifecycleState.WakingUp));
        }
        
        
        [Test]
        public async Task SHOULD_log()
        {
            //Act
            Sut.NotifyAppWakingUp();
            await Task.Delay(10);
            
            //Assert
            MockAnalyticsService.VerifyTrace("App WakingUp. Notifying handlers: 2");
        }
        
        
        [Test]
        public async Task IF_handler_returns_error_SHOULD_handle()
        {
            //Arrange
            MockHandlerTwo.Mock.Setup(x => x.HandleAppStateChangeAsync(It.IsAny<AppLifecycleState>()))
                .ReturnsAsync(Response.Failure(Errors.Errors.RequiredValue()));
            
            //Act
            Sut.NotifyAppWakingUp();
            await Task.Delay(10);
            
            //Assert
            MockErrorHandler.Mock.Verify(x => x.HandleErrorAsync(Errors.Errors.RequiredValue()));
        }
        
        [Test]
        public async Task IF_handler_throws_SHOULD_handle_exception()
        {
            //Arrange
            var ex = new Exception("oopsydaisy");
            MockHandlerTwo.Mock.Setup(x => x.HandleAppStateChangeAsync(It.IsAny<AppLifecycleState>())).ThrowsAsync(ex);
            
            //Act
            Sut.NotifyAppWakingUp();
            await Task.Delay(10);
            
            //Assert
            MockErrorHandler.Mock.Verify(x => x.HandleExceptionAsync(Sut, ex));
        }
        
    }
}