using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.NotifyPropertyChanged;
using CSharpFunctionalExtensions;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoValueCommandTests
{
    public class AsyncExecutingNoValueResultCommandTests : BaseExecutingCommandTest<AsyncExecutingResultCommand>
    {
        private Func<Task<Result>> _task;
        private Result _result;
        private Func<bool> _canExecute;

        public override void Setup()
        {
            base.Setup();

            _result = Result.Success();
            _task = async () =>
            {
                await Task.CompletedTask;
                return _result;
            };
            _canExecute = () => true;
        }

        protected override AsyncExecutingResultCommand ConstructSut()
        {
            return new AsyncExecutingResultCommand(MockErrorHandlingService.Object, _task, _canExecute);
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
            _task = async () =>
            {
                await Task.CompletedTask;
                wasCalled = true;
                return Result.Success();
            };

            //Act
            Sut.Command.Execute(null);
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
                return Result.Success();
            };

            //Act
            Sut.Command.Execute(null);
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void IF_task_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _task = () => throw new Exception("gosh darn it");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Command.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandlingService.Verify_HandleExceptionMessage("gosh darn it");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }

        [Test]
        public void IF_task_returns_fail_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _result = Result.Failure("oops");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Command.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandlingService.Verify_HandleError("oops");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }
         
    }
}