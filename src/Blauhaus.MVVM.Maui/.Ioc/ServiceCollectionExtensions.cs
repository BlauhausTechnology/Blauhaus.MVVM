﻿using Blauhaus.DeviceServices.Maui.Ioc;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation.UriNavigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Ioc;
using Blauhaus.MVVM.Maui.Services;
using Blauhaus.MVVM.Maui.Services.Navigation;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.MVVM.Maui.Ioc;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddMauiApplication(this IServiceCollection services)
    {

        services
            .AddMauiServices()
            .AddMauiNavigator()
            .AddMauiDeviceServices();

        services.AddSingleton<IServiceLocator>(sp => new DotNetCoreServiceLocator(sp));
        services.TryAddSingleton<IAppLifecycleService, AppLifecycleService>();


        return services;
    }

    private static IServiceCollection AddMauiServices(this IServiceCollection services)
    {
        services
            .AddTransient<IErrorHandler, MauiErrorHandler>()
            .AddTransient<IDialogService, MauiDialogService>();

        return services;
    }

    private static IServiceCollection AddMauiNavigator(this IServiceCollection services)
    {
        services
            .AddSingleton<IPlatformNavigator, MauiNavigator>()
            .AddNavigator();

        return services;
    }

    public static IServiceCollection AddMauiView<TView, TViewModel>(this IServiceCollection services, ViewIdentifier viewIdentifier) 
        where TView : Page, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        services.AddView<TView, TViewModel>(viewIdentifier);
        Routing.RegisterRoute(viewIdentifier.Name, typeof(TView));
        return services;
    }
     
}