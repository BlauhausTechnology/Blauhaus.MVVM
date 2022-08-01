﻿using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
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

    public async Task NavigateAsync(NavigationTarget target)
    {
        var applicationMainView = _platformNavigator.GetCurrentMainView();

        if (target.Container is null)
        {
            if (target.View is null)
            {
                throw new Exception("A navigation target without a container must specify a View");
            }

            var viewType = _viewRegister.GetViewType(target.View);
            var constructedView = _serviceLocator.Resolve(viewType);
            if (constructedView is not IView newView)
            {
                throw new Exception($"{constructedView.GetType().Name} must implement {nameof(IView)}");
            }
            
            _platformNavigator.SetCurrentMainView(newView);
             
        }
        else
        {
            if (applicationMainView is INavigationContainerView containerView && containerView.ViewIdentifier == target.Container)
            {
                _logger.LogTrace("Target container {ContainerName} is already the application main screen, target will be relayed to it", target.Container.Name);
                await containerView.NavigateAsync(target);
                return;
            }

            _logger.LogTrace("Target container {ContainerName} is not the application main screen, it will be created", target.Container.Name);
            
            var containerViewType = _viewRegister.GetViewType(target.Container);
            var constructedContainerView = _serviceLocator.Resolve(containerViewType);
            if (constructedContainerView is not INavigationContainerView newContainerView)
            {
                throw new Exception($"{constructedContainerView.GetType().Name} must implement {nameof(INavigationContainerView)} to be a navigation container");
            }

            _platformNavigator.SetCurrentMainView(newContainerView);
            if (target.View is not null)
            {
                await newContainerView.NavigateAsync(target);
            }
        }
         
    }
}