using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Blauhaus.MVVM.Abstractions.Bindable
{
    public abstract class BaseBindableObject : INotifyPropertyChanged
    {

        protected bool SetProperty<T>(ref T backingStore, T value,  Action onChanged, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T>(Action<T> action, T value,  Action onChanged, [CallerMemberName]string propertyName = "")
        { 
            action.Invoke(value);
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}