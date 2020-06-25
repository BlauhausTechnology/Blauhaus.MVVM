using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands
{
    public class AsyncExecutingCommand : BaseExecutingNoValueCommand
    {
         
        public AsyncExecutingCommand(IErrorHandlingService errorHandlingService, Func<Task> task, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
        {
            Command = new Command(async () => await InvokeAsync(task), CanExecute);
        }

        private async Task InvokeAsync(Func<Task> task)
        {
            try
            {
                if (CanExecute())
                {
                    IsExecuting = true;
                    await task.Invoke().ConfigureAwait(true);
                    IsExecuting = false;
                }
            }
            catch (Exception e)
            {
                IsExecuting = false;
                await ErrorHandlingService.HandleExceptionAsync(this, e);
            }
        }

    }
}