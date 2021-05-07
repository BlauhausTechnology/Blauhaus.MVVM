using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableStringCollection<T> : ObservableIdCollection<T, string> where T : class, IHasId<string>, IAsyncInitializable<string>
    {
        public ObservableStringCollection(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}