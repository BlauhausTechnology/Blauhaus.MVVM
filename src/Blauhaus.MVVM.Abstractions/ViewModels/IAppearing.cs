using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IAppearing : IViewModel
    {
        IExecutingCommand AppearingCommand { get; }
    }
}