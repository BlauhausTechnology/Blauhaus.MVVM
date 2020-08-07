using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands
{
    public class AsyncExecutingParameterCommand<TParameter> : BaseExecutingParameterCommand<AsyncExecutingParameterCommand<TParameter>, TParameter>
    {
        private Func<TParameter, Task>? _task;

        public AsyncExecutingParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }

        public AsyncExecutingParameterCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            Func<TParameter, Task> task, 
            Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
            _task = task;
        }
        
        public AsyncExecutingParameterCommand<TParameter> WithTask(Func<TParameter, Task> task)
        {
            _task = task;
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
                        await _task.Invoke(value).ConfigureAwait(true);
                        
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