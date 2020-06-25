using System;
using Blauhaus.MVVM.Abstractions.Bindable;

namespace Blauhaus.MVVM.Abstractions.State
{
    public class State<T> : BaseBindableObject
    {
        private T _value;
        private readonly Action? _callback;

        public State(T initialValue, Action? callback = null)
        {
            _value = initialValue;
            _callback = callback;
        }

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value, () => _callback?.Invoke());
        }
    }
}