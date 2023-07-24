using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigator;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public class MauiViewFactory : IMauiViewFactory
{
    private readonly IViewRegister _viewRegister;
    private readonly IServiceLocator _serviceLocator;

    public MauiViewFactory(
        IViewRegister viewRegister,
        IServiceLocator serviceLocator)
    {
        _viewRegister = viewRegister;
        _serviceLocator = serviceLocator;
    }

    public async Task<Page> GetViewAsync(ViewIdentifier viewIdentifier)
    {
        var viewType = _viewRegister.GetViewType(viewIdentifier);
        object? constructedView = _serviceLocator.Resolve(viewType);
        if (constructedView is not INavigableView navigableView)
        {
            throw new Exception($"{constructedView.GetType().Name} must implement {nameof(INavigableView)}");
        }
        
        if (constructedView is not Page page)
        {
            throw new Exception($"{constructedView.GetType().Name} must implement {nameof(INavigableView)}");
        }

        if (constructedView is IAsyncInitializable<ViewIdentifier> initializeable)
        {
            await initializeable.InitializeAsync(viewIdentifier);
        }
        if (constructedView is IAsyncInitializable<string> stringInitializeable)
        {
            await stringInitializeable.InitializeAsync(viewIdentifier.Serialize());
        }

        return page;
    }
}