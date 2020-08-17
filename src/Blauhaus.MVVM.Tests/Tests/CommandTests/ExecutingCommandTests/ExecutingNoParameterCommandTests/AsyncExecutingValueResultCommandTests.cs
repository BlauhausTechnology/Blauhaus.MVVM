using System;
using System.Threading.Tasks;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using CSharpFunctionalExtensions;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoParameterCommandTests
{
    public class AsyncExecutingValueResultCommandTests : BaseExecutingCommandTest<AsyncExecutingValueResultCommand<int>>
    {
        private Func<Task<Result<int>>> _task;
        private Result<int> _result;
        private Func<bool> _canExecute;
        private Func<int, Task> _onSuccess;

        public override void Setup()
        {
            base.Setup();

            _result = Result.Success(1);
            _task = async () =>
            {
                await Task.CompletedTask;
                return _result;
            };
            _canExecute = () => true;
            _onSuccess = null;
        }

        protected override AsyncExecutingValueResultCommand<int> ConstructSut()
        {
            return base.ConstructSut()
                .OnSuccess(_onSuccess)
                .WithExecute(_task)
                .WithCanExecute(_canExecute);
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
                return Result.Success(1);
            };

            //Act
            Sut.Execute(null);
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
                return Result.Success(1);
            };

            //Act
            Sut.Execute(null);
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public async Task IF_OnSuccess_is_given_SHOULD_invoke_onsuccess()
        {
            //Arrange
            var tcs = new TaskCompletionSource<string>();
            _onSuccess = async (i) =>
            {
                await Task.CompletedTask;
                tcs.SetResult("hi " + i);
            };

            //Act
            Sut.Execute("hi");
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual("hi 1", result);
        }

        
        [Test]
        public async Task IF_OnSuccess_is_given_SHOULD_NOT_invoke_if_action_returns_fail()
        {
            //Arrange
            _result = Result.Failure<int>("oops");
            var onSuccess = "notCalled";
            _onSuccess = async (i) =>
            {
                onSuccess = "called";
                await Task.CompletedTask;
            };

            //Act
            await Task.Delay(10);
            Sut.Execute("hi"); 

            //Assert
            Assert.That(onSuccess, Is.EqualTo("notCalled"));
            MockErrorHandler.Mock.Verify(x => x.HandleExceptionAsync(It.IsAny<object>(), It.Is<Exception>(y =>
                y.Message.StartsWith("You attempted to access"))), Times.Never);
        }


        [Test]
        public void IF_task_throws_exception_SHOULD_handle_and_reset_IsExecuting()
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
        public void IF_task_returns_fail_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _result = Result.Failure<int>("oops");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute();
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleError("oops");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }
        
        [Test]
        public void IF_OnFailure_is_given_and_task_returns_correct_fail_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            Error result = default;
            Sut.OnFailure(TestErrors.Fail(), x =>
            {
                result = x;
                return Task.CompletedTask;
            });
            _result = Result.Failure<int>(TestErrors.Fail().ToString());
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute();
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                Assert.That(result == TestErrors.Fail());
                Assert.AreEqual(false, Sut.IsExecuting);
                MockErrorHandler.Verify_HandleError_not_called();
            }
        }
         
    }
}