// ReSharper disable CheckNamespace

using AndroidX.AppCompat.Widget;

namespace Blauhaus.MVVM.Maui.Controls.EntryControls;

public partial class PaddedEntry
{
    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        base.OnHandlerChanging(args);

        if (args.NewHandler.PlatformView is AppCompatEditText editText)
        {
            editText.SetPadding(Padding, Padding, Padding, Padding);
        }
    }
}