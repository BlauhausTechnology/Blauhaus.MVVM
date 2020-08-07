using System;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands._Base;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base
{
    public abstract class BaseExecutingValueCommand<TValue> : BaseExecutingCommand
    {

        protected BaseExecutingValueCommand(IErrorHandler errorHandler, Func<bool>? canExecute = null) 
            : base(errorHandler, canExecute)
        {
        }
          

        protected TValue Convert(object parameter)
        {
            if (parameter == null)
            {
                return default;
            }

            if (!(parameter is TValue value))
            {
                throw new Exception($"Expected {typeof(TValue).Name} but got {parameter.GetType().Name}");
            }

            return value;
        }
         
    }
}