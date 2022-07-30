using Blauhaus.MVVM.Abstractions.Navigator;

namespace Blauhaus.MVVM.Maui.TestApp.Navigation;

public static class AppNavigation
{
    public static class Views
    {
        public static ViewIdentifier LoadingViewIdentifier = ViewIdentifier.Create();
        public static ViewIdentifier FullScreenViewIdentifier = ViewIdentifier.Create();
        public static ViewIdentifier ContainerViewIdentifier = ViewIdentifier.Create();
    }


    public static NavigationTarget Loading = new(Views.LoadingViewIdentifier);
    public static NavigationTarget FullScreen = new(Views.FullScreenViewIdentifier);
    public static NavigationTarget Container = new(Views.ContainerViewIdentifier);
}