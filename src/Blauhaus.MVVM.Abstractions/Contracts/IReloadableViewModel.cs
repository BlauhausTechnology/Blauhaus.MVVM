using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IReloadableViewModel
    {
        IExecutingCommand ReloadCommand { get; }
    }
}