using System.ComponentModel;

namespace Blauhaus.MVVM.ValueProperties.Base;

public interface IValueProperty<T> : INotifyPropertyChanged
{
    public T? Value { get; set; }
    public bool IsVisible { get; set; }
}