using System;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class ErrorHandlingServiceMockBuilder : BaseMockBuilder<ErrorHandlingServiceMockBuilder, IErrorHandlingService>
    {
        public void Verify_HandleException(Exception exception)
        {
            Mock.Verify(x => x.HandleExceptionAsync(It.IsAny<object>(), exception));
        }
        
        public void Verify_HandleExceptionMessage(string exceptionMessage)
        {
            Mock.Verify(x => x.HandleExceptionAsync(It.IsAny<object>(), It.Is<Exception>(y => y.Message == exceptionMessage)));
        }
        
        public void Verify_HandleError(string errorMessage)
        {
            Mock.Verify(x => x.HandleErrorAsync(errorMessage));
        }

        public void Verify_HandleError_not_called()
        {
            Mock.Verify(x => x.HandleErrorAsync(It.IsAny<string>()), Times.Never);
        }
    }
}