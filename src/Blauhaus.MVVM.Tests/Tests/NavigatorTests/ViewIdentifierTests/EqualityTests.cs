using System.Collections.Generic;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using NUnit.Framework;

namespace Blauhaus.MVVM.Tests.Tests.NavigatorTests.ViewIdentifierTests;

public class EqualityTests
{
    [Test]
    public void IF_names_same_and_no_properties_TRUE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView");
        var two = new ViewIdentifier("MyView");

        //Assert
        Assert.That(one == two, Is.True);
        Assert.That(one.Equals(two), Is.True);
        Assert.That(two == one, Is.True);
        Assert.That(two != one, Is.False);
        Assert.That(one != two, Is.False);
    }
    [Test]
    public void IF_names_same_and_multiple_properties_same_TRUE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1", ["two"] = "2" });
        var two = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1", ["two"] = "2"});

        //Assert
        Assert.That(one == two, Is.True);
        Assert.That(one.Equals(two), Is.True);
        Assert.That(two == one, Is.True);
        Assert.That(two != one, Is.False);
        Assert.That(one != two, Is.False);
    }
    
    [Test]
    public void IF_names_same_and_single_properties_same_TRUE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1" });
        var two = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1" });

        //Assert
        Assert.That(one == two, Is.True);
        Assert.That(one.Equals(two), Is.True);
        Assert.That(two == one, Is.True);
        Assert.That(two != one, Is.False);
        Assert.That(one != two, Is.False);
    }


    [Test]
    public void IF_names_same_and_one_has_properties_FALSE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1"});
        var two = new ViewIdentifier("MyView");

        //Assert
        Assert.That(one == two, Is.False);
        Assert.That(one.Equals(two), Is.False);
        Assert.That(two == one, Is.False);
        Assert.That(two != one, Is.True);
        Assert.That(one != two, Is.True);
    }
    
    [Test]
    public void IF_names_same_and_one_property_different_FALSE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1"});
        var two = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "2"});

        //Assert
        Assert.That(one == two, Is.False);
        Assert.That(one.Equals(two), Is.False);
        Assert.That(two == one, Is.False);
        Assert.That(two != one, Is.True);
        Assert.That(one != two, Is.True);
    }
    
    [Test]
    public void IF_names_different_but_properties_same_FALSE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView", new Dictionary<string, string>{["one"] = "1"});
        var two = new ViewIdentifier("YourView", new Dictionary<string, string>{["one"] = "1"});

        //Assert
        Assert.That(one == two, Is.False);
        Assert.That(one.Equals(two), Is.False);
        Assert.That(two == one, Is.False);
        Assert.That(two != one, Is.True);
        Assert.That(one != two, Is.True);
    }
    
    [Test]
    public void IF_names_different_FALSE()
    {
        //Arrange
        var one = new ViewIdentifier("MyView");
        var two = new ViewIdentifier("YourView");

        //Assert
        Assert.That(one == two, Is.False);
        Assert.That(one.Equals(two), Is.False);
        Assert.That(two == one, Is.False);
        Assert.That(two != one, Is.True);
        Assert.That(one != two, Is.True);
    }
}