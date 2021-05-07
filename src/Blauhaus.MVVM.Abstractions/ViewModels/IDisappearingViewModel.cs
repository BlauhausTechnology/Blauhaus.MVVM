using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IDisappearingViewModel
    {
        IExecutingCommand DisappearCommand { get; }
    }
}