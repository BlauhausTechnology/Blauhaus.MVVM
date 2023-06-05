using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.Extensions;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.ExecutingCommands.Base
{
    public abstract class BaseExecutingCommand<TExecutingCommand> : BaseBindableObject, IExecutingCommand
        where TExecutingCommand : BaseExecutingCommand<TExecutingCommand>
    {
        private Func<bool>? _canExecute;

        protected readonly IErrorHandler ErrorHandler;
        private readonly IServiceLocator _serviceLocator;
        private IDisposable? _cleanup;
        private IAnalyticsLogger? _logger;
        private Func<IDisposable>? _loggerFunc;
        private IIsExecuting? _externalIsExecuting;
        private PropertyInfo[]? _externalCommandProperties;

        protected BaseExecutingCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;
            _serviceLocator = serviceLocator;
        }
         
        public TExecutingCommand WithCanExecute(Func<bool> canExecute)
        {
            _canExecute = canExecute;
            return (TExecutingCommand) this;
        }

        
        public TExecutingCommand LogAction<TSource>(string actionName, LogLevel logLevel = LogLevel.Information)
        {
            _logger = _serviceLocator.Resolve<IAnalyticsLogger<TSource>>();
            _loggerFunc = () =>
            {
                _logger.SetValue("ActionId", Guid.NewGuid().ToString());
                _logger.SetValue("ActionName",actionName);
                _logger.SetValue("ActionSource",typeof(TSource).Name);
                var disposable = _logger.BeginTimedScope(logLevel, actionName);
                return disposable;
            };
            return (TExecutingCommand)this;
        }

        public TExecutingCommand WithIsExecuting(IIsExecuting externalIsExecuting)
        {
            _externalIsExecuting = externalIsExecuting;
            return (TExecutingCommand)this;
        }
        public bool CanExecute(object parameter) => CanExecute();
        protected bool CanExecute()
        {
            if (_externalIsExecuting is not null && _externalIsExecuting.IsExecuting)
                return false;
            
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
            SetIsExecuting(true);
            
            if (_loggerFunc != null)
            {
                _cleanup = _loggerFunc.Invoke();
            }
        }

        protected void Finish()
        {
            SetIsExecuting(false);
            _cleanup?.Dispose();
        }

        protected async void Fail(Exception e)
        {
            SetIsExecuting(false);
            await ErrorHandler.HandleExceptionAsync(this, e); 
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
                    Fail(e);
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
                    Fail(e);
                }
            }
        }

        public bool IsVisible
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        private void SetIsExecuting(bool value)
        {
            IsExecuting = value;
            if (_externalIsExecuting == null) return;
            _externalIsExecuting.IsExecuting = value;

            return;

            //todo
            _externalCommandProperties ??= _externalIsExecuting.GetExecutingCommandProperties();

            foreach (var externalCommandProperty in _externalCommandProperties)
            {
                try
                {
                    var command = this.GetCommand(externalCommandProperty);
                    if (command == null)
                    {
                        _logger?.LogInformation($"Command for {externalCommandProperty.Name} is null");
                    }
                    else
                    {
                        command.RaiseCanExecuteChanged();
                        _logger?.LogInformation($"RaiseCanExecuteChanged called on {externalCommandProperty.Name}");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to raise external IsExecuting for " + externalCommandProperty.Name, e);
                }
            }
        }

    }


   
}