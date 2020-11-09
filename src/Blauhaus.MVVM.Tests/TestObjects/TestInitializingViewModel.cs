using System;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestInitializingViewModel : BaseBindableObject, IViewModel, IInitialize<Guid>
    {

        public Guid InitializedValue { get; private set; }
         
        public Task InitializeAsync(Guid initialValue)
        {
            InitializedValue = initialValue;
            return Task.CompletedTask;
        }
    }
}