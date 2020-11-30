using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface ISelectableViewModel
    {
        public interface ISelect
        {
            IExecutingCommand SelectCommand { get; }
        }
    }
}