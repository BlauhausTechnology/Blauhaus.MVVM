using Blauhaus.Analytics.Serilog.Maui.Ioc;
using Blauhaus.DeviceServices.Maui.Ioc;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Ioc;
using Blauhaus.MVVM.Maui.Ioc;
using Blauhaus.MVVM.Maui.TestApp.Navigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using static Blauhaus.MVVM.Maui.TestApp.Navigation.AppNavigation.Views;

namespace Blauhaus.MVVM.Maui.TestApp
{
    public static class MauiProgram
    {
        public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
        {
            var builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
            builder
                .UseMauiApp<MauiApp>()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services
                .AddMauiServices()
                .AddMauiNavigator()
                .AddMauiDeviceServices()
                .AddServiceLocator()
                .AddExecutingCommands()
                .AddMauiSerilogAnalytics("Test app", config => { });


            builder.Services
                .AddView<LoadingView, LoadingViewModel>(LoadingViewIdentifier) 
                .AddView<FullScreenView, FullScreenViewModel>(FullScreenViewIdentifier)  
                .AddView<MainContainerView, MainContainerViewModel>(ContainerViewIdentifier)
                ;

            return builder.Build();
        }
    }
}