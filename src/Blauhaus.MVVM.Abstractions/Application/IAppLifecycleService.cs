using System;

namespace Blauhaus.MVVM.Abstractions.Application
{
    public interface IAppLifecycleService
    {
         
        event EventHandler AppStarting;
        event EventHandler AppGoingToSleep;
        event EventHandler AppWakingUp;
    }
}