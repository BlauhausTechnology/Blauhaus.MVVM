using System.Runtime.CompilerServices;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ValueProperties.Base;

namespace Blauhaus.MVVM.ValueProperties;

public class StringValueProperty : BaseValueProperty<string?>
{
    
    protected StringValueProperty(IPropertyChanger viewModel, string name, string? value, bool isVisible) : base(viewModel, name, value, isVisible)
    {
    }

    public static StringValueProperty Create(IPropertyChanger viewModel, string? value = null, [CallerMemberName] string name = "")
    {
        return new StringValueProperty(viewModel, name, value, true);
    }
    public static StringValueProperty CreateInvisible(IPropertyChanger viewModel, string? value = null, [CallerMemberName] string name = "")
    {
        return new StringValueProperty(viewModel, name, value, false);
    }

}