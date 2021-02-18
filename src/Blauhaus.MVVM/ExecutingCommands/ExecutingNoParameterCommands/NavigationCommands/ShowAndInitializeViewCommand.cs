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
        private Func<TParameter>? _parameterFunc;

        public ShowAndInitializeViewCommand(
            IErrorHandler errorHandler, 
            IAnalyticsService analyticsService,
            INavigationService navigationService) 
                : base(errorHandler, analyticsService)
        {
            WithExecute(async () =>
            {
                if (_parameterFunc == null)
                {
                    throw new InvalidOperationException("ShowAndInitializeViewCommand must be initialized with a parameter using WithParameter before use");
                }
                await navigationService.ShowAndInitializeViewAsync<TViewModel, TParameter>(_parameterFunc.Invoke());
            });
        }

        public ShowAndInitializeViewCommand<TViewModel, TParameter> WithParameter(Func<TParameter> parameter)
        {
            _parameterFunc = parameter;
            return this;
        }
       
 
    }
}