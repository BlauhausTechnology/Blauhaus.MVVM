using System;
using Blauhaus.Common.ValueObjects.Navigation;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface IViewRegister
{
    Type GetViewType(ViewIdentifier viewIdentifier);
}