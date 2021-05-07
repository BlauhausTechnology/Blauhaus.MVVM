using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestAsyncInitializableViewModel : BaseViewModel, IAsyncInitializable
    {
        
        public bool IsInitialized { get; private set; }
        
        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }
    }
}