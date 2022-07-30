using System;

namespace Blauhaus.MVVM.Abstractions.Navigator;

public interface INavigationRegister
{
    Type? GetViewType(ViewIdentifier viewIdentifier);
    Type? GetViewModelType(ViewIdentifier viewIdentifier);
}