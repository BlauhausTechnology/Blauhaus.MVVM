using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableLongCollection<T> : ObservableIdCollection<T, long> where T : class, IHasId<long>, IAsyncInitializable<long>
    {
        public ObservableLongCollection(IServiceLocator serviceLocator, IThreadService threadService, IAnalyticsService analyticsService, IErrorHandler errorHandler) : base(serviceLocator, threadService, analyticsService, errorHandler)
        {
        }
    }
}