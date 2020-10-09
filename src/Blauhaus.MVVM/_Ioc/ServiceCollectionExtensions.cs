﻿using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.MVVM._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExecutingCommands(this IServiceCollection services)
        {
            services.AddTransient<AsyncExecutingCommand>();
            services.AddTransient<AsyncExecutingResultCommand>();
            services.AddTransient<AsyncExecutingResponseCommand>();
            services.AddTransient(typeof(AsyncExecutingValueResultCommand<>));
            services.AddTransient(typeof(AsyncExecutingValueResponseCommand<>));
            services.AddTransient<ExecutingCommand>();

            services.AddTransient(typeof(ExecutingObservableCommand<>));

            services.AddTransient(typeof(AsyncExecutingParameterCommand<>));
            services.AddTransient(typeof(AsyncExecutingResultParameterCommand<>));
            services.AddTransient(typeof(AsyncExecutingResponseParameterCommand<>));
            services.AddTransient(typeof(AsyncExecutingValueResultParameterCommand<,>));
            services.AddTransient(typeof(AsyncExecutingValueResponseParameterCommand<,>));
            services.AddTransient(typeof(ExecutingParameterCommand<>));

            return services;
        }
    }
}