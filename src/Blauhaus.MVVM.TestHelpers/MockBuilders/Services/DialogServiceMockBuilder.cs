using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class DialogServiceMockBuilder : BaseMockBuilder<DialogServiceMockBuilder, IDialogService>
    {
        public void Verify_DisplayAlert_called(string title, string message, string accept)
        {
            Mock.Verify(x => x.DisplayAlertAsync(title, message, accept));
        }
        public void Verify_DisplayAlert_called(string title, string message)
        {
            Mock.Verify(x => x.DisplayAlertAsync(title, message, It.IsAny<string>()));
        }
        public void Verify_DisplayAlert_NOT_called(string title, string message, string accept)
        {
            Mock.Verify(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}