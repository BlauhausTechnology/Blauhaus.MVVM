using System.Windows.Input;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IDisappear : IAppear
    {
        ICommand DisappearCommand { get; }
    }
}