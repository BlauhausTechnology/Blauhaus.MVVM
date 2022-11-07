using System.Collections.Generic;
using System.ComponentModel;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.ValueProperties.Base;

public class BaseValueProperty<T> : BaseValueObject<BaseValueProperty<T>>, IValueProperty<T>
{
    private T? _value;
    private bool _isVisible;

    protected BaseValueProperty(string name, T? value, bool isVisible = true)
    {
        Name = name;
        _value = value;
        _isVisible = isVisible;
    }

    public string Name { get; }

    public T? Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => SetValue(ref _isVisible, value);

    }

    #region Equality etc

    public override string ToString()
    {
        return Value is null ? "" : GetString(Value);
    }

    protected string GetString(T value)
    {
        return value!.ToString()!;
    }

    protected override int GetHashCodeCore()
    {
        return Value == null ? 0 : Value.GetHashCode();
    }

    protected override bool EqualsCore(BaseValueProperty<T> other)
    {

        if(other.Value is not null && Value is null) return false;
        if(other.Value is null && Value is not null) return false;

        if (Value is null)
        {
            return other.Value is null;
        }

        return Value.Equals(other.Value);
    }

    
    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetValue<TProperty>(ref TProperty? field, TProperty? value)
    {
        if (EqualityComparer<TProperty>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        OnValueChanged();
    }
    
    protected virtual void OnValueChanged()
    {
    }

    #endregion
}