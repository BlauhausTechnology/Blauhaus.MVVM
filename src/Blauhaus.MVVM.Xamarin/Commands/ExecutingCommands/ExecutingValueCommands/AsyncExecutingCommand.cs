using System;
using System.Threading.Tasks;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands
{
    public class AsyncExecutingCommand<TValue> : BaseExecutingValueCommand<TValue>
    {
         
        public AsyncExecutingCommand(IErrorHandler errorHandler, Func<TValue, Task> task, Func<bool>? canExecute = null) 
            : base(errorHandler, canExecute)
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
                await ErrorHandler.HandleExceptionAsync(this, e);
                IsExecuting = false;
            }
        }

    }
}