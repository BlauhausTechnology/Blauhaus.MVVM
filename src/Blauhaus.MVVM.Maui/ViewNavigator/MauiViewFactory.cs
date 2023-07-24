using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.Views;

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

    public async Task<Page> GetViewAsync(IViewTarget viewTarget)
    {
        var viewType = _viewRegister.GetViewType(viewTarget.View);
        object? constructedView = _serviceLocator.Resolve(viewType);
        if (constructedView is not INavigableView navigableView)
        {
            throw new Exception($"{constructedView.GetType().Name} must implement {nameof(INavigableView)}");
        }
        
        await navigableView.InitializeAsync(viewTarget);

        if (constructedView is not Page page)
        {
            throw new Exception($"{constructedView.GetType().Name} must implement {nameof(INavigableView)}");
        }

        return page;
    }
}