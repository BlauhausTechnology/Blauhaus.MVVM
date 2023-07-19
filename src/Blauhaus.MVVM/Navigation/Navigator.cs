using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.Views;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Navigation;

public class Navigator : INavigator
{
    private readonly IAnalyticsLogger<Navigator> _logger;
    private readonly IPlatformNavigator _platformNavigator;
    private readonly IServiceLocator _serviceLocator;
    private readonly IViewRegister _viewRegister;

    public Navigator(
        IAnalyticsLogger<Navigator> logger,
        IPlatformNavigator platformNavigator,
        IServiceLocator serviceLocator,
        IViewRegister viewRegister)
    {
        _logger = logger;
        _platformNavigator = platformNavigator;
        _serviceLocator = serviceLocator;
        _viewRegister = viewRegister;
    }

    public async Task<INavigationContainerView> GetContainerViewAsync(NavigationTarget target, ViewIdentifier viewIdentifier)
    {
        var view = await GetViewAsync(target, viewIdentifier);
        if (view is not INavigationContainerView newContainerView)
        {
            throw new Exception($"{view.GetType().Name} must implement {nameof(INavigationContainerView)} to be a navigation container");
        }

        newContainerView.Initialize(viewIdentifier);
        return newContainerView;
    }

    public async Task<IView> GetViewAsync(NavigationTarget target, ViewIdentifier viewIdentifier)
    {
        var viewType = _viewRegister.GetViewType(viewIdentifier);
        var constructedView = _serviceLocator.Resolve(viewType);
        if (constructedView is not IView newView)
        {
            throw new Exception($"{constructedView.GetType().Name} must implement {nameof(IView)}");
        }

        if (constructedView is IAsyncInitializable<NavigationTarget> initializeable)
        {
            await initializeable.InitializeAsync(target);
        }
        if (constructedView is IAsyncInitializable<string> stringInitializeable)
        {
            await stringInitializeable.InitializeAsync(target.Serialize());
        }

        return newView;
    }
    public async Task NavigateAsync(NavigationTarget target)
    {
        var applicationMainView = _platformNavigator.GetCurrentMainView();
        
        if (target.View is null)
        {
            throw new Exception("A navigation target must specify a View");
        }

        var newView = await GetViewAsync(target, target.View);


        if (target.Container is null)
        {
            _platformNavigator.SetCurrentMainView(newView);
             
        }
        else
        { 
            if (applicationMainView is INavigationContainerView containerView && containerView.ContainerViewIdentifier == target.Container)
            {
                _logger.LogTrace("Target container {ContainerName} is already the application main screen, target will be relayed to it", target.Container.Name);
            }

            else
            {
                _logger.LogTrace("Target container {ContainerName} is not the application main screen, it will be created", target.Container.Name);

                var constructedContainerView = await GetContainerViewAsync(target, target.Container);
                 
                _platformNavigator.SetCurrentMainView(constructedContainerView);
                _logger.LogTrace("Target container {ContainerName} constructed and set as main screen", target.Container.Name);
                
                containerView = constructedContainerView;
            }

            _logger.LogTrace("Showing view {ViewName} in Container {ContainerName}", target.View.Name, target.Container.Name);

            await containerView.NavigateAsync(target, newView);

        }
         

    }
}