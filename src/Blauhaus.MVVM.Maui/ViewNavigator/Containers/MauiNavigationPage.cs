using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Maui.ViewNavigator.Containers;

public class MauiNavigationPage : NavigationPage, IMauiViewContainer
{
    private readonly IMauiViewNavigator _viewNavigator;

    public MauiNavigationPage( 
        IMauiViewNavigator viewNavigator, Page rootPage) 
            : base(rootPage)
    {
        _viewNavigator = viewNavigator;
    }
    
    public Task InitializeAsync(IViewTarget value)
    {
        ViewTarget = value;
        return Task.CompletedTask;
    }

    public IViewTarget ViewTarget { get; private set; } = null!;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewNavigator.SetActive(this);
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