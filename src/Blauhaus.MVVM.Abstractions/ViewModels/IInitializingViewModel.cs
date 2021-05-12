using System;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    [Obsolete("Use IAsyncInitializable instead")]
    public interface IInitializingViewModel
    {
        public IExecutingCommand InitializeCommand { get; }
    }
}