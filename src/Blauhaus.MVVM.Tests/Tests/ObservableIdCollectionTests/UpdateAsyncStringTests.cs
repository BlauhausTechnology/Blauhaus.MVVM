using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Collections;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests
{

   
    public class UpdateAsyncStringTests : BaseMvvmTest<ObservableIdCollection<UpdateAsyncStringTests.IOutputObject, string>>
    {
        public interface IOutputObject : IId<string>, IInitialize<string>
        {
        } 


        private MockBuilder<IOutputObject> _outputObject1;
        private MockBuilder<IOutputObject> _outputObject2;
        private MockBuilder<IOutputObject> _outputObject3;

        protected MockBuilder<IServiceLocator> MockServiceLocator => AddMock<IServiceLocator>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockServiceLocator.Object);

            _outputObject1 = new MockBuilder<IOutputObject>().With(x => x.Id, "1");
            _outputObject2 = new MockBuilder<IOutputObject>().With(x => x.Id, "2");
            _outputObject3 = new MockBuilder<IOutputObject>().With(x => x.Id, "3");
             
            MockServiceLocator.Mock.Setup(x => x.Resolve<IOutputObject>()).Returns(new Queue<IOutputObject>(new[] { _outputObject1.Object, _outputObject2.Object, _outputObject3.Object }).Dequeue);
        }

        [Test]
        public async Task WHEN_no_items_exist_already_SHOULD_create_and_initialize_and_add_one_for_each_inputid_in_order()
        {
            //Act
            await Sut.UpdateAsync(new string[] {"1", "2", "3"});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(3));
            Assert.That(Sut[0].Id, Is.EqualTo("1"));
            Assert.That(Sut[1].Id, Is.EqualTo("2"));
            Assert.That(Sut[2].Id, Is.EqualTo("3"));
            _outputObject1.Mock.Verify(x => x.InitializeAsync("1"), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync("2"), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync("3"), Times.Once);
        }

        [Test]
        public async Task WHEN_items_exist_already_SHOULD_only_add_new_ones_in_correct_place()
        {
            //Arrange
            
            MockServiceLocator.Mock.Setup(x => x.Resolve<IOutputObject>()).Returns(new Queue<IOutputObject>(new[]
            {
                _outputObject2.Object, _outputObject1.Object, _outputObject3.Object
            }).Dequeue);
        
            await Sut.UpdateAsync(new string[] { "2" });

            //Act
            await Sut.UpdateAsync(new string[] {"1", "2", "3"});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(3));
            Assert.That(Sut[0].Id, Is.EqualTo("1"));
            Assert.That(Sut[1].Id, Is.EqualTo("2"));
            Assert.That(Sut[2].Id, Is.EqualTo("3"));
            _outputObject1.Mock.Verify(x => x.InitializeAsync("1"), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync("2"), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync("3"), Times.Once);
        }

        [Test]
        public async Task WHEN_item_is_removed_SHOULD_remove_item()
        {
            //Arrange
            
            MockServiceLocator.Mock.Setup(x => x.Resolve<IOutputObject>()).Returns(new Queue<IOutputObject>(new[]
            {
                _outputObject2.Object, _outputObject1.Object, _outputObject3.Object
            }).Dequeue);
        
            await Sut.UpdateAsync(new string[] {"2"});
            await Sut.UpdateAsync(new string[] {"1", "2", "3"});

            //Act
            await Sut.UpdateAsync(new string[] {"1", "3"});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(2));
            Assert.That(Sut[0].Id, Is.EqualTo("1"));
            Assert.That(Sut[1].Id, Is.EqualTo("3"));
            _outputObject1.Mock.Verify(x => x.InitializeAsync("1"), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync("2"), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync("3"), Times.Once);
        }

        [Test]
        public async Task WHEN_items_change_position_SHOULD_reorder()
        {
            //Arrange
            
            MockServiceLocator.Mock.Setup(x => x.Resolve<IOutputObject>()).Returns(new Queue<IOutputObject>(new[]
            {
                _outputObject2.Object, _outputObject1.Object, _outputObject3.Object
            }).Dequeue);
         
            await Sut.UpdateAsync(new string[] {"2", "1", "3"});

            //Act
            await Sut.UpdateAsync(new string[] {"3", "1", "2"});

            //Assert
            Assert.That(Sut.Count, Is.EqualTo(3));
            Assert.That(Sut[0].Id, Is.EqualTo("3"));
            Assert.That(Sut[1].Id, Is.EqualTo("1"));
            Assert.That(Sut[2].Id, Is.EqualTo("2"));
            _outputObject1.Mock.Verify(x => x.InitializeAsync("1"), Times.Once);
            _outputObject2.Mock.Verify(x => x.InitializeAsync("2"), Times.Once);
            _outputObject3.Mock.Verify(x => x.InitializeAsync("3"), Times.Once);
        }

    }
}