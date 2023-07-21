using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;


public class ViewTarget : IReadOnlyList<ViewIdentifier>
{
    private readonly ViewIdentifier[] _viewIdentifiers;

    public ViewTarget(ViewIdentifier[] viewIdentifiers)
    {
        _viewIdentifiers = viewIdentifiers;
    }

    public IEnumerator<ViewIdentifier> GetEnumerator() => (IEnumerator<ViewIdentifier>)_viewIdentifiers.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => _viewIdentifiers.Length;
    public ViewIdentifier this[int index] => _viewIdentifiers[index];

}

public interface IViewNavigator
{
    Task<Response> PushAsync(IReadOnlyList<ViewIdentifier> viewTarget);
}