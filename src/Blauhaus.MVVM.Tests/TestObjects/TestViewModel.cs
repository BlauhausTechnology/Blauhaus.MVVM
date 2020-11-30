using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestViewModel : BaseBindableObject, IViewModel, IAppearingViewModel
    {

        public IExecutingCommand AppearCommand { get; }
    }
}