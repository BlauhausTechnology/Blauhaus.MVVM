using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.Base
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
         

        public override void Execute(object parameter) => Execute();

    }
}