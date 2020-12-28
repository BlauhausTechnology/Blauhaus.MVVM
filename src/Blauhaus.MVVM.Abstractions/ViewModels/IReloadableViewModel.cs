using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IReloadableViewModel
    {
        IExecutingCommand ReloadCommand { get; }
    }
}