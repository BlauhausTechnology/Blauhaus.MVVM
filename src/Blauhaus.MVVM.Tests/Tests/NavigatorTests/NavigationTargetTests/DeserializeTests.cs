
using Blauhaus.MVVM.Abstractions.TargetNavigation;

namespace Blauhaus.MVVM.Tests.Tests.NavigatorTests.NavigationTargetTests;

public class DeserializeTests
{

    private ViewIdentifier _viewOne = null!;

    [SetUp]
    public void Setup()
    {
        _viewOne = new ViewIdentifier("ViewOne");
    }

    [Test]
    public void SHOULD_combine_paths_and_View_ignoring_container()
    {
        //Arrange
        var sut = NavigationTarget.CreateContainer(new ViewIdentifier("Container"))
            .WithPath("Home", "Choices")
            .WithView(new ViewIdentifier("Content"))
            .WithViewProperty("key1", "value1")
            .WithViewProperty("key2", "value2");
        
        //Act
        var result = NavigationTarget.Deserialize(sut.Serialize());

        //Assert
        Assert.That(result, Is.EqualTo(sut));
    }

    [Test]
    public void SHOULD_ignore_Path_if_not_exist()
    {
        //Arrange
        var sut = NavigationTarget.CreateContainer(new ViewIdentifier("Container"))
            .WithView(new ViewIdentifier("Content"))
            .WithViewProperty("key1", "value1")
            .WithViewProperty("key2", "value2");
        
        //Act
        var result = NavigationTarget.Deserialize(sut.Serialize());

        //Assert
        Assert.That(result, Is.EqualTo(sut));
    }

    [Test]
    public void SHOULD_ignore_View_if_not_exist()
    {
        //Arrange
        var sut = NavigationTarget.CreateContainer(new ViewIdentifier("Container"))
            .WithPath("Home", "Choices");
        
        //Act
        var result = NavigationTarget.Deserialize(sut.Serialize());

        //Assert
        Assert.That(result, Is.EqualTo(sut));
    }
		
}