using System;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.MVVM.Xamarin.App
{

    /// <summary>
    /// A static holder for the ServiceLocator to reference from classes that are not part of the Dependency Injection world, eg Android MainActivity
    /// </summary>
    public static class AppServiceLocator
    {
        public static IServiceProvider Instance;

        public static void SetProvider(IServiceProvider serviceProvider)
        {
            Instance = serviceProvider;
        }

        public static T Resolve<T>()
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