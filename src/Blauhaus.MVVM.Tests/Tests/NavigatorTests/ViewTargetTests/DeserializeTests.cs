using Blauhaus.Common.Abstractions.Extensions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Navigation;

namespace Blauhaus.MVVM.Tests.Tests.NavigatorTests.ViewTargetTests;

public class DeserializeTests 
{
    [Test]
    public void SHOULD_deserialize_path()
    {
        //Arrange
        var sut = ViewTarget.Create(
            new ViewIdentifier("Container")
                .WithValue("key1", "value1"),
            new ViewIdentifier("Content")
                .WithValue("key2", "value2")
                .WithValue("key3", "value3"));
        
        //Act
        var result = ViewTarget.Deserialize(sut.Path);

        //Assert
        Assert.That(result[0].Name, Is.EqualTo("Container"));
        Assert.That(result[0].Properties["key1"], Is.EqualTo("value1"));
        Assert.That(result[1].Name, Is.EqualTo("Content"));
        Assert.That(result[1].Properties["key2"], Is.EqualTo("value2"));
        Assert.That(result[1].Properties["key3"], Is.EqualTo("value3"));
    }
}