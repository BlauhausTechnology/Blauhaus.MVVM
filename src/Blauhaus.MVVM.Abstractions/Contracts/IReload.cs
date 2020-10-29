using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IReload
    {
        IExecutingCommand ReloadCommand { get; }
    }
}