using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IDisappear : IAppear
    {
        IExecutingCommand DisappearCommand { get; }
    }
}