using System.Runtime.CompilerServices;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public class NavigationTarget : BaseValueObject<NavigationTarget>
{
    private readonly string _uniqueName;
    public NavigationTarget(NavigationContainerIdentifier? container, string name, string? payload) 
    {
        Name = name;
        Payload = payload;
        Container = container;

        Uri = payload == null ? name : $"{name}?payload={payload}";
        _uniqueName = Container == null ? Uri : $"{Container.Name}::{Uri}";
    }
    
    public NavigationContainerIdentifier? Container { get; }
    public string Name { get; }
    public string? Payload { get; }
    public string Uri { get; }


    //todo maybe replace with ContainerName, RouteName, ViewName, Payload

    //if only ViewName, replace main page with page and initialize with payload
    
    //if only containerName, replace main page with shell (and initialize)
    //if containerName and route name, replace main page with shell, call GoTo with route
    //if containerName and route name and viewname, replace main page with shell, call GoTo with route/page?param=param
    
    //if only route, and main page is shell, navigate route (with parameter)
    


    #region Eqaulity etc

    public override string ToString()
    {
        return _uniqueName;
    }

    protected override int GetHashCodeCore()
    {
        return _uniqueName.GetHashCode();
    }

    protected override bool EqualsCore(NavigationTarget other)
    {
        return GetHashCode().Equals(other.GetHashCode());
    }

    #endregion;
}


public class MainPageNavigationTarget : NavigationTarget
{
    public MainPageNavigationTarget(string name, string? payload) : base(null, name, payload)
    {
    }
    
    public static NavigationTarget Create(string? payload = null, [CallerMemberName] string name = "") => new MainPageNavigationTarget(name, payload);
}
