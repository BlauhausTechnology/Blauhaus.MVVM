using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using CSharpFunctionalExtensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands
{
    public class AsyncExecutingResultCommand : BaseExecutingNoValueCommand
    {
         
        public AsyncExecutingResultCommand(IErrorHandlingService errorHandlingService, Func<Task<Result>> task, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
        {
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