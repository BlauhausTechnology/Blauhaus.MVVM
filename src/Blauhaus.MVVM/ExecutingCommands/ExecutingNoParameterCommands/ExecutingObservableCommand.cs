using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class ExecutingObservableCommand<T> : BaseExecutingNoParameterCommand<ExecutingObservableCommand<T>>
    {

        private IObservable<T>? _observable;
        private Action<T>? _onNext;
        private Action? _onCompleted;
        
        public ExecutingObservableCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService) 
            : base(errorHandler, analyticsService)
        {
        }
         
        public ExecutingObservableCommand<T> WithObservable(IObservable<T> observable)
        {
            _observable = observable;
            return this;
        }
        
        public ExecutingObservableCommand<T> OnNext(Action<T> onNext)
        {
            _onNext = onNext;
            return this;
        }
        
        public ExecutingObservableCommand<T> OnCompleted(Action onCompleted)
        {
            _onCompleted = onCompleted;
            return this;
        }

        public override void Execute()
        {
            if (CanExecute())
            {

                if (_onNext == null)
                {
                    throw new InvalidOperationException("the action for this command has not been set");
                }

                Start();

                _observable.Subscribe(next =>
                    {
                        //OnNext
                        try
                        {
                            _onNext.Invoke(next);
                        }
                        catch (Exception e)
                        {
                            Fail(this, e);
                        }
                    },
                    error =>
                    {
                        //OnError
                        Fail(this, error);
                    },
                    () =>
                    {
                        //OnCompleted
                        _onCompleted?.Invoke();
                        Finish();
                    });
            }
        }
    }
}