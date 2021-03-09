using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestViewModel : BaseBindableObject, IViewModel, IAppearingViewModel
    {

        public IExecutingCommand AppearCommand { get; }
    }
}