using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands._Base;
using CSharpFunctionalExtensions;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class AsyncExecutingResultCommand : BaseExecutingNoParameterCommand<AsyncExecutingResultCommand>
    {
        private Func<Task<Result>>? _task;
        private Func<Task>? _onSuccess;
        
        public AsyncExecutingResultCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        
        public AsyncExecutingResultCommand WithOnSuccess(Func<Task> onSuccess)
        {
            _onSuccess = onSuccess;
            return this;
        }

        public AsyncExecutingResultCommand WithTask(Func<Task<Result>> task)
        {
            _task = task;
            return this;
        }

        
        public override async void Execute()
        {
            await TryExecuteAsync(_task, async () =>
            {
                var result = await _task!.Invoke();
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