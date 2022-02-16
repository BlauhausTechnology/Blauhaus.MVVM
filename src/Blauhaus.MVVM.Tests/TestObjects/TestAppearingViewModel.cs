using System.Windows.Input;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestAppearingViewModel : BaseBindableObject, IAppearingViewModel
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