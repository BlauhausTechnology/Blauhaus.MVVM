using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IAppearingViewModel : IViewModel
    {
        IExecutingCommand AppearCommand { get; }
    }
}