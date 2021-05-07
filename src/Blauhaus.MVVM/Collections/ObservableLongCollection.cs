using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableLongCollection<T> : ObservableIdCollection<T, long> where T : class, IHasId<long>, IAsyncInitializable<long>
    {
        public ObservableLongCollection(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}