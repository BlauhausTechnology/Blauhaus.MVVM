using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestAppearingViewModel : BaseViewModel, IAppearingViewModel
    {

        public TestAppearingViewModel(
            IExecutingCommand appearingCommand, 
            ICommand disappearingCommand)
        {
            AppearCommand = appearingCommand;
            DisappearingCommand = disappearingCommand;
        }


        public IExecutingCommand AppearCommand { get; }
        public ICommand DisappearingCommand { get; }
    }
}