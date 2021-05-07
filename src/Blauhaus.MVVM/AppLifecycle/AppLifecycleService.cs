using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Application;

namespace Blauhaus.MVVM.AppLifecycle
{
    public class AppLifecycleService : IAppLifecycleService
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IErrorHandler _errorHandler;
        private readonly IReadOnlyList<IAppLifecycleHandler> _handlers;

        public AppLifecycleService(
            IAnalyticsService analyticsService,
            IEnumerable<IAppLifecycleHandler> handlers,
            IErrorHandler errorHandler)
        {
            _analyticsService = analyticsService;
            _errorHandler = errorHandler;
            _handlers = handlers.ToList();
        }
 

        public void NotifyAppStarting()
        {
            HandleStateChange(AppLifecycleState.Starting, () => AppStarting?.Invoke(this, EventArgs.Empty));
        }

        public void NotifyAppGoingToSleep()
        {
            HandleStateChange(AppLifecycleState.GoingToSleep, () => AppGoingToSleep?.Invoke(this, EventArgs.Empty));
        }

        public void NotifyAppWakingUp()
        {
            HandleStateChange(AppLifecycleState.WakingUp, () => AppWakingUp?.Invoke(this, EventArgs.Empty));
        }

        private void HandleStateChange(AppLifecycleState state, Action eventHandler)
        {
            Task.Run(async () =>
            {
                try
                {
                    eventHandler.Invoke();
                    
                    _analyticsService.Trace(this, $"App {state}. Notifying handlers: {_handlers.Count}");
                    
                    foreach (var appLifecycleHandler in _handlers)
                    {
                        var response = await appLifecycleHandler.HandleAppStateChangeAsync(state);
                        if (response.IsFailure)
                        {
                            await _errorHandler.HandleErrorAsync(response.Error);
                        }
                    }
                }
                catch (Exception e)
                {
                    await _errorHandler.HandleExceptionAsync(this, e);
                }
            });
        }

        public event EventHandler? AppStarting;
        public event EventHandler? AppGoingToSleep;
        public event EventHandler? AppWakingUp;
    }
}