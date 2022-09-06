using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.Responses;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.AppLifecycle
{
    public class AppLifecycleService : IAppLifecycleService
    {
        private readonly IAnalyticsLogger _logger;
        private readonly IErrorHandler _errorHandler;
        private readonly IReadOnlyList<IAppLifecycleHandler> _handlers;

        public AppLifecycleService(
            IAnalyticsLogger<AppLifecycleService> logger,
            IEnumerable<IAppLifecycleHandler> handlers,
            IErrorHandler errorHandler)
        {
            _logger = logger;
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

        private async void HandleStateChange(AppLifecycleState state, Action eventHandler)
        {
            try
            {
                eventHandler.Invoke();
                
                _logger.LogTrace("App state changing to {AppState}", state);

                if (_handlers.Count == 0)
                {
                    return;
                }
                
                _logger.LogTrace("Notifying IAppLifecycleHandlers: {HandlerCount}", _handlers.Count);

                foreach (var appLifecycleHandler in _handlers)
                {
                    var handlerResult = await appLifecycleHandler.HandleAppStateChangeAsync(state);
                    if (handlerResult.IsFailure)
                    {
                        _logger.LogWarning("AppLifeCycle handler failed! {Error}", handlerResult.Error.ToString());
                        await _errorHandler.HandleErrorAsync(handlerResult.Error);
                    }
                } 
            }
            catch (Exception e)
            {
                await _errorHandler.HandleExceptionAsync(this, e);
            }
        }

        public event EventHandler? AppStarting;
        public event EventHandler? AppGoingToSleep;
        public event EventHandler? AppWakingUp;
    }
}