using System;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Views.MasterDetail;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestMenuView : BaseMenuPage<TestViewModel>, IView
    {
        public TestMenuView(TestViewModel viewModel) : base(viewModel, "Menu")
        {
            ViewId = Guid.NewGuid().ToString();
        }

        public string ViewId { get; set; }
    }
}