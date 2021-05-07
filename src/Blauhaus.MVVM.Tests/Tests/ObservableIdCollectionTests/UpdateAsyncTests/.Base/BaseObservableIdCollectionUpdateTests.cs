using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.UpdateAsyncTests.Base
{
    public abstract class BaseObservableIdCollectionUpdateTests<TId> : BaseObservableIdCollectionTest<TId>
    {
        protected BaseObservableIdCollectionUpdateTests(TId[] ids) : base(ids)
        {
        }

        [Test]
        public async Task WHEN_no_items_exist_already_SHOULD_create_and_initialize_and_add_one_for_each_inputid_in_order()
        {
            //Act
            await Sut.UpdateAsync(new[] {Ids[0], Ids[1], Ids[2]});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(3));
            Assert.That(Sut[0].Id, Is.EqualTo(Ids[0]));
            Assert.That(Sut[1].Id, Is.EqualTo(Ids[1]));
            Assert.That(Sut[2].Id, Is.EqualTo(Ids[2]));
            _outputObject1.Mock.Verify(x => x.InitializeAsync(Ids[0]), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync(Ids[1]), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync(Ids[2]), Times.Once);
        }

        
        [Test]
        public async Task WHEN_items_exist_already_SHOULD_only_add_new_ones_in_correct_place()
        {
            //Arrange
            MockServiceLocator.Mock.Setup(x => x.Resolve<IIdObject<TId>>()).Returns(new Queue<IIdObject<TId>>(new[]
            {
                _outputObject2.Object, _outputObject1.Object, _outputObject3.Object
            }).Dequeue);
        
            await Sut.UpdateAsync(new [] { Ids[1] });

            //Act
            await Sut.UpdateAsync(new [] {Ids[0], Ids[1], Ids[2]});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(3));
            Assert.That(Sut[0].Id, Is.EqualTo(Ids[0]));
            Assert.That(Sut[1].Id, Is.EqualTo(Ids[1]));
            Assert.That(Sut[2].Id, Is.EqualTo(Ids[2]));
            _outputObject1.Mock.Verify(x => x.InitializeAsync(Ids[0]), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync(Ids[1]), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync(Ids[2]), Times.Once);
            _outputObject1.Mock.Verify(x => x.ReloadAsync(), Times.Never);
            _outputObject2.Mock.Verify(x => x.ReloadAsync(), Times.Once);
            _outputObject3.Mock.Verify(x => x.ReloadAsync(), Times.Never);
        }

        [Test]
        public async Task WHEN_item_is_removed_SHOULD_remove_and_dispose_item()
        {
            //Arrange
            MockServiceLocator.Mock.Setup(x => x.Resolve<IIdObject<TId>>()).Returns(new Queue<IIdObject<TId>>(new[]
            {
                _outputObject2.Object, _outputObject1.Object, _outputObject3.Object
            }).Dequeue);
            await Sut.UpdateAsync(new [] { Ids[1] });
            await Sut.UpdateAsync(new [] {Ids[0], Ids[1], Ids[2]});

            //Act
            await Sut.UpdateAsync(new [] {Ids[0], Ids[2]});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(2));
            Assert.That(Sut[0].Id, Is.EqualTo(Ids[0]));
            Assert.That(Sut[1].Id, Is.EqualTo(Ids[2]));
            _outputObject1.Mock.Verify(x => x.InitializeAsync(Ids[0]), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync(Ids[1]), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync(Ids[2]), Times.Once);
            _outputObject1.Mock.Verify(x => x.DisposeAsync(), Times.Never);
            _outputObject2.Mock.Verify(x => x.DisposeAsync(), Times.Once);
            _outputObject3.Mock.Verify(x => x.DisposeAsync(), Times.Never);
        }

        
        [Test]
        public async Task WHEN_items_change_position_SHOULD_reorder()
        {
            //Arrange
            MockServiceLocator.Mock.Setup(x => x.Resolve<IIdObject<TId>>()).Returns(new Queue<IIdObject<TId>>(new[]
            {
                _outputObject2.Object, _outputObject1.Object, _outputObject3.Object
            }).Dequeue);
            await Sut.UpdateAsync(new [] {Ids[1], Ids[0], Ids[2]});

            //Act
            await Sut.UpdateAsync(new [] {Ids[2], Ids[0], Ids[1]});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(3));
            Assert.That(Sut[0].Id, Is.EqualTo(Ids[2]));
            Assert.That(Sut[1].Id, Is.EqualTo(Ids[0]));
            Assert.That(Sut[2].Id, Is.EqualTo(Ids[1]));
            _outputObject1.Mock.Verify(x => x.InitializeAsync(Ids[0]), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync(Ids[1]), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync(Ids[2]), Times.Once);
        }

    }
}