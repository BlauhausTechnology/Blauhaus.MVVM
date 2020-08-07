using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands._Base;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoParameterCommands._Base
{
    public abstract class BaseExecutingNoParameterCommand<TExecutingCommand> : BaseExecutingCommand<TExecutingCommand> 
        where TExecutingCommand : BaseExecutingNoParameterCommand<TExecutingCommand>
    {

        protected BaseExecutingNoParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }

        protected BaseExecutingNoParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
        }

        public override void Execute(object parameter) => Execute();
        public abstract void Execute();

    }
}