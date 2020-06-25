using System;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands._Base;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands._Base
{
    public abstract class BaseExecutingValueCommand<TValue> : BaseExecutingCommand, IExecutingCommand
    {

        protected BaseExecutingValueCommand(IErrorHandlingService errorHandlingService, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
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