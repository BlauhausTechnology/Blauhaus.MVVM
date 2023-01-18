namespace Blauhaus.MVVM.Maui.Controls.EntryControls;

public partial class PaddedEntry : Entry
{
    public PaddedEntry(int padding = 20)
    {
        Padding = padding;
    }

    public int Padding { get; }
}