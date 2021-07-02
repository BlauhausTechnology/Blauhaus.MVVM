using System;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Services
{

    /// <summary>
    /// A static holder for the ServiceLocator to reference from classes that are not part of the Dependency Injection world, eg Android MainActivity
    /// </summary>
    public static class AppServiceLocator
    {
        private static IServiceLocator? _instance;

        public static IServiceLocator Instance
        {
            get
            {
                if (_func == null)
                {
                    throw new Exception("ServiceLocator instance has not been initialized");
                }

                return _instance ??= _func.Invoke();
            }
        }

        private static Func<IServiceLocator>? _func;

        public static void Initialize(Func<IServiceLocator> serviceLocatorFunc)
        {
            _func = serviceLocatorFunc;
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