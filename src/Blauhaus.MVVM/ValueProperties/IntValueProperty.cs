using System.Runtime.CompilerServices;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ValueProperties.Base;

namespace Blauhaus.MVVM.ValueProperties;

public class IntValueProperty : BaseValueProperty<int?>
{
     
    protected IntValueProperty(IPropertyChanger viewModel, string name, int? value, bool isVisible = true) : base(viewModel, name, value, isVisible)
    {
    }

    public static IntValueProperty Create(IPropertyChanger viewModel, int? value = null, [CallerMemberName] string name = "")
    {
        return new IntValueProperty(viewModel, name, value, true);
    }
    public static IntValueProperty CreateInvisible(IPropertyChanger viewModel, int? value = null, [CallerMemberName] string name = "")
    {
        return new IntValueProperty(viewModel, name, value, false);
    }

}