using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoParameterCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoParameterCommands
{
    public class AsyncExecutingCommand : BaseExecutingNoParameterCommand<AsyncExecutingCommand>
    {
        private Func<Task>? _task;

        public AsyncExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }

        public AsyncExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService, Func<Task> task, Func<bool>? canExecute = null) 
            : base(errorHandler, analyticsService, canExecute)
        {
            _task = task;
        }

        public AsyncExecutingCommand WithTask(Func<Task> task)
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

                        await _task.Invoke().ConfigureAwait(true);

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