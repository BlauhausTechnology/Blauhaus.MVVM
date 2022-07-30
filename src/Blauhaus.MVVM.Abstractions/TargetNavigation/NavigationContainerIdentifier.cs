using System.Runtime.CompilerServices;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public class NavigationContainerIdentifier : BaseNamedValueObject<NavigationContainerIdentifier>
{
    public NavigationContainerIdentifier(string name) : base(name)
    {
    }

    public static NavigationContainerIdentifier Create([CallerMemberName] string name = "") => new(name);
}