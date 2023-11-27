using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.ExecutingCommands.Base;
using Blauhaus.MVVM.Tests.Tests.Base;
using Blauhaus.TestHelpers.Extensions;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.Base
{
    [TestFixture]
    public class BaseExecutingCommandTest<TCommand> : BaseMvvmTest<TCommand> where TCommand : BaseExecutingCommand<TCommand>
    {
        
        private AnalyticsOperationMockBuilder _mockOperation = null!;
        private Mock<IDisposable> _mockDisposable = null!;

        public override void Setup()
        {
            base.Setup();
            _mockDisposable = MockLogger.MockScopeDisposable;
        }

        [Test]
        public void SHOULD_set_and_reset_IsExecuting()
        {
            //Act
            using var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting);

            //Act
            Sut.Execute(default);
            isExecutingChanges.WaitForChangeCount(2);
                
            //Assert
            ClassicAssert.AreEqual(true, isExecutingChanges[0]);
            ClassicAssert.AreEqual(false, isExecutingChanges[1]);
        }

        [Test]
        public void SHOULD_set_and_reset_CanExecute()
        {
            //Act
            using var canExecuteChanges = Sut.SubscribeToCanExecuteChanged();

            //Act
            Sut.Execute(null);
            canExecuteChanges.WaitForChangeCount(2);
                
            //Assert
            ClassicAssert.AreEqual(false, canExecuteChanges[0]);
            ClassicAssert.AreEqual(true, canExecuteChanges[1]);
        }

        [Test]
        public void IF_LogAction_is_set_SHOULD_clear_values()
        {
            //Arrange 
            Sut.LogAction<TCommand>("MyAction", LogLevel.Critical);

            //Act
            Sut.Execute();

            //Assert
            MockLogger.Mock.Verify(x => x.SetValue("ActionId", It.Is<string>(y => y.Length > 30)), Times.Once);
            MockLogger.Mock.Verify(x => x.SetValue("ActionName", It.Is<string>(y => y == "MyAction")), Times.Once);
            MockLogger.Mock.Verify(x => x.SetValue("ActionSource", It.Is<string>(y => y == typeof(TCommand).Name)), Times.Once);
            MockLogger.Mock.Verify(x => x.SetValue("ActionId", It.Is<string>(y => y == "")), Times.Once);
            MockLogger.Mock.Verify(x => x.SetValue("ActionName", It.Is<string>(y => y == "")), Times.Once);
            MockLogger.Mock.Verify(x => x.SetValue("ActionSource", It.Is<string>(y => y == "")), Times.Once);
        }
        
        [Test]
        public void IF_LogAction_is_set_SHOULD_log()
        {
            //Arrange 
            Sut.LogAction<TCommand>("MyAction", LogLevel.Critical);

            //Act
            Sut.Execute();

            //Assert
            MockLogger.VerifyBeginTimedScope(LogLevel.Critical, "MyAction");
            _mockDisposable.Verify(x => x.Dispose(), Times.Once);
            MockLogger.MockScopeDisposable.Verify(x => x.Dispose());
        }

        [Test]
        public void IF_IsExecuting_is_given_SHOULD_notify()
        {
            //Arrange 
            var mock = new MockIsExecuting();
            Sut.WithIsExecuting(mock.Object);

            //Act
            Sut.Execute();

            //Assert
            mock.Mock.VerifySet(x => x.IsExecuting = true, Times.Once);
            mock.Mock.VerifySet(x => x.IsExecuting = false, Times.Once);
        }
        [Test]
        public void IF_IsExecuting_has_executing_commands_SHOULD_raise()
        {
            //Arrange 
            var mock = new MockIsExecuting();
            Sut.WithIsExecuting(mock.Object);

            //Act
            Sut.Execute();

            //Assert
            mock.MockCommand.Verify(x => x.RaiseCanExecuteChanged(), Times.Exactly(2));
        }

        [Test]
        public void IF_IsExecuting_is_given_SHOULD_set()
        {
            //Arrange
            var isExecuting = new TestIsExecuting();
            Sut.WithIsExecuting(isExecuting);
            
            //Act
            Sut.Execute();
            
            //Assert
            Assert.That(isExecuting.Sets.Count, Is.EqualTo(2));
            Assert.That(isExecuting.Sets[0], Is.True);
            Assert.That(isExecuting.Sets[1], Is.False);
        }
        
        [Test]
        public void IF_IsExecuting_is_given_and_is_true_SHOULD_disable()
        {
            //Arrange
            var isExecuting = new TestIsExecuting { IsExecuting = true };
            Sut.WithIsExecuting(isExecuting);
            
            //Assert
            Assert.That(Sut.CanExecute(null), Is.False);
        }
      
    }
}


internal class TestIsExecuting : IIsExecuting
{
    private bool _isExecuting;

    public bool IsExecuting
    {
        get
        {
            Gets.Add(_isExecuting);
            return _isExecuting;
        }
        set
        {
            Sets.Add(value);
            _isExecuting = value;
        }
    }

    public List<bool> Gets { get; } = new();
    public List<bool> Sets { get; } = new();
}