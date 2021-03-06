﻿using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class AsyncExecutingParameterCommand<TParameter> : BaseExecutingParameterCommand<AsyncExecutingParameterCommand<TParameter>, TParameter>
    {
        private Func<TParameter, Task>? _task;

        public AsyncExecutingParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public AsyncExecutingParameterCommand<TParameter> WithExecute(Func<TParameter, Task> task)
        {
            _task = task;
            return this;
        }

        public override async void Execute(object parameter)
        {
            await TryExecuteAsync(_task, async () =>
            {
                var value = ConvertParameter(parameter);
                await _task!.Invoke(value).ConfigureAwait(true);
            });
        }
         

    }
}