using System;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public interface INavigationLookup
    {
        Type? GetViewType(Type viewModelType);
    }
}