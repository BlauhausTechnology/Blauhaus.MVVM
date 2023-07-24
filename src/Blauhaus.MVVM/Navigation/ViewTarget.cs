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
                var paramaters = new StringBuilder();
                foreach (var viewIdentifier in _viewIdentifiers)
                {
                    path.Append('/').Append(viewIdentifier.Name);

                    foreach (var prop in viewIdentifier.Properties)
                    {
                        paramaters.Append(paramaters.Length == 0 ? '?' : '&');
                        paramaters.Append(prop.Key).Append('=').Append(prop.Value);
                    }
                }

                _path = path.Append(paramaters).ToString();
            }

            return _path;
        }
    }
}
