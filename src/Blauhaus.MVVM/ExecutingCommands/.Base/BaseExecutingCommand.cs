using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.ExecutingCommands.Base
{
    public abstract class BaseExecutingCommand<TExecutingCommand> : BaseBindableObject, IExecutingCommand
        where TExecutingCommand : BaseExecutingCommand<TExecutingCommand>
    {
        protected string? AnalyticsOperationName;
        private Func<bool>? _canExecute;

        protected readonly IErrorHandler ErrorHandler;
        private readonly IServiceLocator _serviceLocator;
        protected readonly IAnalyticsService AnalyticsService;
        private IDisposable? _cleanup;
        private bool _isPageView = false;
        private object? _sender;
        private readonly string _caller = typeof(TExecutingCommand).Name;
        private IAnalyticsLogger? _logger;
        private Func<IDisposable>? _loggerFunc;
        protected object Sender => _sender ??= this;

        protected BaseExecutingCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService)
        {
            ErrorHandler = errorHandler;
            _serviceLocator = serviceLocator;
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

        public TExecutingCommand LogAction<TSource>(LogLevel logLevel, string message, params object[] args)
        {
            _logger = _serviceLocator.Resolve<IAnalyticsLogger<TSource>>();
            _loggerFunc = () =>
            {
                _logger.SetValue("ActionId", Guid.NewGuid());
                var disposable = _logger.BeginTimedScope(logLevel, message, args);
                return disposable;
            };
            return (TExecutingCommand)this;
        }

        public TExecutingCommand LogAction<TSource>(string message, params object[] args)
        {
            return LogAction<TSource>(LogLevel.Information, message, args);
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
                _cleanup = AnalyticsService.StartPageViewOperation(Sender, Sender.GetType().Name, null, _caller);
            }
            else if (!string.IsNullOrEmpty(AnalyticsOperationName))
            {
                var properties = new Dictionary<string, object> { ["Command"] = typeof(TExecutingCommand).Name };
                _cleanup = AnalyticsService.StartOperation(Sender, AnalyticsOperationName, properties, _caller);
            }
            else if (_loggerFunc != null)
            {
                _cleanup = _loggerFunc.Invoke();
            }
        }

        protected void Finish()
        {
            IsExecuting = false;
            _cleanup?.Dispose();
        }

        protected async void Fail(object sender, Exception e)
        {
            IsExecuting = false;
            await ErrorHandler.HandleExceptionAsync(sender, e); 
            _cleanup?.Dispose(); //dispose after handling error else analytics logged without operation
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