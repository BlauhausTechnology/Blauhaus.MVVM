using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Abstractions.Navigation.Register;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.MonoGame.Screens;
using Blauhaus.MVVM.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.MVVM.MonoGame.Ioc
{
    public static class ServiceCollectionExtensions
    {
        private static readonly NavigationRegister NavigationRegister = new NavigationRegister();
        
        public static IServiceCollection AddMvvmServices(this IServiceCollection services)
        {
            return services.AddMvvmServices<MonoGameErrorHandler>();
        }

        public static IServiceCollection AddMvvmServices<TErrorHandler>(this IServiceCollection services)
            where TErrorHandler : class, IErrorHandler
        {
            services.AddSingleton<INavigationService, MonoGameNavigationService>();
            
            services
                .AddSingleton<IDialogService, MonoGameDialogService>()
                .AddSingleton<INavigationService, MonoGameNavigationService>()
                .AddSingleton<INavigationRegister>(x => NavigationRegister)
                .AddSingleton<IErrorHandler, TErrorHandler>();
            
            return services;
        }
        
        public static IServiceCollection AddPage<TPage, TViewModel>(this IServiceCollection services) 
            where TPage : BaseScreen<TViewModel>, IView 
            where TViewModel : class, IViewModel
        {
            services.TryAddSingleton<INavigationRegister>(NavigationRegister);
            
            services.AddTransient<TPage>();
            services.AddTransient<TViewModel>();

            NavigationRegister.Register<TPage, TViewModel>();
            return services;
        }
    }
}