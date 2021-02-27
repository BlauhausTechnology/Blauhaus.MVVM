using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.MVVM.Abstractions.Application;

namespace Blauhaus.MVVM.AppLifecycle
{
    public class AppLifecycleService : IAppLifecycleService
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IEnumerable<IAppLifecycleHandler> _handlers;

        public AppLifecycleService(
            IAnalyticsService analyticsService,
            IEnumerable<IAppLifecycleHandler> handlers)
        {
            _analyticsService = analyticsService;
            _handlers = handlers;
        }
 

        public void NotifyAppStarting()
        {
            AppStarting?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyAppGoingToSleep()
        {
            AppGoingToSleep?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyAppWakingUp()
        {
            AppWakingUp?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? AppStarting;
        public event EventHandler? AppGoingToSleep;
        public event EventHandler? AppWakingUp;
    }
}