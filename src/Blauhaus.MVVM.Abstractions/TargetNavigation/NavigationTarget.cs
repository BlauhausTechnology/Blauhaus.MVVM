using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public class NavigationTarget 
{
    private readonly string _uri;

    public NavigationTarget(ViewIdentifier? container, IReadOnlyList<string>? path, ViewIdentifier? view)
    {
        Container = container;
        Path = path;
        View = view;

        if (Container is null && View is null)
        {
            throw new InvalidOperationException("A navigation target must specific either a Container or a View or both");
        }

        var uri = new StringBuilder();
        if (Path is not null)
        {
            foreach (var route in Path)
            {
                uri.Append('/').Append(route);
            }
        }

        if (View is not null)
        {
            uri.Append(View);
        }

        _uri = uri.ToString();
    }

    public ViewIdentifier? Container { get; }
    public IReadOnlyList<string>? Path { get; }
    public ViewIdentifier? View  { get; }


    public static NavigationTarget CreateContainer(ViewIdentifier container) => new(container, null, null);
    public static NavigationTarget CreateView(ViewIdentifier view) => new(null, null, view);

    public NavigationTarget WithView(ViewIdentifier view) => new(Container, Path, view);
    public NavigationTarget WithContainer(ViewIdentifier container) => new(container, Path, View);
    public NavigationTarget WithPath(params string[] path) => new(Container, path, View);

    public NavigationTarget WithViewProperty(string key, string value)
    {
        if (View is null) throw new InvalidOperationException("Cannot add properties to non-existent View");
        var properties = View.Properties;
        properties[key] = value;
        return new NavigationTarget(Container, Path, View);
    }


    //todo maybe replace with ContainerName, RouteName, ViewName, Payload
    //actually just make it IHasProperties

    //if only ViewName, replace main page with page and initialize with payload

    //if only containerName, replace main page with shell (and initialize)
    //if containerName and route name, replace main page with shell, call GoTo with route
    //if containerName and route name and viewname, replace main page with shell, call GoTo with route/page?param=param

    //if only route, and main page is shell, navigate route (with parameter)



    #region Eqaulity etc

    public override string ToString()
    {
        return _uri;
    }

    //protected override int GetHashCodeCore()
    //{
    //    return _uniqueName.GetHashCode();
    //}

    //protected override bool EqualsCore(NavigationTarget other)
    //{
    //    return GetHashCode().Equals(other.GetHashCode());
    //}

    #endregion;
}

 
