using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands._Base;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands
{
    public class ExecutingObservableCommand<T> : BaseExecutingNoValueCommand
    {
        public ExecutingObservableCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            IObservable<T> observable,
            Action<T> onNext,
            Action? onCompleted = null,
            Func<bool>? canExecute = null,
            string operationName = null) 
                : base(errorHandler, canExecute)
        {
            Command = new Command(() =>
            {
                if (CanExecute())
                {
                    operationName ??= "Observable Command of " + typeof(T).Name;

                    var analyticsOperation = analyticsService.StartOperation(this, operationName);

                    IsExecuting = true;

                    observable.Subscribe(
                        next =>
                        {
                            //OnNext
                            try
                            {
                                onNext.Invoke(next);
                            }
                            catch (Exception e)
                            {
                                IsExecuting = false;
                                analyticsOperation.Dispose();
                                errorHandler.HandleExceptionAsync(this, e);
                            }
                        },
                        error =>
                        {
                            //OnError
                            IsExecuting = false;
                            analyticsOperation.Dispose();
                            errorHandler.HandleExceptionAsync(this, error); 
                        },
                        () =>
                        {
                            //OnCompleted
                            IsExecuting = false;
                            analyticsOperation.Dispose();
                            onCompleted?.Invoke();
                        });
                }
            }, CanExecute);
        }
         
    }
}