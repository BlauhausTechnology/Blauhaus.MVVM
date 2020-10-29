using System;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Views.Content;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestView : BaseContentPage<TestViewModel>, IView
    {
        public TestView(TestViewModel viewModel) : base(viewModel)
        {
            ViewId = Guid.NewGuid().ToString();
        }

        public string ViewId { get; set; }
    }
}