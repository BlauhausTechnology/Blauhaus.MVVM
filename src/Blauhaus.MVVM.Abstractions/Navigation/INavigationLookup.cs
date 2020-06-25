using System;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public interface INavigationLookup
    {
        Type? GetViewType(Type viewModelType);
    }
}