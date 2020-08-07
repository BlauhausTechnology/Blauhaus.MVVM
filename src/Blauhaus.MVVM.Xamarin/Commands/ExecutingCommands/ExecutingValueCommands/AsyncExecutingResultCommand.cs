using System;
using System.Threading.Tasks;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base;
using CSharpFunctionalExtensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands
{
    public class AsyncExecutingResultCommand<TValue> : BaseExecutingValueCommand<TValue>
    {
        private readonly Func<Task>? _onSuccess;

        public AsyncExecutingResultCommand(IErrorHandler errorHandler, Func<TValue, Task<Result>> task, Func<Task>? onSuccess = null, Func<bool>? canExecute = null) 
            : base(errorHandler, canExecute)
        {
            _onSuccess = onSuccess;
            Command = new Command((value) => InvokeAsync(task, value), x => CanExecute());
        }

        private async void InvokeAsync(Func<TValue, Task<Result>> task, object parameter)
        {
            var value = Convert(parameter);
            try
            {
                if (CanExecute())
                {
                    IsExecuting = true;
                    var result = await task.Invoke(value).ConfigureAwait(true);
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
                await ErrorHandler.HandleExceptionAsync(this, e);
                IsExecuting = false;
            }
        }

    }
}