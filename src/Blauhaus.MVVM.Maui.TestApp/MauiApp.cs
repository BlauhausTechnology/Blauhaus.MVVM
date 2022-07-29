using Blauhaus.MVVM.Maui.TestApp.Views;

namespace Blauhaus.MVVM.Maui.TestApp
{
    public class MauiApp : Application
    {
        public MauiApp(IServiceProvider serviceProvider)
        {
            MainPage = serviceProvider.GetRequiredService<LoadingView>();
        }
    }
}