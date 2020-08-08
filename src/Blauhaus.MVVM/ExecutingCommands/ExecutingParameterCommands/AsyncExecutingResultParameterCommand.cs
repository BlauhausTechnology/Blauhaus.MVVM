using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;
using CSharpFunctionalExtensions;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class AsyncExecutingResultParameterCommand<TParameter> : BaseExecutingParameterCommand<AsyncExecutingResultParameterCommand<TParameter>, TParameter>
    {
        private Func<TParameter, Task<Result>>? _task;
        private Func<Task>? _onSuccess;


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
        
        public AsyncExecutingResultParameterCommand<TParameter> WithOnSuccess(Func<Task> onSuccess)
        {
            _onSuccess = onSuccess;
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
                    await ErrorHandler.HandleErrorAsync(result.Error);
                }

                if (_onSuccess != null)
                {
                    await _onSuccess.Invoke();
                }
            });
        }
    }
}