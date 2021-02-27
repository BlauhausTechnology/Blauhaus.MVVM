using Blauhaus.MVVM.Abstractions.Contracts;

namespace Blauhaus.MVVM.TestHelpers.NotifyUpdates
{
    public static class NotifyUpdateExtensions
    { 
        public static UpdateChanges<TUpdate> SubscribeToUpdates<TUpdate>(this INotifyUpdates notifyUpdates) 
        {
            return new UpdateChanges<TUpdate> (notifyUpdates);
        }
    }
}