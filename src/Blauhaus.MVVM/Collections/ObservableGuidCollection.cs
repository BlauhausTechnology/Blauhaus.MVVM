using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableGuidCollection<T> : ObservableIdCollection<T, Guid> where T : class, IHasId<Guid>, IAsyncInitializable<Guid>
    {
        public ObservableGuidCollection(IServiceLocator serviceLocator, IThreadService threadService) : base(serviceLocator, threadService)
        {
        }
    }
}