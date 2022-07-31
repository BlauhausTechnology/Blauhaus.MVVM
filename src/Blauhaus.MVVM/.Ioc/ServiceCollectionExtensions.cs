using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Abstractions.Navigation.Register;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Collections;
using Blauhaus.MVVM.Collections.Base;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands;
using Blauhaus.MVVM.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.MVVM.Ioc
{
    public static class ServiceCollectionExtensions
    {

        private static readonly ViewRegister ViewRegister = new();

        public static IServiceCollection AddAppLifecycleStateHandler<THandler>(this IServiceCollection services) where THandler : class, IAppLifecycleHandler
        {
            services.AddSingleton<IAppLifecycleHandler, THandler>();
            return services;
        }

        public static IServiceCollection AddNavigator(this IServiceCollection services)
        {
            services
                .AddSingleton<INavigator, Navigator>()
                .AddSingleton<IViewRegister>(_=> ViewRegister);
            return services;
        }
        
        public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, ViewIdentifier viewIdentifier) 
            where TView : class, IView<TViewModel>
            where TViewModel : class, IViewModel
        {
            services.AddTransient<TView>();
            services.AddTransient<TViewModel>();
            ViewRegister.RegisterView<TView>(viewIdentifier);

            return services;
        }


        public static IServiceCollection AddExecutingCommands(this IServiceCollection services)
        {
            services.AddTransient<AsyncExecutingCommand>();
            services.AddTransient<AsyncExecutingResponseCommand>();
            services.AddTransient(typeof(AsyncExecutingValueResponseCommand<>));
            services.AddTransient<ExecutingCommand>();

            services.AddTransient(typeof(ExecutingObservableCommand<>));

            services.AddTransient(typeof(AsyncExecutingParameterCommand<>));
            services.AddTransient(typeof(AsyncExecutingResponseParameterCommand<>));
            services.AddTransient(typeof(AsyncExecutingValueResponseParameterCommand<,>));
            services.AddTransient(typeof(ExecutingParameterCommand<>));

            return services;
        }
        
        public static IServiceCollection AddNavigationCommands(this IServiceCollection services)
        {
            services.AddTransient(typeof(ShowAndInitializeViewCommand<,>));
            services.AddTransient(typeof(ShowViewCommand<>));
            services.AddTransient(typeof(ShowMainViewCommand<>));

            return services;
        }

        public static IServiceCollection AddObservableIdCollections(this IServiceCollection services)
        {
            services.AddTransient(typeof(ObservableIdCollection<,>));
            services.AddTransient(typeof(ObservableGuidCollection<>));
            services.AddTransient(typeof(ObservableLongCollection<>));
            services.AddTransient(typeof(ObservableStringCollection<>));

            return services;
        }
    }
}