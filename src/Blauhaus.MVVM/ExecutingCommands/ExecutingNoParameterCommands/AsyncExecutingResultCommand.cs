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

        public AsyncExecutingResultCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService, Func<Task<Result>> task, Func<Task>? onSuccess = null, Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
            _task = task;
            _onSuccess = onSuccess;
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

        
        public override void Execute()
        {
            Task.Run(async () =>
            {
                
                if (_task == null)
                {
                    throw new InvalidOperationException("the action for this command has not been set");
                }

                if (CanExecute())
                {
                    try
                    {
                        Start();

                        var result = await _task.Invoke().ConfigureAwait(true);
                        if (result.IsFailure)
                        {
                            await ErrorHandler.HandleErrorAsync(result.Error);
                        }

                        if (_onSuccess != null)
                        {
                            await _onSuccess.Invoke();
                        }

                        Finish();
                    }
                    catch (Exception e)
                    {
                        Fail(this, e);
                    }
                }
            });
        } 

    }
}