using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.ExecutingCommands._Base
{
    public abstract class BaseExecutingCommand<TExecutingCommand> : BaseBindableObject, IExecutingCommand
        where TExecutingCommand : BaseExecutingCommand<TExecutingCommand>
    {
        protected string? AnalyticsOperationName;
        private Func<bool>? _canExecute;

        protected readonly IErrorHandler ErrorHandler;
        protected readonly IAnalyticsService AnalyticsService;
        private IAnalyticsOperation? _analyticsOperation;
        private bool _isPageView = false;
        private object? _sender;
        private string _caller = typeof(TExecutingCommand).Name;
        protected object Sender => _sender ??= this;

        protected BaseExecutingCommand(IErrorHandler errorHandler, IAnalyticsService analyticsService)
        {
            ErrorHandler = errorHandler;
            AnalyticsService = analyticsService;
        }
         
        public TExecutingCommand WithCanExecute(Func<bool> canExecute)
        {
            _canExecute = canExecute;
            return (TExecutingCommand) this;
        }
        
        public TExecutingCommand LogOperation(object sender, string operationName)
        {
            _sender = sender;
            AnalyticsOperationName = operationName;
            return (TExecutingCommand) this;
        }

        public TExecutingCommand LogPageView(object page)
        {
            _sender = page;
            _isPageView = true;
            return (TExecutingCommand) this;
        }

        
        public bool CanExecute(object parameter) => CanExecute();
        protected bool CanExecute()
        {
            if (_canExecute == null)
            {
                return !IsExecuting;
            }

            return !IsExecuting && _canExecute.Invoke();
        }

        public bool IsExecuting
        {
            get => GetProperty<bool>();
            set => SetProperty(value, RaiseCanExecuteChanged);
        }
        
        public void Execute()
        {
            Execute(null);
        }

        public abstract void Execute(object? parameter);

        public event EventHandler? CanExecuteChanged;
        
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void Start()
        {
            IsExecuting = true;

            if (_isPageView)
            {
                _analyticsOperation = AnalyticsService.StartPageViewOperation(Sender, Sender.GetType().Name, null, _caller);
            }
            else if (AnalyticsOperationName != null && AnalyticsOperationName != string.Empty)
            {
                var properties = new Dictionary<string, object> { ["Command"] = typeof(TExecutingCommand).Name };
                _analyticsOperation = AnalyticsService.StartOperation(Sender, AnalyticsOperationName, properties, _caller);
            }
        }

        protected void Finish()
        {
            IsExecuting = false;
            _analyticsOperation?.Dispose();
        }

        protected async void Fail(object sender, Exception e)
        {
            IsExecuting = false;
            await ErrorHandler.HandleExceptionAsync(sender, e); 
            _analyticsOperation?.Dispose(); //dispose after handling error else analytics logged without operation
        }

        protected void TryExecute(object? action, Action act)
        {
            if (CanExecute())
            {
                if (action == null)
                {
                    throw new InvalidOperationException("the action for this command has not been set");
                }
                try
                {
                    Start();
                    act.Invoke();
                    Finish();
                }
                catch (Exception e)
                {
                    Fail(Sender, e);
                }
            }
        }

        protected async Task TryExecuteAsync(object? action, Func<Task> act)
        {
            if (CanExecute())
            {
                if (action == null)
                {
                    throw new InvalidOperationException("the action for this command has not been set");
                }

                try
                {
                    Start();
                    await act.Invoke();
                    Finish();
                }
                catch (Exception e)
                {
                    Fail(Sender, e);
                }
            }
        }

    }


   
}