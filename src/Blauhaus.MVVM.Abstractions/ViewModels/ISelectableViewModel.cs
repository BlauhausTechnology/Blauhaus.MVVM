using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface ISelectableViewModel
    {
        IExecutingCommand SelectCommand { get; }
    }
}