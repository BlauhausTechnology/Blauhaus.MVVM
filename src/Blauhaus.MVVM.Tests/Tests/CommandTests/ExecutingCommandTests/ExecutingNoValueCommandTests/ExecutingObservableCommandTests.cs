using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests._Base;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands;
using Blauhaus.TestHelpers.MockBuilders;
using Blauhaus.TestHelpers.PropertiesChanged.CanExecuteChanged;
using Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged;
using Moq;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.ExecutingNoValueCommandTests
{
    public class ExecutingObservableCommandTests : BaseExecutingCommandTest<ExecutingObservableCommand<int>>
    {
        private Action<int> _onNext;
        private Func<bool> _canExecute;
        private IObservable<int> _observable;
        private Action? _onCompleted;
        private string _operationName;
        private List<int> _calls;
        private MockBuilder<IAnalyticsOperation> MockOperation;

        public override void Setup()
        {
            base.Setup();

            MockOperation = MockAnalyticsService.Where_StartOperation_returns_operation();

            _calls = new List<int>();
            _onNext =   (i) => { _calls.Add(i); };
            _canExecute = () => true;
            _observable = Observable.Create<int>(observer =>
            {
                observer.OnNext(1); 
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        protected override ExecutingObservableCommand<int> ConstructSut()
        {
            return new ExecutingObservableCommand<int>(MockErrorHandler.Object, MockAnalyticsService.Object, 
                _observable, _onNext, _onCompleted, _canExecute, _operationName);
        }

        [Test]
        public async Task IF_CanExecute_is_given_And_returns_false_SHOULD_not_execute()
        {
            //Arrange 
            _canExecute = () => false;

            //Act
            Sut.Command.Execute(null);
            await Task.Delay(10);

            //Assert
            Assert.That(_calls.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task SHOULD_start_operation_and_dispose_operation_when_completed()
        {
            //Arrange 
            _operationName = "MyOp";

            //Act
            Sut.Command.Execute(null);
            MockAnalyticsService.VerifyStartOperation("MyOp"); 

            //Assert
            MockOperation.Mock.Verify(x => x.Dispose(), Times.Once);
        }


        [Test]
        public async Task SHOULD_subscribe_to_observable_and_invoke_onNext()
        {
            //Arrange
            var result = 0;
            _onNext =  i =>
            {
                result = i;
            }; 

            //Act
            Sut.Command.Execute(null);  

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
        public void IF_observer_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _observable = Observable.Create<int>(observer =>
            {
                observer.OnError(new Exception("oops"));
                return Disposable.Empty;
            }); 
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Command.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleExceptionMessage("oops");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }

        [Test]
        public void IF_onNext_throws_exception_SHOULD_handle_and_reset_IsExecuting()
        {
            //Arrange
            _onNext = i => throw new Exception("oopss");
            
            using (var isExecutingChanges = Sut.SubscribeToPropertyChanged(x => x.IsExecuting))
            {
                //Act
                Sut.Command.Execute(null);
                isExecutingChanges.WaitForChangeCount(2);

                //Assert
                MockErrorHandler.Verify_HandleExceptionMessage("oopss");
                Assert.AreEqual(false, Sut.IsExecuting);
            }
        }
         
    }
}