using System;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands._Base;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base
{
    public abstract class BaseExecutingNoValueCommand : BaseExecutingCommand
    {

        protected BaseExecutingNoValueCommand(IErrorHandler errorHandler, Func<bool>? canExecute = null) 
            : base(errorHandler, canExecute)
        {
        }


    }
}