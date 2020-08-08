using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands._Base;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands._Base;
using CSharpFunctionalExtensions;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class AsyncExecutingResultCommand : BaseExecutingNoParameterCommand<AsyncExecutingResultCommand>
    {
        private Func<Task<Result>>? _task;
        private Func<Task>? _successHandler;
        private Dictionary<Error, Func<Error, Task>>? _errorHandlers;
        
        public AsyncExecutingResultCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        
        public AsyncExecutingResultCommand WithTask(Func<Task<Result>> task)
        {
            _task = task;
            return this;
        }

        public AsyncExecutingResultCommand OnSuccess(Func<Task> onSuccess)
        {
            _successHandler = onSuccess;
            return this;
        }

        public AsyncExecutingResultCommand OnFailure(Error errorCondition, Func<Error, Task> errorHandler)
        {
            _errorHandlers ??= new Dictionary<Error, Func<Error, Task>>();
            _errorHandlers[errorCondition] = errorHandler;
            return this;
        }

        
        public override async void Execute(object parameter)
        {
            await TryExecuteAsync(_task, async () =>
            {
                var result = await _task!.Invoke();
                if (result.IsFailure)
                {
                    if(!await _errorHandlers.TryHandle(result.Error))
                    {
                        await ErrorHandler.HandleErrorAsync(result.Error);
                    }
                }

                if (_successHandler != null)
                {
                    await _successHandler.Invoke();
                }
            }); 
        } 
    }
}