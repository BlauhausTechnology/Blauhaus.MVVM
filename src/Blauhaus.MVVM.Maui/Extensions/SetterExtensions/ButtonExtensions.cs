using Blauhaus.MVVM.Maui.Controls.ButtonControls;
using static Blauhaus.MVVM.Maui.Styling.AppStyling;

namespace Blauhaus.MVVM.Maui.Extensions.SetterExtensions;

public static class ButtonExtensions
{
    public static T WithText<T>(this T control, string text) where T : Button
    {
        control.Text = text;
        return control;
    }
     

}