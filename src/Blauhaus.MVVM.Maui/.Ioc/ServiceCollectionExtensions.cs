using System.ComponentModel;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Maui.MauiApplication;
using Blauhaus.MVVM.Maui.Navigator;
using Blauhaus.MVVM.Maui.Services;
using Blauhaus.MVVM.Maui.Views;
using Blauhaus.MVVM.Navigator;

namespace Blauhaus.MVVM.Maui.Ioc;

public static class ServiceCollectionExtensions
{
    private static NavigationRegister NavigationRegister = new();

    public static IServiceCollection AddMauiServices(this IServiceCollection services)
    {
        services
            .AddTransient<IErrorHandler, MauiErrorHandler>()
            .AddSingleton<IUriNavigator, MauiShellUriNavigator>()
            .AddTransient<IDialogService, MauiShellDialogService>();

        services.AddSingleton<IMauiApplication, MauiApplication.MauiApplication>();

        return services;
    }

    public static IServiceCollection AddMauiNavigator(this IServiceCollection services)
    {
        services.AddSingleton<INavigator, MauiNavigator>();
        services.AddSingleton<INavigationRegister>(_ => NavigationRegister);

        return services;
    }
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, ViewIdentifier viewIdentifier) 
        where TView : class, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        return services.AddView<TView, TViewModel>(viewIdentifier.Name);
    }

    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, string name) 
        where TView : class, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();

        Routing.RegisterRoute(name, typeof(TView));
        NavigationRegister.Register<TView, TViewModel>(name);

        return services;
    }
}