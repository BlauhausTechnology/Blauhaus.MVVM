using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.ExecutingCommands.Base;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.Base;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class AsyncExecutingValueResponseCommand<TValue> : BaseExecutingNoParameterCommand<AsyncExecutingValueResponseCommand<TValue>>
    {
        private Func<Task<Response<TValue>>>? _task;
        private Func<TValue, Task>? _successHandler;
        private Dictionary<Error, Func<Error, Task>>? _errorHandlers;
        
        public AsyncExecutingValueResponseCommand(
            IServiceLocator serviceLocator, 
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
                : base(serviceLocator, errorHandler, analyticsService)
        {
        }
         
        
        public AsyncExecutingValueResponseCommand<TValue> WithExecute(Func<Task<Response<TValue>>> task)
        {
            _task = task;
            return this;
        }
        public AsyncExecutingValueResponseCommand<TValue> WithExecute(Task<Response<TValue>> task)
        {
            _task = ()=> task;
            return this;
        }

        public AsyncExecutingValueResponseCommand<TValue> OnSuccess(Func<TValue, Task> onSuccess)
        {
            _successHandler = onSuccess;
            return this;
        }

        public AsyncExecutingValueResponseCommand<TValue> OnFailure(Error errorCondition, Func<Error, Task> errorHandler)
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

                else if (_successHandler != null)
                {
                    await _successHandler.Invoke(result.Value);
                }
            }); 
        } 
    }
}