using System;
using System.Threading.Tasks;
using Blauhaus.MVVM.ExecutingCommands.ExecutingParameterCommands;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingParameterCommandTests
{
    public class ExecutingParameterCommandTests : BaseExecutingCommandTest<ExecutingParameterCommand<string>>
    {
        private Action<string> _action;
        private Func<bool> _canExecute;

        public override void Setup()
        {
            base.Setup();
            _action =   (s) => { };
            _canExecute = () => true;
        }

        protected override ExecutingParameterCommand<string> ConstructSut()
        {
            return base.ConstructSut()
                .WithCanExecute(_canExecute)
                .WithExecute(_action);
        }

        [Test]
        public async Task IF_CanExecute_is_given_And_returns_false_SHOULD_not_execute()
        {
            //Arrange
            var wasCalled = "false";
            _canExecute = () => false;
            _action =  (s) =>
            {
                wasCalled = s;
            };

            //Act
            Sut.Execute("hi");
            await Task.Delay(10);

            //Assert
            Assert.AreEqual("false", wasCalled);
        }

        [Test]
        public async Task SHOULD_invoke_given_action()
        {
            //Arrange
            var tcs = new TaskCompletionSource<string>();
            _action =  (s) =>
            {
                tcs.SetResult(s);
            };

            //Act
            Sut.Execute("bye");
            var result = await tcs.Task;

            //Assert
            Assert.AreEqual("bye", result);
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
        public void IF_action_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _action = (s) => throw new Exception("gosh darn it");
            

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
         
    }
}