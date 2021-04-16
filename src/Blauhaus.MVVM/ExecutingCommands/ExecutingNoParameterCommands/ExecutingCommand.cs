using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class ExecutingCommand : BaseExecutingNoParameterCommand<ExecutingCommand>
    {
        private Action? _action;

        
        public ExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public ExecutingCommand WithExecute(Action action)
        {
            _action = action;
            return this;
        }
         
        public override void Execute(object parameter)
        {
            TryExecute(_action, () =>
            {
                _action?.Invoke();
            });
        }
 

    }
}