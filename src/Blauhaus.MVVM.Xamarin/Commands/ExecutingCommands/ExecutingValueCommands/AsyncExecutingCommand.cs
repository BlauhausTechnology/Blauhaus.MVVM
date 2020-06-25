using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands
{
    public class AsyncExecutingCommand<TValue> : BaseExecutingValueCommand<TValue>
    {
         
        public AsyncExecutingCommand(IErrorHandlingService errorHandlingService, Func<TValue, Task> task, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
        {
            Command = new Command((parameter) => InvokeAsync(task, parameter), x => CanExecute());
        }

        private async void InvokeAsync(Func<TValue, Task> task, object parameter)
        {

            try
            {
                var value = Convert(parameter);

                if (CanExecute())
                {
                    IsExecuting = true;
                    await task.Invoke(value).ConfigureAwait(true);
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