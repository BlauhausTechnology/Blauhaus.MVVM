using System.Windows.Input;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IDisappearing : IAppearing
    {
        ICommand DisappearingCommand { get; }
    }
}