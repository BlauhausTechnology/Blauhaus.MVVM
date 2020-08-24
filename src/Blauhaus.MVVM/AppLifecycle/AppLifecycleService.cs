using System;
using Blauhaus.MVVM.Abstractions.Application;

namespace Blauhaus.MVVM.AppLifecycle
{
    public class AppLifecycleService : IAppLifecycleService
    { 

 

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