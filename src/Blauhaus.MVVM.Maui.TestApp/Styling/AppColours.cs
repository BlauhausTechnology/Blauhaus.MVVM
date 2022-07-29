namespace Blauhaus.MVVM.Maui.TestApp.Styling;

public class AppColours
{
    public static bool IsDarkMode { get; set; } = true;

    public static Color White = Color.FromRgb(255, 255, 255);
    public static Color WhiteFaded = White.WithAlpha(0.7f);
    public static Color Black = Color.FromRgb(0, 0, 0);
    public static Color BlackFaded = Black.WithAlpha(0.7f);
    public static Color Transparent = White.WithAlpha(0f);

    public static Color PrussianBlue = Color.FromRgb(28, 49, 68);
    public static Color GhostWhite = Color.FromRgb(247, 247, 255);
    public static Color BurntUmber = Color.FromRgb(144, 41, 35);
    public static Color RichBlack = Color.FromRgb(3, 26, 42);
    public static Color PolishedPine = Color.FromRgb(81, 158, 138);

    public static Color Background => IsDarkMode ? RichBlack : GhostWhite;
    public static Color TextOnBackground => IsDarkMode ? GhostWhite : RichBlack;
    public static Color TextFadedOnBackground => IsDarkMode ? WhiteFaded : BlackFaded;

    public static Color Accent => PolishedPine;
    public static Color TextOnAccent => White;
    public static Color TextFadedOnAccent => WhiteFaded;
}