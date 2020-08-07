using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoValueCommandTests
{
    public class ExecutingNoValueCommandTests : BaseExecutingCommandTest<ExecutingCommand>
    {
        private Action _action;
        private Func<bool> _canExecute;

        public override void Setup()
        {
            base.Setup();
            _action =   () => { };
            _canExecute = () => true;
        }

        protected override ExecutingCommand ConstructSut()
        {
            return new ExecutingCommand(MockErrorHandler.Object, _action, _canExecute);
        }

        [Test]
        public async Task IF_CanExecute_is_given_And_returns_false_SHOULD_not_execute()
        {
            //Arrange
            var wasCalled = false;
            _canExecute = () => false;
            _action =  () =>
            {
                wasCalled = true;
            };

            //Act
            Sut.Command.Execute(null);
            await Task.Delay(10);

            //Assert
            Assert.IsFalse(wasCalled);
        }

        [Test]
        public async Task SHOULD_invoke_given_action()
        {
            //Arrange
            var tcs = new TaskCompletionSource<int>();
            _action =  () =>
            {
                tcs.SetResult(1);
            };

            //Act
            Sut.Command.Execute(null);
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual(1, result);
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
        public void IF_action_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _action = () => throw new Exception("gosh darn it");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Command.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleExceptionMessage("gosh darn it");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }
         
    }
}