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

        public AsyncExecutingResultParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService, 
            Func<TParameter, Task<Result>> task, 
            Func<Task>? onSuccess = null, 
            Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
            _task = task;
            _onSuccess = onSuccess;
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

        public override void Execute(object parameter)
        {
            


            if (CanExecute())
            {
                if (_task == null)
                {
                    throw new InvalidOperationException("the action for this command has not been set");
                }

                Task.Run(async () =>
                {
                    try
                    {
                        Start();

                        var value = ConvertParameter(parameter);
                        var result = await _task.Invoke(value).ConfigureAwait(true);
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
                });
            }

           
        }
    }
}