using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class ExecutingParameterCommand<TParameter> : BaseExecutingParameterCommand<ExecutingParameterCommand<TParameter>, TParameter>
    {
        private Action<TParameter>? _action;
        public ExecutingParameterCommand<TParameter> WithAction(Action<TParameter> action)
        {
            _action = action;
            return this;
        }

        public ExecutingParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }

        public ExecutingParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            Action<TParameter> action, 
            Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
            _action = action;
        }
          

        public override void Execute(object parameter)
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

                    var value = ConvertParameter(parameter);
                    _action?.Invoke(value);

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