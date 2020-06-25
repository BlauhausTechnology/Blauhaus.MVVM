using System;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands
{
    public class ExecutingCommand<TValue> : BaseExecutingValueCommand<TValue>
    {
        public ExecutingCommand(IErrorHandlingService errorHandlingService, Action<TValue> action, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
        {
            Command = new Command((value) => Invoke(action, value), x => CanExecute());
        }
         
        private void Invoke(Action<TValue> action, object parameter)
        { 
            try
            {
                var value = Convert(parameter);

                if (CanExecute())
                {
                    IsExecuting = true;
                    action.Invoke(value);
                    IsExecuting = false;
                }
            }
            catch (Exception e)
            {
                ErrorHandlingService.HandleExceptionAsync(this, e);
                IsExecuting = false;
            }
        } 
    }
}