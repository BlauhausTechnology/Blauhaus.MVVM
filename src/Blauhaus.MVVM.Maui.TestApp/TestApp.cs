using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Maui.TestApp.Navigation;

namespace Blauhaus.MVVM.Maui.TestApp
{
    public class TestApp : Application
    {
        private readonly INavigator _navigator;

        public TestApp(IServiceProvider serviceProvider)
        {
            _navigator = serviceProvider.GetRequiredService<INavigator>();
            MainPage = new ContentPage{BackgroundColor = Color.FromRgb(0,0,120)};
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await _navigator.NavigateAsync(NavigationTarget.CreateView(AppViews.LoadingView));
        }
    }
}