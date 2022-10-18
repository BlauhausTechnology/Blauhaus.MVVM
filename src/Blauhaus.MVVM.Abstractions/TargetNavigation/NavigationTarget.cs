using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public class NavigationTarget : BaseValueObject<NavigationTarget>
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

    public static NavigationTarget Deserialize(string serialized)
    {
        if (serialized.StartsWith("/"))
        {
            serialized = serialized.TrimStart('/');
        }

        var pathSplit = serialized.Split('/');

        string[]? path = null;
        ViewIdentifier? view = null;

        if (pathSplit.Length > 0)
        {
            var viewString = pathSplit.Last();
            view = ViewIdentifier.Deserialize(viewString);
        }

        if (pathSplit.Length > 1)
        {
            path = new string[ pathSplit.Length - 1];
            for (var i = 0; i < pathSplit.Length-1; i++)
            {
                path[i] = pathSplit[i];
            }
        }

 
        return new NavigationTarget(null, path, view);
    }

    public string Serialize()
    {
        return _uri;
    }
    public override string ToString()
    {
        return _uri;
    }

    protected override int GetHashCodeCore()
    {
        return _uri.GetHashCode();
    }

    protected override bool EqualsCore(NavigationTarget other)
    {
        return other.ToString().Equals(_uri);
    }
}

 
