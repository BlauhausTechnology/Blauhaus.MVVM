using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IInitializableViewModel
    {
        public IExecutingCommand InitializeCommand { get; }
    }
}