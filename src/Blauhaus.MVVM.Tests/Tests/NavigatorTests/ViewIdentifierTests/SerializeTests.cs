using Blauhaus.MVVM.Abstractions.TargetNavigation;
using NUnit.Framework;
// ReSharper disable ExplicitCallerInfoArgument

namespace Blauhaus.MVVM.Tests.Tests.NavigatorTests.ViewIdentifierTests;

public class SerializeTests
{
     
    [Test]
    public void IF_no_properties_SHOULD_return_only_name()
    {
        //Arrange
        var sut = new ViewIdentifier("MyView");

        //Assert
        Assert.That(sut.ToString(), Is.EqualTo("/MyView"));
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

        //Assert
        Assert.That(sut.ToString(), Is.EqualTo("/MyView?one=1"));
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

        //Assert
        Assert.That(sut.ToString(), Is.EqualTo("/MyView?one=1&two=2"));
    }
		
}