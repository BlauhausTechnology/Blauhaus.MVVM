﻿using System.ComponentModel;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Maui.MauiApplication;
using Blauhaus.MVVM.Maui.Services;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.Ioc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMauiServices(this IServiceCollection services)
    {
        services
            .AddTransient<IErrorHandler, MauiShellErrorHandler>()
            .AddSingleton<IUriNavigator, MauiShellUriNavigator>()
            .AddTransient<IDialogService, MauiShellDialogService>();

        services.AddSingleton<IMauiApplication, MauiApplication.MauiApplication>();

        return services;
    }
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services) 
        where TView : BaseMauiContentPage<TViewModel>
        where TViewModel : class
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();
        Routing.RegisterRoute(typeof(TViewModel).Name, typeof(TView));

        return services;
    }
    public static IServiceCollection AddShell<TShell, TViewModel>(this IServiceCollection services) 
        where TShell : BaseMauiShell<TViewModel>
        where TViewModel : class
    {
        services.AddTransient<TShell>();
        services.AddTransient<TViewModel>();
        Routing.RegisterRoute(typeof(TViewModel).Name, typeof(TShell));

        return services;
    }

    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, string route) 
        where TView : BaseMauiContentPage<TViewModel>
        where TViewModel : class
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();
        Routing.RegisterRoute(route, typeof(TView));

        return services;
    }
}