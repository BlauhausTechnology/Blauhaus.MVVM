using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public abstract class BaseViewModel : BaseBindableObject, IViewModel
    {
        protected readonly IServiceLocator ServiceLocator;
        protected readonly IAnalyticsService AnalyticsService;
        protected readonly IErrorHandler ErrorHandler;
        protected readonly INavigationService NavigationService;
        
        protected T Resolve<T>() where T : class => ServiceLocator.Resolve<T>();

        protected BaseViewModel(
            IServiceLocator serviceLocator, 
            IAnalyticsService analyticsService, 
            IErrorHandler errorHandler, 
            INavigationService navigationService)
        {
            ServiceLocator = serviceLocator;
            AnalyticsService = analyticsService;
            ErrorHandler = errorHandler;
            NavigationService = navigationService;
        }
    }
}