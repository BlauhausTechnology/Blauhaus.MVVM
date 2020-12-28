using System;
using System.Threading.Tasks;
using Blauhaus.Errors;
using Blauhaus.Errors.Handler;

namespace Blauhaus.MVVM.MonoGame.Services
{
    public class MonoGameErrorHandler : IErrorHandler
    {
        public Task HandleExceptionAsync(object sender, Exception exception)
        {
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(string errorMessage)
        {
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Error errorMessage)
        {
            return Task.CompletedTask;
        }
    }
}