using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Collections.Base;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableLongCollection<T> : BaseObservableIdCollection<T, long> where T : class, IHasId<long>, IAsyncInitializable<long>
    {
        public ObservableLongCollection(
            IAnalyticsLogger<ObservableLongCollection<T>> logger,
            IServiceLocator serviceLocator, 
            IThreadService threadService, 
            IErrorHandler errorHandler) 
                : base(logger, serviceLocator, threadService, errorHandler)
        {
        }
    }
}