using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface INavigatingBackViewModel
    {
        public IExecutingCommand PopFromStackCommand { get; }
    }
}