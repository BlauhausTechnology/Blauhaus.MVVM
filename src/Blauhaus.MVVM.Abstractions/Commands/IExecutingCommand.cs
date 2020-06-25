using System.ComponentModel;
using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Abstractions.Commands
{
    public interface IExecutingCommand : IExecuting, INotifyPropertyChanged
    {
        ICommand Command { get; }
        void Execute(object? parameter = null);
        void RaiseCanExecuteChanged();
    }
}