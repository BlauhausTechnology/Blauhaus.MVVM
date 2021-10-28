using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands
{
    public class ShowMainViewCommand<TViewModel> : AsyncExecutingCommand 
        where TViewModel : class, IViewModel
    {
        public ShowMainViewCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            INavigationService navigationService) 
                : base(errorHandler, analyticsService)
        {
            WithExecute(async () =>
            {
                await navigationService.ShowMainViewAsync<TViewModel>();
            });
        }
  
    }
}