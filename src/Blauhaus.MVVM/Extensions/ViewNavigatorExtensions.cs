using System.Threading.Tasks;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Navigation;

namespace Blauhaus.MVVM.Extensions;

public static class ViewNavigatorExtensions
{
    public static Task NavigateAsync(this IViewNavigator viewNavigator, params ViewIdentifier[] viewIdentifier)
    {
        return viewNavigator.NavigateAsync(ViewTarget.Create(viewIdentifier));
    }
}