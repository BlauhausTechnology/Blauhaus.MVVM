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

        public ExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService, Action action, Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
            _action = action;
        }

        public ExecutingCommand WithAction(Action action)
        {
            _action = action;
            return this;
        }
         
        public override void Execute()
        {
            if (CanExecute())
            {
                if (_action == null)
                {
                    throw new InvalidOperationException("the action for this command has not been set");
                }

                try
                {
                    Start();
                    
                    _action.Invoke();
                    
                    Finish();
                }
                catch (Exception e)
                {
                    Fail(this, e);
                }
            }
        }
 

    }
}