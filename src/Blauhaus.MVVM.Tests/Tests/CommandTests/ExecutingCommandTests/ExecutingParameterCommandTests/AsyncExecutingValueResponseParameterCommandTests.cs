﻿using System;
using System.Threading.Tasks;
using Blauhaus.Errors;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.Base;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingParameterCommandTests
{
    public class AsyncExecutingValueResponseParameterCommandTests : BaseExecutingCommandTest<AsyncExecutingValueResponseParameterCommand<string, int>>
    {
        private Func<string, Task<Response<int>>> _task;
        private Response<int> _result;
        private Func<bool> _canExecute;
        private Func<int, Task>? _onSuccess;

        public override void Setup()
        {
            base.Setup();

            _result = Response.Success(2);
            _task = async (s) =>
            {
                await Task.CompletedTask;
                return _result;
            };
            _canExecute = () => true;
            _onSuccess = null;
        }

        protected override AsyncExecutingValueResponseParameterCommand<string, int> ConstructSut()
        {
            return base.ConstructSut()
                .WithExecute(_task)
                .OnSuccess(_onSuccess)
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
            _task = async (s) =>
            {
                await Task.CompletedTask;
                wasCalled = true;
                return Response.Success(2);
            };

            //Act
            Sut.Execute("hi");
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
                return Response.Success(2);
            };

            //Act
            Sut.Execute("hi");
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual("hi", result);
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
            Assert.AreEqual("hi 2", result);
        }

        
        [Test]
        public async Task IF_OnSuccess_is_given_SHOULD_NOT_invoke_if_action_returns_fail()
        {
            //Arrange
            _result = Response.Failure<int>(Error.Create("oops"));
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
            _task = (a) => throw new Exception("gosh darn it");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute("hi");
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
            _result = Response.Failure<int>(Error.Create("oops"));
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute("hi");
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
            _result = Response.Failure<int>(TestErrors.Fail());
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute();
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                Assert.That(result == TestErrors.Fail());
                Assert.AreEqual(false, Sut.IsExecuting);
                MockErrorHandler.Verify_HandleErrorMessage_not_called();
            }
        }
    }
}