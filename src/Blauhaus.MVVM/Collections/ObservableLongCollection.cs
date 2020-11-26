using System;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableLongCollection<T> : ObservableIdCollection<T, long> where T : class, IId<long>, IInitialize<long>
    {
        public ObservableLongCollection(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}