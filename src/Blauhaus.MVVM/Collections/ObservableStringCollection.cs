using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Collections.Base;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableStringCollection<T> : BaseObservableIdCollection<T, string> where T : class, IHasId<string>, IAsyncInitializable<string>
    {
        public ObservableStringCollection(
            IAnalyticsLogger<ObservableStringCollection<T>> logger,
            IServiceLocator serviceLocator,
            IThreadService threadService, 
            IErrorHandler errorHandler) 
                : base(logger, serviceLocator, threadService, errorHandler)
        {
        }
    }
}