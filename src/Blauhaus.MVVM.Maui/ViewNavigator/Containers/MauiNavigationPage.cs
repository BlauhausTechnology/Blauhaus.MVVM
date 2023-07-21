using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Maui.ViewNavigator.Containers;

public class MauiNavigationPage : NavigationPage, IMauiViewContainer
{
    private readonly IAnalyticsLogger<MauiNavigationPage> _logger;
    private readonly IMauiViewFactory _viewFactory;

    public MauiNavigationPage(
        IAnalyticsLogger<MauiNavigationPage> logger,
        IMauiViewFactory viewFactory)
    {
        _logger = logger;
        _viewFactory = viewFactory;
    }

    public ViewIdentifier Identifier { get; private set; } = null!;
    
    public Task InitializeAsync(ViewIdentifier value)
    {
        Identifier = value;
        return Task.CompletedTask;
    }


    Task IMauiViewContainer.PushAsync(Page page)
    {
        return PushAsync(page);
    }


    Task IMauiViewContainer.PopAsync()
    {
        return PopAsync();
    }
}