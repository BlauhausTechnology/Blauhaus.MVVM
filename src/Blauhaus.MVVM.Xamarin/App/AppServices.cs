using System;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.MVVM.Xamarin.App
{
    public static class AppServices
    {
        public static IServiceProvider Instance;

        public static void SetProvider(IServiceProvider serviceProvider)
        {
            Instance = serviceProvider;
        }

        public static T GetService<T>()
        {
            if (Instance == null)
            {
                throw new Exception("Service Provider has not been initialized");
            }

            var service = Instance.GetService<T>();
            if (service == null)
            {
                throw new Exception($"The type {typeof(T)} is not registered with the IOC container");
            }

            return service;

        }
    }
}