using Blauhaus.Analytics.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Maui.Services.Application;
using Blauhaus.MVVM.Maui.Services.TargetNavigation.Register;

namespace Blauhaus.MVVM.Maui.Services.TargetNavigation;

public class MauiTargetNavigator : ITargetNavigator
{
    private readonly IAnalyticsLogger<MauiTargetNavigator> _logger;
    private readonly IMauiApplicationProxy _mauiApplicationProxy;
    private readonly IServiceLocator _serviceLocator;
    private readonly INavigationRegister _navigationRegister;

    public MauiTargetNavigator(
        IAnalyticsLogger<MauiTargetNavigator> logger,
        IMauiApplicationProxy mauiApplicationProxy,
        IServiceLocator serviceLocator,
        INavigationRegister navigationRegister)
    {
        _logger = logger;
        _mauiApplicationProxy = mauiApplicationProxy;
        _serviceLocator = serviceLocator;
        _navigationRegister = navigationRegister;
    }

    public async Task NavigateAsync(NavigationTarget target)
    {
        if (target.Container is not null)
        {
            var targetContainerType = _navigationRegister.GetContainerType(target);

            var currentMainPage = _mauiApplicationProxy.MainPage;
            if (currentMainPage is null || currentMainPage.GetType() != targetContainerType || currentMainPage is not INavigationContainerView)
            {
                var containerType = _navigationRegister.GetContainerType(target);
                var container = _serviceLocator.ResolveAs<Shell>(containerType);
                if (container is not INavigationContainerView)
                {
                    throw new InvalidOperationException($"{container.GetType().Name} must implement {nameof(INavigationContainerView)}");
                }
                
                _mauiApplicationProxy.MainPage = container;
            }
            
            await (_mauiApplicationProxy.MainPage as INavigationContainerView)!.NavigateAsync(target);
        }

        else
        {
            var viewType = _navigationRegister.GetViewType(target);
            var newMainPage = _serviceLocator.ResolveAs<Page>(viewType);
            _mauiApplicationProxy.MainPage = newMainPage;
        }
    }


}