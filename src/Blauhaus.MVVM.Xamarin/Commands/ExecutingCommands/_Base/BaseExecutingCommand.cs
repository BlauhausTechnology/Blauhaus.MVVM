using System;
using System.Windows.Input;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Commands;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands._Base
{
    public abstract class BaseExecutingCommand : BaseBindableObject, IExecutingCommand
    {
        private readonly Func<bool>? _canExecute;
        private bool _isExecuting;

        protected readonly IErrorHandler ErrorHandler;

        protected BaseExecutingCommand(IErrorHandler errorHandler, Func<bool>? canExecute)
        {
            ErrorHandler = errorHandler;
            _canExecute = canExecute;
        }
        
        public ICommand Command { get; protected set; }
        
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
            set => SetProperty(ref _isExecuting, value, OnIsExecutingChanged);
        }
        
        public void Execute(object? parameter = null)
        {
            Command.Execute(parameter);
        }

        private void OnIsExecutingChanged()
        {
            RaiseCanExecuteChanged();
        }
        
        public void RaiseCanExecuteChanged()
        {
            ((Command)Command).ChangeCanExecute();
        }

    }


   
}