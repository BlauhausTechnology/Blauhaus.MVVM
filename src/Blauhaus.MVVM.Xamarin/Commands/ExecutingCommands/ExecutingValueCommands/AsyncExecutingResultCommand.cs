using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base;
using CSharpFunctionalExtensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands
{
    public class AsyncExecutingResultCommand<TValue> : BaseExecutingValueCommand<TValue>
    {
         
        public AsyncExecutingResultCommand(IErrorHandlingService errorHandlingService, Func<TValue, Task<Result>> task, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
        {
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
                        await ErrorHandlingService.HandleErrorAsync(result.Error);
                    }
                    IsExecuting = false;
                }
            }
            catch (Exception e)
            {
                await ErrorHandlingService.HandleExceptionAsync(this, e);
                IsExecuting = false;
            }
        }

    }
}