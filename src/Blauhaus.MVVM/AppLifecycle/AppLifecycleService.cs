using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.Responses;

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

                    if (_handlers.Count == 0)
                    {
                        return;
                    }

                    foreach (var appLifecycleHandler in _handlers)
                    {
                        var handlerResult = await appLifecycleHandler.HandleAppStateChangeAsync(state);
                        if (handlerResult.IsFailure)
                        {
                            _analyticsService.TraceWarning(this, "AppLifeCycle handler failed!");
                            await _errorHandler.HandleErrorAsync(handlerResult.Error);
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