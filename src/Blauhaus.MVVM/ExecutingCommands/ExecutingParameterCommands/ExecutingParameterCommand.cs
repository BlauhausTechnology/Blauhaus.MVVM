﻿using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class ExecutingParameterCommand<TParameter> : BaseExecutingParameterCommand<ExecutingParameterCommand<TParameter>, TParameter>
    {
        private Action<TParameter>? _action;
        public ExecutingParameterCommand<TParameter> WithExecute(Action<TParameter> action)
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
            TryExecute(_action, () =>
            {
                var value = ConvertParameter(parameter);
                _action?.Invoke(value);
            });
        }
    }
}