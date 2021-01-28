using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Localization;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Localization;
using Blauhaus.MVVM.Xamarin.Dialogs;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Navigation;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Blauhaus.MVVM.Xamarin.Views.Content;
using Blauhaus.MVVM.Xamarin.Views.ContentViews;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin._Ioc
{
    public static class ServiceCollectionExtensions
    {

        private static readonly NavigationLookup NavigationLookup = new NavigationLookup();

        public static IServiceCollection AddMvvmServices(this IServiceCollection services)
        {
            services
                .AddSingleton<ILocalizationService, LocalizationService>()
                .AddSingleton<IDialogService, FormsDialogService>()
                .AddSingleton<INavigationService, FormsNavigationService>()
                .AddSingleton<IFormsApplicationProxy, FormsApplicationProxy>()
                .AddSingleton<INavigationLookup>(x => NavigationLookup)
                .AddSingleton<IErrorHandler, ErrorHandler>();
            
            return services;
        }
         
        public static IServiceCollection AddPageSingleton<TView, TViewModel>(this IServiceCollection services) 
            where TView : Page, IView 
            where TViewModel : class, IViewModel
        {
            services.TryAddSingleton<INavigationLookup>(NavigationLookup);

            services.AddSingleton<TView>();
            services.AddSingleton<TViewModel>();

            NavigationLookup.Register<TView, TViewModel>();
            return services;
        }

        public static IServiceCollection AddPage<TPage, TViewModel>(this IServiceCollection services) 
            where TPage : BasePage<TViewModel>, IView 
            where TViewModel : class, IViewModel
        {
            services.TryAddSingleton<INavigationLookup>(NavigationLookup);
            
            services.AddTransient<TPage>();
            services.AddTransient<TViewModel>();

            NavigationLookup.Register<TPage, TViewModel>();
            return services;
        }

        public static IServiceCollection AddContentView<TView>(this IServiceCollection services) 
            where TView : BaseContentView, IView 
        {
            services.AddTransient<TView>();
            return services;
        }
        
        public static IServiceCollection AddContentView<TView, TViewModel>(this IServiceCollection services) 
            where TView : BaseContentView, IView 
            where TViewModel : class, IViewModel
        {
            services.AddTransient<TView>();
            services.AddTransient<TViewModel>();
            return services;
        }
    }
}