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
        private bool _isExecuting;

        protected readonly IErrorHandler ErrorHandler;
        protected readonly IAnalyticsService AnalyticsService;
        private IAnalyticsOperation _analyticsOperation;
        private object? _page;

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
        
        public TExecutingCommand WithAnalyticsOperationName(string operationName)
        {
            AnalyticsOperationName = operationName;
            return (TExecutingCommand) this;
        }

        public TExecutingCommand IsPageView(object page)
        {
            _page = page;
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
            get => _isExecuting;
            set => SetProperty(ref _isExecuting, value, RaiseCanExecuteChanged);
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

            if (AnalyticsOperationName != null && AnalyticsOperationName != string.Empty)
            {
                var properties = new Dictionary<string, object> { ["Command"] = typeof(TExecutingCommand).Name };
                _analyticsOperation = _page == null 
                    ? AnalyticsService.StartOperation(this, AnalyticsOperationName, properties)
                    : AnalyticsService.StartPageViewOperation(_page);
            }
        }

        protected void Finish()
        {
            IsExecuting = false;
            _analyticsOperation?.Dispose();
        }

        protected void Fail(object sender, Exception e)
        {
            IsExecuting = false;
            _analyticsOperation?.Dispose();
            ErrorHandler.HandleExceptionAsync(sender, e); 
        }

        protected void TryExecute(object sender, object? action, Action act)
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
                    Fail(sender, e);
                }
            }
        }

        protected async Task TryExecuteAsync(object sender, object? action, Func<Task> act)
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
                    Fail(sender, e);
                }
            }
        }

    }


   
}