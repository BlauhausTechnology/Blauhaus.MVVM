using System;
using Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests.Base;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests
{
    [TestFixture]
    public class UpdateAsyncGuidTests : BaseObservableIdCollectionUpdateTests<Guid>
    {
        public UpdateAsyncGuidTests() : base(new [] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() })
        {
        }
    }
}