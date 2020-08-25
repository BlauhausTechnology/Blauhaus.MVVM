using Blauhaus.Domain.Abstractions.CommandHandlers.Sync;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestSyncCommand : SyncCommand
    {
        public string FavouriteColour { get; set; }
    }
}