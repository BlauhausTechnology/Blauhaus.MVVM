using System;
using Blauhaus.Errors;
using Blauhaus.Errors.Handler;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class ErrorHandlerMockBuilder : BaseMockBuilder<ErrorHandlerMockBuilder, IErrorHandler>
    {
        public void Verify_HandleException(Exception exception)
        {
            Mock.Verify(x => x.HandleExceptionAsync(It.IsAny<object>(), exception));
        }
        
        public void Verify_HandleExceptionMessage(string exceptionMessage)
        {
            Mock.Verify(x => x.HandleExceptionAsync(It.IsAny<object>(), It.Is<Exception>(y => y.Message == exceptionMessage)));
        }
        
        public void Verify_HandleErrorMessage(string errorMessage)
        {
            Mock.Verify(x => x.HandleErrorAsync(errorMessage));
        }

        public void Verify_HandleError(Error error)
        {
            Mock.Verify(x => x.HandleErrorAsync(error));
        }
        public void Verify_HandleError(string errorDescription)
        {
            Mock.Verify(x => x.HandleErrorAsync(It.Is<Error>(y => y.Description == errorDescription)));
        }

        public void Verify_HandleErrorMessage_not_called()
        {
            Mock.Verify(x => x.HandleErrorAsync(It.IsAny<string>()), Times.Never);
        }
        public void Verify_HandleError_not_called()
        {
            Mock.Verify(x => x.HandleErrorAsync(It.IsAny<Error>()), Times.Never);
        }
    }
}