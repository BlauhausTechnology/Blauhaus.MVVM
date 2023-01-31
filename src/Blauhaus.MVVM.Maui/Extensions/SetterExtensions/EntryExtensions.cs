using static Blauhaus.MVVM.Maui.Styling.AppStyling;

namespace Blauhaus.MVVM.Maui.Extensions.SetterExtensions;

public static class EntryExtensions
{ 

    public static T WithPlaceholder<T>(this T control, string placeHolder) where T : Entry
    {
        control.Placeholder = placeHolder;
        return control;
    }
    public static T WithKeyboard<T>(this T control, KeyboardFlags keyboards) where T : Entry
    {
        control.Keyboard = Keyboard.Create(keyboards);
        return control;
    }
    public static T WithKeyboard<T>(this T control, Keyboard keyboard) where T : Entry
    {
        control.Keyboard = keyboard;
        return control;
    }
     
}