using System;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands._Base;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base
{
    public abstract class BaseExecutingNoValueCommand : BaseExecutingCommand
    {

        protected BaseExecutingNoValueCommand(IErrorHandlingService errorHandlingService, Func<bool>? canExecute = null) 
            : base(errorHandlingService, canExecute)
        {
        }


    }
}