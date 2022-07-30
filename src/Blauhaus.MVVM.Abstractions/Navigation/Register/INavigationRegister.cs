using System;

namespace Blauhaus.MVVM.Abstractions.Navigation.Register
{
    public interface INavigationRegister
    {
        Type? GetViewType(Type viewModelType);
    }
}