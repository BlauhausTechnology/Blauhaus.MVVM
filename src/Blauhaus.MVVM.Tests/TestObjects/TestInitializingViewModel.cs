using System;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestInitializingViewModel : BaseBindableObject, IViewModel, IInitialize<Guid>
    {

        public Guid InitializedValue { get; private set; }

        public void Initialize(Guid initialValue)
        {
            InitializedValue = initialValue;
        }
    }
}