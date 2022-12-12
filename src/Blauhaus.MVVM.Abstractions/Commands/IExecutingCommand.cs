using System.ComponentModel;
using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Contracts;

namespace Blauhaus.MVVM.Abstractions.Commands
{
    public interface IExecutingCommand : IIsExecuting, INotifyPropertyChanged, ICommand, IIsVisible
    { 
        void RaiseCanExecuteChanged();
        void Execute();
    }
}