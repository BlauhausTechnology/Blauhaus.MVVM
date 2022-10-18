using Blauhaus.MVVM.Abstractions.TargetNavigation;
using NUnit.Framework;
// ReSharper disable ExplicitCallerInfoArgument

namespace Blauhaus.MVVM.Tests.Tests.NavigatorTests.ViewIdentifierTests;

public class DeserializeTests
{
     
    [Test]
    public void IF_no_properties_SHOULD_return_only_name()
    {
        //Arrange
        var sut = new ViewIdentifier("MyView");

        //Act
        var result = ViewIdentifier.Deserialize(sut.Serialize());

        //Assert
        Assert.That(result, Is.EqualTo(sut));
    }

    [Test]
    public void IF_one_property_SHOULD_append()
    {
        //Arrange
        var sut = new ViewIdentifier("MyView")
        {
            Properties =
            {
                ["one"] = "1"
            }
        };
        
        //Act
        var result = ViewIdentifier.Deserialize(sut.Serialize());

        //Assert
        Assert.That(result, Is.EqualTo(sut));
    }

    [Test]
    public void IF_multiple_properties_SHOULD_append()
    {
        //Arrange
        var sut = new ViewIdentifier("MyView")
        {
            Properties =
            {
                ["one"] = "1",
                ["two"] = "2"
            }
        };
        
        //Act
        var result = ViewIdentifier.Deserialize(sut.Serialize());

        //Assert
        Assert.That(result, Is.EqualTo(sut));
    }
		
}