﻿using System;
using System.Collections;
using Blauhaus.Common.ValueObjects.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blauhaus.MVVM.Abstractions.Navigator;

namespace Blauhaus.MVVM.Navigation;


public class ViewTarget : IViewTarget
{
    private readonly IReadOnlyList<ViewIdentifier> _viewIdentifiers;
    private string? _path;

    public ViewTarget(IReadOnlyList<ViewIdentifier> viewIdentifiers, ViewIdentifier? view = null)
    {   
        _viewIdentifiers = viewIdentifiers;
        View = view ?? viewIdentifiers.Last();
    }

    public static ViewTarget Create (params ViewIdentifier[] viewIdentifiers) => new(viewIdentifiers);

    public IEnumerator<ViewIdentifier> GetEnumerator() => _viewIdentifiers.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => _viewIdentifiers.Count;
    public ViewIdentifier this[int index] => _viewIdentifiers[index];
    public ViewIdentifier View { get; }
    public string Path
    {
        get
        {
            if (_path == null)
            {
                var path = new StringBuilder();
                foreach (var viewIdentifier in _viewIdentifiers)
                {
                    path.Append('/').Append(viewIdentifier.Name);

                    var parameterAdded = false;
                    foreach (var prop in viewIdentifier.Properties)
                    {
                        if (!parameterAdded)
                        {
                            
                            parameterAdded = true;
                            path.Append('?');
                        }
                        else
                        {
                            path.Append('&');
                        }
                        path.Append(prop.Key).Append('=').Append(prop.Value);
                    }
                }

                _path = path.ToString();
            }

            return _path;
        }
    }

    public static IViewTarget Deserialize(string serialized)
    {
        string[] pathComponents = serialized.Split('/');

        var viewIdentifiers = pathComponents
            .Skip(1) //the first component is empty from the leading '/'
            .Select(ViewIdentifier.Deserialize)
            .ToArray();
         
        return new ViewTarget(viewIdentifiers);
    }
}
