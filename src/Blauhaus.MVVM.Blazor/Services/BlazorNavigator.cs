using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.Views;
using Microsoft.AspNetCore.Components;

namespace Blauhaus.MVVM.Blazor.Services;

public class BlazorNavigator : INavigator
{
    private readonly IAnalyticsLogger<BlazorNavigator> _logger;
    private readonly NavigationManager _navigationManager;

    public BlazorNavigator(
        IAnalyticsLogger<BlazorNavigator> logger,
        NavigationManager navigationManager)
    {
        _logger = logger;
        _navigationManager = navigationManager;
    }

    public async Task NavigateAsync(NavigationTarget target)
    {
        _navigationManager.NavigateTo(target.ToString());
    }

    public Task<INavigationContainerView> GetContainerViewAsync(NavigationTarget target, ViewIdentifier viewIdentifier)
    {
        throw new NotImplementedException();
    }

    public Task<IView> GetViewAsync(NavigationTarget target, ViewIdentifier viewIdentifier)
    {
        throw new NotImplementedException();
    }
}