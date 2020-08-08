using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands._Base;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;
using CSharpFunctionalExtensions;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class AsyncExecutingResultParameterCommand<TParameter> : BaseExecutingParameterCommand<AsyncExecutingResultParameterCommand<TParameter>, TParameter>
    {
        private Func<TParameter, Task<Result>>? _task;
        private Func<Task>? _successHandler;
        private Dictionary<Error, Func<Error, Task>>? _errorHandlers;


        public AsyncExecutingResultParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public AsyncExecutingResultParameterCommand<TParameter> WithTask(Func<TParameter, Task<Result>> task)
        {
            _task = task;
            return this;
        }
        public AsyncExecutingResultParameterCommand<TParameter> OnSuccess(Func<Task> onSuccess)
        {
            _successHandler = onSuccess;
            return this;
        }
        public AsyncExecutingResultParameterCommand<TParameter> OnFailure(Error errorCondition, Func<Error, Task> errorHandler)
        {
            _errorHandlers ??= new Dictionary<Error, Func<Error, Task>>();
            _errorHandlers[errorCondition] = errorHandler;
            return this;
        }

        public override async void Execute(object parameter)
        {
            await TryExecuteAsync(this, _task, async () =>
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

                if (_successHandler != null)
                {
                    await _successHandler.Invoke();
                }
            });
        }
    }
}