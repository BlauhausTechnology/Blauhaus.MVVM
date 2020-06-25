﻿using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingValueCommands;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.NotifyPropertyChanged;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingValueCommandTests
{
    public class AsyncExecutingValueCommandTests : BaseExecutingCommandTest<AsyncExecutingCommand<string>>
    {
        private Func<string, Task> _task;
        private Func<bool> _canExecute;

        public override void Setup()
        {
            base.Setup();
            _task =  async (s) => { await Task.CompletedTask;};
            _canExecute = () => true;
        }

        protected override AsyncExecutingCommand<string> ConstructSut()
        {
            return new AsyncExecutingCommand<string>(MockErrorHandlingService.Object, _task, _canExecute);
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
            using (var canExecuteChanges = Sut.Command.SubscribeToCanExecuteChanged())
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
            _task = async (s) =>
            {
                await Task.CompletedTask;
                wasCalled = true;
            };

            //Act
            Sut.Command.Execute("S");
            await Task.Delay(10);

            //Assert
            Assert.IsFalse(wasCalled);
        }
         
        [Test]
        public async Task SHOULD_invoke_given_task()
        {
            //Arrange
            var tcs = new TaskCompletionSource<string>();
            _task = async (s) =>
            {
                await Task.CompletedTask;
                tcs.SetResult(s);
            };

            //Act
            Sut.Command.Execute("s");
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual("s", result);
        }
        

        [Test]
        public void IF_ErrorHandlingService_is_provided_and_task_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _task = (s) => throw new Exception("gosh darn it");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Command.Execute("s");
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandlingService.Verify_HandleExceptionMessage("gosh darn it");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }
         
    }
}