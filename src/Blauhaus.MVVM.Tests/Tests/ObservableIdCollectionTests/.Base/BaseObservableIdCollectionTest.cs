using System;
using System.Collections.Generic;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Collections;
using Blauhaus.MVVM.Tests.Tests.Base;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.Base
{
    public interface IIdObject<TId> : IHasId<TId>, IAsyncInitializable<TId>, IAsyncReloadable, IAsyncDisposable
    {
    }

    public class BaseObservableIdCollectionTest<TId>: BaseMvvmTest<ObservableIdCollection<IIdObject<TId>, TId>>
    {
        protected readonly TId[] Ids;

        protected MockBuilder<IIdObject<TId>> _outputObject1 = null!;
        protected MockBuilder<IIdObject<TId>> _outputObject2 = null!;
        protected MockBuilder<IIdObject<TId>> _outputObject3 = null!;

        public BaseObservableIdCollectionTest(TId[] ids)
        {
            Ids = ids;
        }
        protected MockBuilder<IServiceLocator> MockServiceLocator => AddMock<IServiceLocator>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockServiceLocator.Object);

            _outputObject1 = new MockBuilder<IIdObject<TId>>().With(x => x.Id, Ids[0]);
            _outputObject2 = new MockBuilder<IIdObject<TId>>().With(x => x.Id, Ids[1]);
            _outputObject3 = new MockBuilder<IIdObject<TId>>().With(x => x.Id, Ids[2]);
             
            MockServiceLocator.Mock.Setup(x => x.Resolve<IIdObject<TId>>()).Returns(new Queue<IIdObject<TId>>(new[]
            {
                _outputObject1.Object, 
                _outputObject2.Object, 
                _outputObject3.Object
            }).Dequeue);
        }
    }
}