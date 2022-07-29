using Blauhaus.Analytics.Serilog.Maui.Ioc;
using Blauhaus.DeviceServices.Maui.Ioc;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Ioc;
using Blauhaus.MVVM.Maui.Ioc;
using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

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
                .AddMauiDeviceServices()
                .AddServiceLocator()
                .AddExecutingCommands()
                .AddMauiSerilogAnalytics("Test app", config => { });


            builder.Services
                .AddShell<StartShell, StartViewModel>()
                .AddView<LoadingView, LoadingViewModel>();

            return builder.Build();
        }
    }
}