using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Maui.Services.TargetNavigation.Register;

public class NavigationRegister : INavigationRegister
{

    private readonly Dictionary<string, Type> _containerTypes = new();
    private readonly Dictionary<string, Type> _viewTypes = new();

    public void RegisterView<TView, TViewModel>(NavigationTarget target)
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        _viewTypes[target.Name] = typeof(TView);
    }

    public void RegisterContainer<TView, TViewModel>(NavigationContainerIdentifier containerIdentifier)
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        _containerTypes[containerIdentifier.Name] = typeof(TView);
    }

    public Type GetViewType(NavigationTarget target)
    {
        if (!_viewTypes.TryGetValue(target.Name, out var viewType))
        {
            if (target.Container is null || !_containerTypes.TryGetValue(target.Container.Name, out viewType))
            {
                throw new InvalidOperationException("No view type registered for " + target.Name);
            }
        }

        return viewType;
    }

    public Type GetContainerType(NavigationTarget target)
    {
        if (target.Container is null)
        {
            throw new InvalidOperationException("No container defined by navigation target " + target.Name);
        }
        if (!_containerTypes.TryGetValue(target.Container.Name, out var containerType))
        {
            throw new InvalidOperationException("No container type registered for " + target.Name);
        }

        return containerType;
    }
}