﻿using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class AsyncExecutingCommand : BaseExecutingNoParameterCommand<AsyncExecutingCommand>
    {
        private Func<Task>? _task;

        public AsyncExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }

        public AsyncExecutingCommand WithExecute(Func<Task> task)
        {
            _task = task;
            return this;
        }
         
        public override async void Execute(object parameter)
        {
            await TryExecuteAsync(_task, async () =>
            {
                await _task!.Invoke();
            });
        }
    }
}