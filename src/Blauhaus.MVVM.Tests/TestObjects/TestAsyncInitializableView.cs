using Blauhaus.MVVM.Xamarin.Views.Content;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestAsyncInitializableView : BasePage<TestAsyncInitializableViewModel>
    {
        public TestAsyncInitializableView(TestAsyncInitializableViewModel viewModel) : base(viewModel)
        {
        }
    }
}