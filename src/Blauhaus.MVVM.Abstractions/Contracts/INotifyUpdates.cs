using System.ComponentModel;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface INotifyUpdates : INotifyPropertyChanged
    {
        public object? Update { get; }

    }
}