using Blauhaus.TestHelpers.BaseTests;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ViewTests._Base
{
    public abstract class BaseViewTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
        }
    }
}