using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableStringCollection<T> : ObservableIdCollection<T, string> where T : class, IHasId<string>, IAsyncInitializable<string>
    {
        public ObservableStringCollection(IServiceLocator serviceLocator, IThreadService threadService) : base(serviceLocator, threadService)
        {
        }
    }
}