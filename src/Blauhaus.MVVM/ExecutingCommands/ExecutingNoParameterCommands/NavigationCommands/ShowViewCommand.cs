using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands
{
    public class ShowViewCommand<TViewModel> : AsyncExecutingCommand 
        where TViewModel : IViewModel
    {
        private string _navigationStack = string.Empty;

        public ShowViewCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            INavigationService navigationService) 
                : base(serviceLocator, errorHandler, analyticsService)
        {
            WithExecute(async () =>
            {
                await navigationService.ShowViewAsync<TViewModel>(_navigationStack);
            });
        }

        public ShowViewCommand<TViewModel> WithNavigationStack(string navigationStack)
        {
            _navigationStack = navigationStack;
            return this;
        }
    }
}