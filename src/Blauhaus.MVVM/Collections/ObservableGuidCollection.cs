using System;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableGuidCollection<T> : ObservableIdCollection<T, Guid> where T : class, IId<Guid>, IInitialize<Guid>
    {
        public ObservableGuidCollection(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}