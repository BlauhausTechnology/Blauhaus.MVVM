using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestAppearingViewModel : BaseViewModel, IAppearing
    {

        public TestAppearingViewModel(
            IExecutingCommand appearingCommand, 
            ICommand disappearingCommand)
        {
            AppearingCommand = appearingCommand;
            DisappearingCommand = disappearingCommand;
        }


        public IExecutingCommand AppearingCommand { get; }
        public ICommand DisappearingCommand { get; }
    }
}