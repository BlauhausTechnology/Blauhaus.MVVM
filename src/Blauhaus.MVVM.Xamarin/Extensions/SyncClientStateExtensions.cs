using Blauhaus.Domain.Abstractions.Sync;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class SyncClientStateExtensions
    {
        public static bool IsExecuting(this SyncClientState state)
        {
            return state == SyncClientState.DownloadingNew
                   || state == SyncClientState.LoadingLocal
                   || state == SyncClientState.DownloadingOld;
        }
    }
}