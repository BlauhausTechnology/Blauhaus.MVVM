using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blauhaus.MVVM.Blazor.ViewNavigator;

public class BlazorViewNavigator : IViewNavigator
{
    private readonly IAnalyticsLogger<BlazorViewNavigator> _logger;
    private readonly NavigationManager _navigationManager;
    private readonly IJSRuntime _jsRuntime;

    public BlazorViewNavigator(
        IAnalyticsLogger<BlazorViewNavigator> logger,
        NavigationManager navigationManager,
        IJSRuntime jsRuntime)
    {
        _logger = logger;
        _navigationManager = navigationManager;
        _jsRuntime = jsRuntime;
    }


    public Task PushAsync(IViewTarget viewTarget)
    {
        _navigationManager.NavigateTo(viewTarget.Path);
        return Task.CompletedTask;
    }

    public async Task PopAsync()
    {
        await _jsRuntime.InvokeVoidAsync("history.back");
    }
}