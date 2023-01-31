namespace Blauhaus.MVVM.Maui.Styling;

public interface IAppColours
{
    Color Primary { get; }
    Color PrimaryFaded { get; }
    Color OnPrimary { get; }
    Color OnPrimaryFaded { get; }

    Color Background { get; }
    Color BackgroundFaded { get; }
    Color OnBackground { get; }
    Color OnBackgroundFaded { get; }
}