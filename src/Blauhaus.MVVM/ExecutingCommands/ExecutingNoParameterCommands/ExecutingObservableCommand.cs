using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands
{
    public class ExecutingObservableCommand<T> : BaseExecutingNoParameterCommand<ExecutingObservableCommand<T>>
    {

        private IObservable<T>? _observable;
        private Action<T>? _onNext;
        private Action? _onCompleted;
        
        public ExecutingObservableCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler) 
                : base(serviceLocator, errorHandler)
        {
        }
         
        public ExecutingObservableCommand<T> Observe(IObservable<T> observable)
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

        public override void Execute(object parameter)
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
                            Fail(e);
                        }
                    },
                    error =>
                    {
                        //OnError
                        Fail(error);
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