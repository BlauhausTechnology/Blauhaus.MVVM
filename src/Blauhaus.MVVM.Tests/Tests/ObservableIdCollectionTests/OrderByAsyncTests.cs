using System.Threading.Tasks;
using Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests.Base;

namespace Blauhaus.MVVM.Tests.Tests.ObservableIdCollectionTests;

public class OrderByAsyncTests : BaseObservableIdCollectionTest<long>
{
    public OrderByAsyncTests(): base(new long []{1,2,3})
    {
    }

    [Test]
    public async Task SHOULD_sort_items()
    {
        //Arrange
        await Sut.UpdateAsync(new[] {Ids[0], Ids[1], Ids[2]});

        //Act
        await Sut.OrberByAsync(x => x.Index);

        //Assert
        Assert.That(Sut.Count, Is.EqualTo(3));
        Assert.That(Sut[0].Index, Is.EqualTo(1)); 
        Assert.That(Sut[1].Index, Is.EqualTo(2)); 
        Assert.That(Sut[2].Index, Is.EqualTo(3)); 
    }

    [Test]
    public async Task SHOULD_sort_items_descending()
    {
        //Arrange
        await Sut.UpdateAsync(new[] {Ids[0], Ids[1], Ids[2]});

        //Act
        await Sut.OrberByAsync(x => x.Index, false);

        //Assert
        Assert.That(Sut.Count, Is.EqualTo(3));
        Assert.That(Sut[0].Index, Is.EqualTo(3)); 
        Assert.That(Sut[1].Index, Is.EqualTo(2)); 
        Assert.That(Sut[2].Index, Is.EqualTo(1)); 
    }
}