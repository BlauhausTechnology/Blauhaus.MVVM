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

        public override void Execute(object parameter)
        {
            TryExecute(this, _action, () =>
            {
                var value = ConvertParameter(parameter);
                _action?.Invoke(value);
            });
        }
    }
}