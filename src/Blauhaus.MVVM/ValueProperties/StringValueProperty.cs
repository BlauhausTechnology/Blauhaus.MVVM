using System.Runtime.CompilerServices;
using Blauhaus.MVVM.ValueProperties.Base;

namespace Blauhaus.MVVM.ValueProperties;

public class StringValueProperty : BaseValueProperty<string?>
{
    
    protected StringValueProperty(string name, string? value, bool isVisible) : base(name, value, isVisible)
    {
    }

    public static StringValueProperty Create(string? value = null, [CallerMemberName] string name = "")
    {
        return new StringValueProperty(name, value, true);
    }
    public static StringValueProperty CreateInvisible(string? value = null, [CallerMemberName] string name = "")
    {
        return new StringValueProperty(name, value, false);
    }

}