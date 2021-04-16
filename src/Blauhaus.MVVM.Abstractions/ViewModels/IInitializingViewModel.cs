using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IInitializingViewModel
    {
        public IExecutingCommand InitializeCommand { get; }
    }
}