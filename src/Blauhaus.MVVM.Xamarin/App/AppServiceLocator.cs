using System;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Xamarin.App
{

    /// <summary>
    /// A static holder for the ServiceLocator to reference from classes that are not part of the Dependency Injection world, eg Android MainActivity
    /// </summary>
    public static class AppServiceLocator
    {
        public static IServiceLocator? Instance;

        public static void Initialize(IServiceLocator serviceLocator)
        {
            Instance = serviceLocator;
        }

        public static T Resolve<T>() where T : class
        {
            if (Instance == null)
            {
                throw new Exception("ServiceLocator instance has not been initialized");
            }

            var service = Instance.Resolve<T>();
            if (service == null)
            {
                throw new Exception($"The type {typeof(T)} is not registered with the IOC container");
            }

            return service;

        }
    }
}