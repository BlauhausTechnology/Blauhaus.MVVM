using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.ExecutingCommands.Base;

namespace Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands.Base
{
    public abstract class BaseExecutingNoParameterCommand<TExecutingCommand> : BaseExecutingCommand<TExecutingCommand> 
        where TExecutingCommand : BaseExecutingNoParameterCommand<TExecutingCommand>
    {

        protected BaseExecutingNoParameterCommand(
            IServiceLocator serviceLocator,
            IErrorHandler errorHandler) 
            : base(serviceLocator, errorHandler)
        {
        }
         

        public override void Execute(object parameter) => Execute();

    }
}