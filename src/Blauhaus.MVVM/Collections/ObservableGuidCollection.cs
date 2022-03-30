using System;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Collections.Base;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableGuidCollection<T> : BaseObservableIdCollection<T, Guid> where T : class, IHasId<Guid>, IAsyncInitializable<Guid>
    {
        public ObservableGuidCollection(
            IAnalyticsLogger<ObservableGuidCollection<T>> logger,
            IServiceLocator serviceLocator, 
            IThreadService threadService, 
            IErrorHandler errorHandler) 
                : base(logger, serviceLocator, threadService, errorHandler)
        {
        }
    }
}