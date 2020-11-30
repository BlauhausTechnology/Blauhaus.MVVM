using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IAppearingViewModel : IViewModel
    {
        IExecutingCommand AppearCommand { get; }
    }
}