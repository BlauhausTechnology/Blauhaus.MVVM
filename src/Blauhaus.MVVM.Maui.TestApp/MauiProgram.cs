﻿using Blauhaus.Analytics.Serilog.Maui.Ioc;
using Blauhaus.DeviceServices.Maui.Ioc;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Ioc;
using Blauhaus.MVVM.Maui.Ioc;
using Blauhaus.MVVM.Maui.TestApp.Navigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views;
using CommunityToolkit.Maui.Markup;

namespace Blauhaus.MVVM.Maui.TestApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<TestApp>()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services
                .AddMauiApplication()
                .AddExecutingCommands()
                .AddMauiSerilogAnalytics("Test app", config => { });

            builder.Services
                .AddView<LoadingView, LoadingViewModel>(AppViews.LoadingView)
                .AddView<MainContainerView, MainContainerViewModel>(AppViews.MainContainerView);

            return builder.Build();
        }
    }
}