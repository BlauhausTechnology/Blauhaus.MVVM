using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.ExecutingCommands._Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands._Base
{
    public abstract class BaseExecutingParameterCommand<TExecutingCommand, TParameter> : BaseExecutingCommand<TExecutingCommand> 
        where TExecutingCommand : BaseExecutingParameterCommand<TExecutingCommand, TParameter>
    {

        protected BaseExecutingParameterCommand(
            IErrorHandler errorHandler,
            IAnalyticsService analyticsService) 
                : base(errorHandler, analyticsService)
        {
        }
         

        public abstract override void Execute(object parameter);

        protected TParameter ConvertParameter(object parameter)
        {
            if (parameter == null)
            {
                return default;
            }

            if (!(parameter is TParameter value))
            {
                throw new Exception($"Expected {typeof(TParameter).Name} parameter but got {parameter.GetType().Name}");
            }

            return value;
        }
         
    }
}