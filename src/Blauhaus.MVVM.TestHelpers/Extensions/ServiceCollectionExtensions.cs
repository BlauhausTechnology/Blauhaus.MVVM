using System;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.MVVM.TestHelpers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestObservableGuidCollection<T>(this IServiceCollection services) where T : class, IHasId<Guid>, IAsyncInitializable<Guid>
    {
        services.TryAddSingleton(new AnalyticsLoggerMockBuilder<ObservableGuidCollection<T>>().Object);
        services.TryAddSingleton(new AnalyticsLoggerMockBuilder<T>().Object);
        services.AddTransient<T>();
        return services;
    }
    
    public static IServiceCollection AddTestObservableLongCollection<T>(this IServiceCollection services) where T : class, IHasId<long>, IAsyncInitializable<long>
    {
        services.TryAddSingleton(new AnalyticsLoggerMockBuilder<ObservableLongCollection<T>>().Object);
        services.TryAddSingleton(new AnalyticsLoggerMockBuilder<T>().Object);
        services.AddTransient<T>();
        return services;
    }
    
    public static IServiceCollection AddTestObservableStringCollection<T>(this IServiceCollection services) where T : class, IHasId<string>, IAsyncInitializable<string>
    {
        services.TryAddSingleton(new AnalyticsLoggerMockBuilder<ObservableStringCollection<T>>().Object);
        services.TryAddSingleton(new AnalyticsLoggerMockBuilder<T>().Object);
        services.AddTransient<T>();
        return services;
    }
}