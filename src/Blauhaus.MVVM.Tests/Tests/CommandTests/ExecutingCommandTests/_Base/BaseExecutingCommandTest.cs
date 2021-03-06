﻿using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.MVVM.ExecutingCommands._Base;
using Blauhaus.MVVM.Tests.Tests.Base;
using Blauhaus.TestHelpers.MockBuilders;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base
{
    public class BaseExecutingCommandTest<TCommand> : BaseMvvmTest<TCommand> where TCommand : BaseExecutingCommand<TCommand>
    {
        
        private MockBuilder<IAnalyticsOperation> _mockOperation;
        public override void Setup()
        {
            base.Setup();
            _mockOperation = MockAnalyticsService.Where_StartOperation_returns_operation();
        }

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

        [Test]
        public void IF_AnalyticsOperationName_is_set_SHOULD_start_operation_and_dispose_operation_when_completed()
        {
            //Arrange 
            _mockOperation = MockAnalyticsService.Where_StartOperation_returns_operation();
            Sut.LogOperation(this, "MyOp");

            //Act
            Sut.Execute();

            //Assert
            MockAnalyticsService.VerifyStartOperation("MyOp"); 
            MockAnalyticsService.Mock.Verify(x => x.StartOperation(It.Is<object>(y => y.GetType() == this.GetType()), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            _mockOperation.Mock.Verify(x => x.Dispose(), Times.Once);
        }

        
        [Test]
        public void IF_IsPageView_SHOULD_start_PageView_operation_and_dispose_operation_when_completed()
        {
            //Arrange 
            _mockOperation = MockAnalyticsService.Where_StartPageViewOperation_returns_operation();
            Sut.LogPageView(this);

            //Act
            Sut.Execute();

            //Assert
            MockAnalyticsService.VerifyStartPageViewOperation(); 
            MockAnalyticsService.Mock.Verify(x => x.StartPageViewOperation(It.Is<object>(y => y.GetType() == this.GetType()), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            _mockOperation.Mock.Verify(x => x.Dispose(), Times.Once);
        }

    }
}