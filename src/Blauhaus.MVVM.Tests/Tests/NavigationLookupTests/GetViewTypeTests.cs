using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.Navigation.Register;
using Blauhaus.MVVM.Tests.TestObjects;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Blauhaus.MVVM.Tests.Tests.NavigationLookupTests
{
    public class GetViewTypeTests
    {

        [Test]
        public void SHOULD_return_type_registered_for_viewmodel()
        {
            //Arrange
            var sut = new NavigationRegister();
            sut.Register<TestView, TestViewModel>();

            //Act
            var result = sut.GetViewType<TestViewModel>();

            //Assert
            ClassicAssert.AreEqual(typeof(TestView), result);
        }

        [Test]
        public void SHOULD_return_null_if_viewmodel_has_not_been_registered()
        {
            //Arrange
            var sut = new NavigationRegister();

            //Act
            var result = sut.GetViewType<TestViewModel>();

            //Assert
            ClassicAssert.IsNull(result);
        }
    }
}