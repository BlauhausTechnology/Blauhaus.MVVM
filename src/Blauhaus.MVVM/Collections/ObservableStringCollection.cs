using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableStringCollection<T> : ObservableIdCollection<T, string> where T : class, IHasId<string>, IAsyncInitializable<string>
    {
        public ObservableStringCollection(IServiceLocator serviceLocator, IThreadService threadService, IAnalyticsService analyticsService, IErrorHandler errorHandler) : base(serviceLocator, threadService, analyticsService, errorHandler)
        {
        }
    }
}