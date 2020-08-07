using System;
using System.Threading.Tasks;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using CSharpFunctionalExtensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands
{
    public class AsyncExecutingResultCommand : BaseExecutingNoValueCommand
    {
        private readonly Func<Task>? _onSuccess;

        public AsyncExecutingResultCommand(IErrorHandler errorHandler, Func<Task<Result>> task, Func<Task>? onSuccess = null, Func<bool>? canExecute = null) 
            : base(errorHandler, canExecute)
        {
            _onSuccess = onSuccess;
            Command = new Command(() => InvokeAsync(task), CanExecute);
        }

        private async void InvokeAsync(Func<Task<Result>> task)
        {
            
            try
            {
                if (CanExecute())
                {
                    IsExecuting = true;
                    var result = await task.Invoke().ConfigureAwait(true);
                    if (result.IsFailure)
                    {
                        await ErrorHandler.HandleErrorAsync(result.Error);
                    }

                    if (_onSuccess != null)
                    {
                        await _onSuccess.Invoke();
                    }

                    IsExecuting = false;
                }
            }
            catch (Exception e)
            {
                IsExecuting = false;
                await ErrorHandler.HandleExceptionAsync(this, e);
            }
        }

    }
}