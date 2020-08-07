using System;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands
{
    public class ExecutingCommand : BaseExecutingNoValueCommand
    {
        public ExecutingCommand(IErrorHandler errorHandler, Action action, Func<bool>? canExecute = null) 
            : base(errorHandler, canExecute)
        {
            Command = new Command(() => Invoke(action), CanExecute);
        }

        private void Invoke(Action action)
        {
            try
            {
                if (CanExecute())
                {
                    IsExecuting = true;
                    action.Invoke();
                    IsExecuting = false;
                }
            }
            catch (Exception e)
            {
                IsExecuting = false;
                ErrorHandler.HandleExceptionAsync(this, e);
            }
        } 
    }
}