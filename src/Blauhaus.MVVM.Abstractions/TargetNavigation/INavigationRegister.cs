using System;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface IViewRegister
{
    Type GetViewType(ViewIdentifier viewIdentifier);
}