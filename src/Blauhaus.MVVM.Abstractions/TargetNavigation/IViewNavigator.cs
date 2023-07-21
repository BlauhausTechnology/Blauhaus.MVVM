using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface IViewTarget : IReadOnlyList<ViewIdentifier>
{
    string Path { get; }
}

public class ViewTarget : IReadOnlyList<ViewIdentifier>
{
    private readonly ViewIdentifier[] _viewIdentifiers;
    private string? _path;

    public ViewTarget(ViewIdentifier[] viewIdentifiers)
    {   
        _viewIdentifiers = viewIdentifiers;
    }

    public static ViewTarget Create (params ViewIdentifier[] viewIdentifiers) => new(viewIdentifiers);
    

    public IEnumerator<ViewIdentifier> GetEnumerator() => (IEnumerator<ViewIdentifier>)_viewIdentifiers.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => _viewIdentifiers.Length;
    public ViewIdentifier this[int index] => _viewIdentifiers[index];

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

public interface IViewNavigator
{
    Task NavigateAsync(IViewTarget viewTarget);
    Task GoBackAsync();
}


public interface INavigableView : IView
{
    ViewIdentifier Identifier { get; }
}