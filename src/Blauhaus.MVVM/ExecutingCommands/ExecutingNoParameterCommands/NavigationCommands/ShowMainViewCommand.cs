using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands
{
    public class ShowMainViewCommand<TViewModel> : AsyncExecutingCommand 
        where TViewModel : class, IViewModel
    {
        public ShowMainViewCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler, 
            INavigationService navigationService) 
                : base(serviceLocator, errorHandler)
        {
            WithExecute(async () =>
            {
                await navigationService.ShowMainViewAsync<TViewModel>();
            });
        }
  
    }
}