﻿using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.Utils.NotifyPropertyChanged;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestInitializingViewModel : BaseBindableObject, IViewModel, IAsyncInitializable<Guid>
    {

        public Guid InitializedValue { get; private set; }
         
        public Task InitializeAsync(Guid initialValue)
        {
            InitializedValue = initialValue;
            return Task.CompletedTask;
        }
    }
}