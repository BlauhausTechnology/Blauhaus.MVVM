using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IDisappearingViewModel : IAppearingViewModel
    {
        IExecutingCommand DisappearCommand { get; }
    }
}