using System;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableGuidCollection<T> : ObservableIdCollection<T, Guid> where T : class, IHasId<Guid>, IAsyncInitializable<Guid>
    {
        public ObservableGuidCollection(IServiceLocator serviceLocator, IThreadService threadService, IAnalyticsService analyticsService, IErrorHandler errorHandler) : base(serviceLocator, threadService, analyticsService, errorHandler)
        {
        }
    }
}