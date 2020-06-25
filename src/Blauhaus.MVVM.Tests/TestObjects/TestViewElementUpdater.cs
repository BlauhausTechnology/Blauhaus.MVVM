using Blauhaus.MVVM.Xamarin.ViewElements.Sync;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestViewElementUpdater : IModelViewElementUpdater<TestModel, TestListItem>
    {
        public TestListItem Update(TestModel model, TestListItem element)
        {
            element.Name = model.Name;
            return element;
        }
    }
}