using System;
using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.MVVM.Tests.Tests.ViewTests._Base;
using Blauhaus.TestHelpers.MockBuilders;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.ViewTests.BaseAppearingContentPageTests
{
    public class OnAppearingTests : BaseViewTest<TestAppearingContentPage>
    {
        private TestAppearingViewModel _testViewmodel;
        private MockBuilder<IExecutingCommand> _appearingCommandMock;
        private MockBuilder<ICommand> _disappearingCommandMock;

        public override void Setup()
        {
            base.Setup();
            _appearingCommandMock = new MockBuilder<IExecutingCommand>();
            _disappearingCommandMock = new MockBuilder<ICommand>();

            _testViewmodel = new TestAppearingViewModel(_appearingCommandMock.Object, _disappearingCommandMock.Object);
        }

        protected override TestAppearingContentPage ConstructSut()
        {
            return new TestAppearingContentPage(_testViewmodel);
        }

        [Test]
        public void SHOULD_execute_ViewModel_AppearingCommand()
        {
            //Act
            Sut.SendAppearing();

            //Assert
            _appearingCommandMock.Mock.Verify(x => x.Execute(null));
        }

        [Test]
        public void IF_ViewModel_is_null_SHOULD_throw_exception()
        {
            //Arrange
            _testViewmodel = null;

            //Assert
            Assert.Throws<Exception>(() => Sut.SendAppearing(), "Cannot call AppearingCommand because TestAppearingViewModel is null");
        }

        [Test]
        public void IF_AppearingCommand_is_null_SHOULD_throw_exception()
        {
            //Arrange
            _testViewmodel = new TestAppearingViewModel(null, _disappearingCommandMock.Object);

            //Assert
            Assert.Throws<Exception>(() => Sut.SendAppearing(), "Cannot call AppearingCommand on TestAppearingViewModel because the command is null");
        }
    }
}