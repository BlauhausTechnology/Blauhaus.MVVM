using Blauhaus.MVVM.ExecutingCommands._Base;
using Blauhaus.MVVM.Tests.Tests._Base;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base
{
    public class BaseExecutingCommandTest<TCommand> : BaseMvvmTest<TCommand> where TCommand : BaseExecutingCommand<TCommand>
    {
        
        [Test]
        public void SHOULD_set_and_reset_IsExecuting()
        {
            //Act
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Execute(default);
                isExecutingChanges.WaitForChangeCount(2);
                
                //Assert
                Assert.AreEqual(true, isExecutingChanges[0]);
                Assert.AreEqual(false, isExecutingChanges[1]);
            }
        }

        
        [Test]
        public void SHOULD_set_and_reset_CanExecute()
        {
            //Act
            using (var canExecuteChanges = Sut.SubscribeToCanExecuteChanged())
            {
                //Act
                Sut.Execute(null);
                canExecuteChanges.WaitForChangeCount(2);
                
                //Assert
                Assert.AreEqual(false, canExecuteChanges[0]);
                Assert.AreEqual(true, canExecuteChanges[1]);
            }
        }
    }
}