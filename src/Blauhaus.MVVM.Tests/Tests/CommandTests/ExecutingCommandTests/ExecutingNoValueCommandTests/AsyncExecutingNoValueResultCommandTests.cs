﻿using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using CSharpFunctionalExtensions;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoValueCommandTests
{
    public class AsyncExecutingNoValueResultCommandTests : BaseExecutingCommandTest<AsyncExecutingResultCommand>
    {
        private Func<Task<Result>> _task;
        private Result _result;
        private Func<bool> _canExecute;
        private Func<Task>? _onSuccess;

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
            _onSuccess = null;
        }

        protected override AsyncExecutingResultCommand ConstructSut()
        {
            return base.ConstructSut()
                .WithOnSuccess(_onSuccess)
                .WithTask(_task)
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
                return Result.Success();
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
                return Result.Success();
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
            _onSuccess = async () =>
            {
                await Task.CompletedTask;
                tcs.SetResult("hi");
            };

            //Act
            Sut.Execute("hi");
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual("hi", result);
        }

        [Test]
        public void IF_task_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _task = () => throw new Exception("gosh darn it");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute(null);
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
            _result = Result.Failure("oops");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleError("oops");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }
         
    }
}