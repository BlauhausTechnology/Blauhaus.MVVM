using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.NavigationCommands
{
    public class ShowAndInitializeViewCommand<TViewModel, TParameter> : AsyncExecutingCommand 
        where TViewModel : IViewModel, IAsyncInitializable<TParameter>
    {
        private Func<TParameter>? _parameterFunc;
        private string _navigationStack = string.Empty;

        public ShowAndInitializeViewCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler, 
            INavigationService navigationService) 
                : base(serviceLocator, errorHandler)
        {
            WithExecute(async () =>
            {
                if (_parameterFunc == null)
                {
                    throw new InvalidOperationException("ShowAndInitializeViewCommand must be initialized with a parameter using WithParameter before use");
                }
                await navigationService.ShowAndInitializeViewAsync<TViewModel, TParameter>(_parameterFunc.Invoke(), _navigationStack);
            });
        }

        public ShowAndInitializeViewCommand<TViewModel, TParameter> WithParameter(Func<TParameter> parameter)
        {
            _parameterFunc = parameter;
            return this;
        }
        
        
        public ShowAndInitializeViewCommand<TViewModel, TParameter> WithNavigationStack(string navigationStack)
        {
            _navigationStack = navigationStack;
            return this;
        }


       
 
    }
}