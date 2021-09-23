using Blauhaus.Domain.Abstractions.Sync;
using Blauhaus.Domain.Abstractions.Sync.Old;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestSyncCommand : SyncCommand
    {
        public string FavouriteColour { get; set; }
    }
}