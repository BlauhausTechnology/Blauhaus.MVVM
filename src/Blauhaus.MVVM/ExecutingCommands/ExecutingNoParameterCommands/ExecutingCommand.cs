using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class ExecutingCommand : BaseExecutingNoParameterCommand<ExecutingCommand>
    {
        private Action? _action;

        
        public ExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public ExecutingCommand WithAction(Action action)
        {
            _action = action;
            return this;
        }
         
        public override void Execute()
        {
            TryExecute(_action, () =>
            {
                _action?.Invoke();
            });
        }
 

    }
}