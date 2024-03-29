﻿using Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests.Base;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests
{

    [TestFixture]
    public class UpdateAsyncLongTests : BaseObservableIdCollectionUpdateTests<long>
    {
        public UpdateAsyncLongTests() : base(new long []{1,2,3})
        {
        }
    }
}