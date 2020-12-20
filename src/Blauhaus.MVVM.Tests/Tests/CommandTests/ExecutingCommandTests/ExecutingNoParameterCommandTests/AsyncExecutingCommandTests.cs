using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoParameterCommandTests
{
    public class AsyncExecutingCommandTests : BaseExecutingCommandTest<AsyncExecutingCommand>
    {
        private Func<Task> _task;
        private Func<bool> _canExecute;

        public override void Setup()
        {
            base.Setup();
            _task =  async () => { await Task.CompletedTask;};
            _canExecute = () => true;
        }

        protected override AsyncExecutingCommand ConstructSut()
        {
            return base.ConstructSut()
                .WithCanExecute(_canExecute)
                .WithExecute(_task);
        }
        
        [Test]
        public void RaiseCanExecuteChanged_SHOULD_call_CanExecute_and_publish_result()
        {
            //Arrange
            var canExecuteWasCalled = false;
            _canExecute = () =>
            {
                canExecuteWasCalled = true;
                return true;
            };

            //Act
            using (var canExecuteChanges = Sut.SubscribeToCanExecuteChanged())
            {
                //Act
                Sut.RaiseCanExecuteChanged();
                
                //Assert
                Assert.AreEqual(1, canExecuteChanges.Count);
                Assert.IsTrue(canExecuteWasCalled);
                Assert.AreEqual(true, canExecuteChanges[0]);
            }
        }

        [Test]
        public async Task IF_CanExecute_is_given_And_returns_false_SHOULD_not_execute()
        {
            //Arrange
            var wasCalled = false;
            _canExecute = () => false;
            _task = async () =>
            {
                await Task.CompletedTask;
                wasCalled = true;
            };

            //Act
            Sut.Execute();
            await Task.Delay(10);

            //Assert
            Assert.IsFalse(wasCalled);
        }
         
        [Test]
        public async Task SHOULD_invoke_given_task()
        {
            //Arrange
            var tcs = new TaskCompletionSource<int>();
            _task = async () =>
            {
                await Task.CompletedTask;
                tcs.SetResult(1);
            };

            //Act
            Sut.Execute();
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void IF_ErrorHandlingService_is_provided_and_task_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _task = () => throw new Exception("gosh darn it");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute();
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleExceptionMessage("gosh darn it");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }

        [Test]
        public void IF_AnalyticsOperation_is_providedSHOULD_log()
        {
            //Arrange 
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Arrange
                Sut.LogOperation(this, "ops");

                //Act
                Sut.Execute();
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockAnalyticsService.Mock.Verify(x => x.StartOperation(
                    It.Is<object>(y => y.GetType() == typeof(AsyncExecutingCommandTests)), 
                    "ops", It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            }
        }
        
        [Test]
        public void IF_AnalyticsOperation_is_provided_without_name_SHOULD_log_Command_name()
        {
            //Arrange 
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Arrange
                Sut.LogOperation(this);

                //Act
                Sut.Execute();
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockAnalyticsService.Mock.Verify(x => x.StartOperation(
                    It.Is<object>(y => y.GetType() == typeof(AsyncExecutingCommandTests)), 
                    "ops", It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            }
        }
         

    }
}