using System;
using System.Collections.Generic;
using Blauhaus.Domain.Client.Sync;
using Blauhaus.Domain.TestHelpers.Extensions;
using Blauhaus.Domain.TestHelpers.MockBuilders.Client.SyncClients;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.MVVM.Xamarin.ViewElements.Sync;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ViewElementTests.SyncCollectionTests
{
    [TestFixture]
    public class InitializeCommandTests : BaseMvvmTest<SyncCollectionViewElement<TestModel, TestListItem, TestSyncCommand>>
    {
        protected SyncClientMockBuilder<TestModel, TestSyncCommand> MockSyncClient => Mocks.AddMockSyncClient<TestModel, TestSyncCommand>().Invoke();

        private DateTime _start;

        public override void Setup()
        {
            base.Setup();

            _start = DateTime.UtcNow;
            
            MockSyncClient.Where_Connect_returns(new List<TestModel>());

            Services.AddSingleton<IModelViewElementUpdater<TestModel, TestListItem>, TestViewElementUpdater>();
            AddService(MockSyncClient.Object);
        }
         

        [Test]
        public void SHOULD_connect_using_configured_properties()
        {
            //Act
            Sut.SyncCommand.FavouriteColour = "Red";
            Sut.SyncRequirement = ClientSyncRequirement.Minimum(100);
            Sut.InitializeCommand.Execute(null);

            //Assert
            MockSyncClient.Mock.Verify(x => x.Connect(
                It.Is<TestSyncCommand>(y => y.FavouriteColour == "Red"), 
                It.Is<ClientSyncRequirement>(z => z.SyncMinimumQuantity.Value == 100), 
                Sut.SyncStats));
        }
        
        [Test]
        public void IF_connect_returns_exception_SHOULD_handle()
        {
            //Arrange
            MockSyncClient.Where_Connect_returns_exception(new Exception("oops"));

            //Act
            Sut.InitializeCommand.Execute(null);

            //Assert
            MockErrorHandlingService.Verify_HandleExceptionMessage("oops");
        }
        
        [Test]
        public void IF_elementUpdater_thorws_exception_SHOULD_handle()
        {
            //Arrange
            var newModels = TestModel.Generate(3, _start);
            MockSyncClient.Where_Connect_returns(newModels);
            Services.AddSingleton<IModelViewElementUpdater<TestModel, TestListItem>, ExceptionViewElementUpdater>();

            //Act
            Sut.InitializeCommand.Execute(null);

            //Assert
            MockErrorHandlingService.Verify_HandleExceptionMessage("This is an exceptionally bad thing that just happened");
        }

        [Test]
        public void SHOULD_publish_new_models()
        {
            //Arrange
            var newModels = TestModel.Generate(3, _start);
            MockSyncClient.Where_Connect_returns(newModels);

            //Act
            Sut.InitializeCommand.Execute(null);

            //Assert
            Assert.AreEqual(3, Sut.Count);
            Assert.AreEqual(newModels[0].Id, Sut[0].Id);
            Assert.AreEqual(newModels[0].Name, Sut[0].Name);
            Assert.AreEqual(newModels[1].Id, Sut[1].Id);
            Assert.AreEqual(newModels[1].Name, Sut[1].Name);
            Assert.AreEqual(newModels[2].Id, Sut[2].Id);
            Assert.AreEqual(newModels[2].Name, Sut[2].Name);
        }
        
        [Test]
        public void SHOULD_publish_models_most_recent_first()
        {
            //Arrange
            MockSyncClient.Where_Connect_returns(new List<TestModel>
            {
                new TestModel(Guid.NewGuid(), "A", 2000),
                new TestModel(Guid.NewGuid(), "B", 1000),
                new TestModel(Guid.NewGuid(), "C", 3000),
            });

            //Act
            Sut.InitializeCommand.Execute(null);

            //Assert
            Assert.AreEqual(3000, Sut[0].ModifiedAtTicks);
            Assert.AreEqual(2000, Sut[1].ModifiedAtTicks);
            Assert.AreEqual(1000, Sut[2].ModifiedAtTicks);
        }

        [Test]
        public void WHEN_model_Changes_SHOULD_update_it_and_reorder_instead_of_adding_new()
        {
            //Arrange
            var update1 = new TestModel(Guid.NewGuid(), "A", 1000);
            var update2 = new TestModel(Guid.NewGuid(), "B", 2000);
            var update3 = new TestModel(update1.Id, "D", 4000);
            MockSyncClient.Where_Connect_returns(new List<TestModel>{update1, update2, update3});

            //Act
            Sut.InitializeCommand.Execute(null);

            //Assert
            Assert.AreEqual(2, Sut.Count);
            Assert.AreEqual(4000, Sut[0].ModifiedAtTicks);
            Assert.AreEqual("D", Sut[0].Name);
            Assert.AreEqual(update1.Id, Sut[0].Id);
            Assert.AreEqual(2000, Sut[1].ModifiedAtTicks);
            Assert.AreEqual("B", Sut[1].Name);
            Assert.AreEqual(update2.Id, Sut[1].Id);

        }


    }
}