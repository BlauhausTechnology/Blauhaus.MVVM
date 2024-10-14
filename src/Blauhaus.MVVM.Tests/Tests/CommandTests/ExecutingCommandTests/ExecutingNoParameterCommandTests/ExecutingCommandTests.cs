using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.Base;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using NUnit.Framework.Legacy;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoParameterCommandTests
{
    public class ExecutingCommandTests : BaseExecutingCommandTest<ExecutingCommand>
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
            return base.ConstructSut()
                .WithExecute(_action)
                .WithCanExecute(_canExecute);
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
            Sut.Execute(null);
            await Task.Delay(10);

            //Assert
            ClassicAssert.IsFalse(wasCalled);
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
            Sut.Execute(null);
            var result = await tcs.Task;

            //Assert
            ClassicAssert.AreEqual(1, result);
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
                ClassicAssert.AreEqual(1, canExecuteChanges.Count);
                ClassicAssert.IsTrue(canExecuteWasCalled);
                ClassicAssert.AreEqual(true, canExecuteChanges[0]);
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
                Sut.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleExceptionMessage("gosh darn it");
                ClassicAssert.AreEqual(false, Sut.IsExecuting);
            }
        }
         
    }
}