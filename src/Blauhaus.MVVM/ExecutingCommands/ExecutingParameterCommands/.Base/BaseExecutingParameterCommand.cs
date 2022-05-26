using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.ExecutingCommands.Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands.Base
{
    public abstract class BaseExecutingParameterCommand<TExecutingCommand, TParameter> : BaseExecutingCommand<TExecutingCommand> 
        where TExecutingCommand : BaseExecutingParameterCommand<TExecutingCommand, TParameter>
    {

        protected BaseExecutingParameterCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler) 
                : base(serviceLocator, errorHandler)
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