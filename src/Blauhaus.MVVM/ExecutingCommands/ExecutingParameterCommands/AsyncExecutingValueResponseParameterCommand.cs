using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands._Base;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;
using Blauhaus.Responses;
using CSharpFunctionalExtensions;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class AsyncExecutingValueResponseParameterCommand<TParameter, TValue> : BaseExecutingParameterCommand<AsyncExecutingValueResponseParameterCommand<TParameter, TValue>, TParameter>
    {
        private Func<TParameter, Task<Response<TValue>>>? _task;
        private Func<TValue, Task>? _successHandler;
        private Dictionary<Error, Func<Error, Task>>? _errorHandlers;


        public AsyncExecutingValueResponseParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public AsyncExecutingValueResponseParameterCommand<TParameter, TValue> WithExecute(Func<TParameter, Task<Response<TValue>>>? task)
        {
            _task = task;
            return this;
        }
        public AsyncExecutingValueResponseParameterCommand<TParameter, TValue> OnSuccess(Func<TValue, Task> onSuccess)
        {
            _successHandler = onSuccess;
            return this;
        }
        public AsyncExecutingValueResponseParameterCommand<TParameter, TValue> OnFailure(Error errorCondition, Func<Error, Task> errorHandler)
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
                    await _successHandler.Invoke(result.Value);
                }
            });
        }
    }
}