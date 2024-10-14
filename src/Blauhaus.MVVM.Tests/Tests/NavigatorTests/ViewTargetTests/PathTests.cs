using Blauhaus.Common.Abstractions.Extensions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Navigation;

namespace Blauhaus.MVVM.Tests.Tests.NavigatorTests.ViewTargetTests;

public class PathTests
{
     
    [Test]
    public void SHOULD_combine_properties_for_views()
    {
        //Arrange
        var sut = ViewTarget.Create(
                new ViewIdentifier("Container")
                    .WithValue("key1", "value1"),
                new ViewIdentifier("Content")
                    .WithValue("key2", "value2")
                    .WithValue("key3", "value3"));
        
        //Act
        string result = sut.Path;

        //Assert
        Assert.That(result, Is.EqualTo("/Container?key1=value1/Content?key2=value2&key3=value3"));
    }
     
		
}