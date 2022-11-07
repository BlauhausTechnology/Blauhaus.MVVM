using System.Runtime.CompilerServices;
using Blauhaus.MVVM.ValueProperties.Base;

namespace Blauhaus.MVVM.ValueProperties;

public class DecimalValueProperty : BaseValueProperty<decimal?>
{
    protected DecimalValueProperty(string name, decimal? value, bool isVisible = true) : base(name, value, isVisible)
    {
    }

    public static DecimalValueProperty Create(decimal? value = null, [CallerMemberName] string name = "")
    {
        return new DecimalValueProperty(name, value, true);
    }
    public static DecimalValueProperty CreateInvisible(decimal? value = null, [CallerMemberName] string name = "")
    {
        return new DecimalValueProperty(name, value, false);
    }
}