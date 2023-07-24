using System;
using Blauhaus.Common.ValueObjects.Navigation;

namespace Blauhaus.MVVM.Abstractions.Navigator;

public interface IViewRegister
{
    Type GetViewType(ViewIdentifier viewIdentifier);
}