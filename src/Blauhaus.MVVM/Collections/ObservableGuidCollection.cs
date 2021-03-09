using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableGuidCollection<T> : ObservableIdCollection<T, Guid> where T : class, IHasId<Guid>, IAsyncInitializable<Guid>
    {
        public ObservableGuidCollection(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}