using System.Runtime.CompilerServices;
using Blauhaus.MVVM.ValueProperties.Base;

namespace Blauhaus.MVVM.ValueProperties;

public class IntValueProperty : BaseValueProperty<int?>
{
     
    protected IntValueProperty(string name, int? value, bool isVisible = true) : base(name, value, isVisible)
    {
    }

    public static IntValueProperty Create(int? value = null, [CallerMemberName] string name = "")
    {
        return new IntValueProperty(name, value, true);
    }
    public static IntValueProperty CreateInvisible(int? value = null, [CallerMemberName] string name = "")
    {
        return new IntValueProperty(name, value, false);
    }

}