using System;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestInitializingView : BaseContentPage<TestInitializingViewModel>, IView
    {
        public TestInitializingView(TestInitializingViewModel viewModel) : base(viewModel)
        {
            ViewId = Guid.NewGuid().ToString();
        }

        public string ViewId { get; set; }
    }
}