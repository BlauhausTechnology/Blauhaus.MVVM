using System.ComponentModel;
using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Contracts;

namespace Blauhaus.MVVM.Abstractions.Commands
{
    public interface IExecutingCommand : IExecute, INotifyPropertyChanged, ICommand
    { 
        void RaiseCanExecuteChanged();
        void Execute();
    }
}