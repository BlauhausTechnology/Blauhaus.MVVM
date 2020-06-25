using Blauhaus.MVVM.Tests.TestObjects;
using Blauhaus.TestHelpers.PropertiesChanged.NotifyPropertyChanged;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.BindableObjectTests
{
    public class SetPropertyTests
    {
        [Test]
        public void SHOULD_set_value()
        {
            //Arrange
            var sut = new TestBindableObject(1);

            //Act
            sut.CountMe = 2;

            //Assert
            Assert.AreEqual(2, sut.CountMe);
        }

        [Test]
        public void WHEN_property_is_different_SHOULD_notify_changed()
        {
            //Arrange
            var sut = new TestBindableObject(1);
            using (var changedProperties = sut.SubscribeToPropertyChanged<TestBindableObject, int>(x => x.CountMe))
            {
                //Act
                sut.CountMe = 2;
                sut.CountMe = 2;
                sut.CountMe = 3;

                //Assert
                Assert.AreEqual(2, changedProperties.Count);
                Assert.AreEqual(2, changedProperties[0]);
                Assert.AreEqual(3, changedProperties[1]);
            }
        }

        [Test]
        public void WHEN_property_is_not_different_SHOULD_not_notify_changed()
        {
            //Arrange
            var sut = new TestBindableObject(2);
            using (var changedProperties = sut.SubscribeToPropertyChanged<TestBindableObject, int>(x => x.CountMe))
            {
                //Act
                sut.CountMe = 2;
                sut.CountMe = 2;
                sut.CountMe = 2;

                //Assert
                Assert.AreEqual(0, changedProperties.Count);
            }
        }

        [Test]
        public void WHEN_property_is_different_and_action_has_been_set_SHOULD_call_action()
        {
            //Arrange
            var sut = new TestBindableObject(1);
            using (var changedProperties = sut.SubscribeToPropertyChanged<TestBindableObject, int>(x => x.SideEffect))
            {
                //Act
                sut.CountMeWithSideEffect = 2;
                sut.CountMeWithSideEffect = 2;
                sut.CountMeWithSideEffect = 3;

                //Assert
                Assert.AreEqual(2, changedProperties.Count);
                Assert.AreEqual(2, changedProperties[0]);
                Assert.AreEqual(3, changedProperties[1]);
            }
        }

        [Test]
        public void WHEN_property_is_not_different_and_action_has_been_set_SHOULD_not_call_action()
        {
            //Arrange
            var sut = new TestBindableObject(2);
            using (var changedProperties = sut.SubscribeToPropertyChanged<TestBindableObject, int>(x => x.SideEffect))
            {
                //Act
                sut.CountMeWithSideEffect = 2;
                sut.CountMeWithSideEffect = 2;
                sut.CountMeWithSideEffect = 2;

                //Assert
                Assert.AreEqual(0, changedProperties.Count);
            }
        }
    }
}