using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.Base;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands.Base;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class AsyncExecutingResponseParameterCommand<TParameter> : BaseExecutingParameterCommand<AsyncExecutingResponseParameterCommand<TParameter>, TParameter>
    {
        private Func<TParameter, Task<Response>>? _task;
        private Func<Task>? _successHandler;
        private Dictionary<Error, Func<Error, Task>>? _errorHandlers;


        public AsyncExecutingResponseParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public AsyncExecutingResponseParameterCommand<TParameter> WithExecute(Func<TParameter, Task<Response>> task)
        {
            _task = task;
            return this;
        }
        public AsyncExecutingResponseParameterCommand<TParameter> WithExecute(Task<Response> task)
        {
            _task = param => task;
            return this;
        }
        public AsyncExecutingResponseParameterCommand<TParameter> OnSuccess(Func<Task> onSuccess)
        {
            _successHandler = onSuccess;
            return this;
        }
        public AsyncExecutingResponseParameterCommand<TParameter> OnFailure(Error errorCondition, Func<Error, Task> errorHandler)
        {
            _errorHandlers ??= new Dictionary<Error, Func<Error, Task>>();
            _errorHandlers[errorCondition] = errorHandler;
            return this;
        }

        public override async void Execute(object parameter)
        {
            await TryExecuteAsync(_task, async () =>
            {
                var value = ConvertParameter(parameter);
                var result = await _task!.Invoke(value).ConfigureAwait(true);
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