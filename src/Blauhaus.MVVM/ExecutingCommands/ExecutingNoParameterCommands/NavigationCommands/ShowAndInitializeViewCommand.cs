using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands
{
    public class ShowAndInitializeViewCommand<TViewModel, TParameter> : AsyncExecutingCommand 
        where TViewModel : IViewModel, IAsyncInitializable<TParameter>
    {
        private TParameter _parameter;
        private bool _isInitialized;

        public ShowAndInitializeViewCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            INavigationService navigationService) 
                : base(errorHandler, analyticsService)
        {
            WithExecute(async () =>
            {
                if (!_isInitialized)
                {
                    throw new InvalidOperationException("ShowAndInitializeViewCommand must be initialized with a parameter using WithParameter before use");
                }
                await navigationService.ShowAndInitializeViewAsync<TViewModel, TParameter>(_parameter!);
            });
        }

        public ShowAndInitializeViewCommand<TViewModel, TParameter> WithParameter(Func<TParameter> parameter)
        {
            _isInitialized = true;
            _parameter = parameter.Invoke();
            return this;
        }
        
        public ShowAndInitializeViewCommand<TViewModel, TParameter> WithParameter(TParameter parameter)
        {
            _isInitialized = true;
            _parameter = parameter;
            return this;
        }
 
    }
}