using Blauhaus.MVVM.Collections;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.MVVM.Ioc
{
    public static class ServiceCollectionExtensions
    {
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
            services.AddTransient(typeof(ObservableGuidCollection<>));
            services.AddTransient(typeof(ObservableLongCollection<>));
            services.AddTransient(typeof(ObservableStringCollection<>));

            return services;
        }
    }
}