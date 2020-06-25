using System;
using Blauhaus.MVVM.Abstractions.Bindable;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestInitializingViewModel : BaseBindableObject, IViewModel, IInitializing<Guid>
    {

        public Guid InitializedValue { get; private set; }

        public void Initialize(Guid initialValue)
        {
            InitializedValue = initialValue;
        }
    }
}