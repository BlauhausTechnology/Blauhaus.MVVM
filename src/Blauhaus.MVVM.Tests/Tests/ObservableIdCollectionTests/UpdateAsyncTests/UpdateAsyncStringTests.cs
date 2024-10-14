using Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests.Base;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests
{
    [TestFixture]
    public class UpdateAsyncStringTests : BaseObservableIdCollectionUpdateTests<string>
    {
        public UpdateAsyncStringTests() : base(new []{"1", "2", "3"})
        {
        }
    }
}