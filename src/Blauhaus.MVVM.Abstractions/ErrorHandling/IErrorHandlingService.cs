using System;
using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.ErrorHandling
{
    public interface IErrorHandlingService
    {
        Task HandleExceptionAsync(object sender, Exception exception);
        Task HandleErrorAsync(string errorMessage);
    }
}