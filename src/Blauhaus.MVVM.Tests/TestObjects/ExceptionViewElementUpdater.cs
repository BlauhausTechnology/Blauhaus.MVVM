using System;
using Blauhaus.MVVM.Xamarin.ViewElements.Sync;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class ExceptionViewElementUpdater : IModelViewElementUpdater<TestModel, TestListItem>
    {
        public TestListItem Update(TestModel model, TestListItem element)
        {
            throw new Exception("This is an exceptionally bad thing that just happened");
        }
    }
}