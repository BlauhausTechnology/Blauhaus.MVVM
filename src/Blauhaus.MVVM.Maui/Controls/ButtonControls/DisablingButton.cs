namespace Blauhaus.MVVM.Maui.Controls.ButtonControls;

public partial class DisablingButton : Button
{
    public Color? EnabledBackgroundColour { get; set; } 
    public Color? EnabledTextColour{ get; set; }

    public Color? DisabledBackgroundColour{ get; set; }
    public Color? DisabledTextColour{ get; set; }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName); 

        if (EnabledBackgroundColour is null || EnabledTextColour is null)
        {
            return;
        }

        DisabledBackgroundColour ??= EnabledBackgroundColour.WithAlpha(0.75f);
        DisabledTextColour ??= EnabledTextColour.WithAlpha(0.75f);

        if (propertyName is nameof(IsEnabled))
        {
            if (IsEnabled)
            { 
                BackgroundColor = EnabledBackgroundColour;
                TextColor = EnabledTextColour;
            }
            else
            {
                BackgroundColor = DisabledBackgroundColour;
                TextColor = DisabledTextColour;
            }
        }

      
    }
     
     
}