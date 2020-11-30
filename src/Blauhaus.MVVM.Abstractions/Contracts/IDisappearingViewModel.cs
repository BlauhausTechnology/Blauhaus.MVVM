using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IDisappearingViewModel : IAppearingViewModel
    {
        IExecutingCommand DisappearCommand { get; }
    }
}