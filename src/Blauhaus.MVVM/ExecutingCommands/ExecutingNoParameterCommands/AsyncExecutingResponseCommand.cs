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
    public class AsyncExecutingResponseCommand : BaseExecutingNoParameterCommand<AsyncExecutingResponseCommand>
    {
        private Func<Task<Response>>? _task;
        private Func<Task>? _successHandler;
        private Dictionary<Error, Func<Error, Task>>? _errorHandlers;
        
        public AsyncExecutingResponseCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(serviceLocator, errorHandler, analyticsService)
        {
        }
         
        
        public AsyncExecutingResponseCommand WithExecute(Func<Task<Response>> task)
        {
            _task = task;
            return this;
        }
        public AsyncExecutingResponseCommand WithExecute(Task<Response> task)
        {
            _task = ()=>task;
            return this;
        }

        public AsyncExecutingResponseCommand OnSuccess(Func<Task> onSuccess)
        {
            _successHandler = onSuccess;
            return this;
        }

        public AsyncExecutingResponseCommand OnFailure(Error errorCondition, Func<Error, Task> errorHandler)
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
                    await _successHandler.Invoke();
                }
            }); 
        } 
    }
}