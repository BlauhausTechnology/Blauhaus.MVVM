using System.Runtime.CompilerServices;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.ValueProperties.Base;

namespace Blauhaus.MVVM.ValueProperties;

public class DecimalValueProperty : BaseValueProperty<decimal?>
{
    protected DecimalValueProperty(IPropertyChanger viewModel, string name, decimal? value, bool isVisible = true) : base(viewModel, name, value, isVisible)
    {
    }

    public static DecimalValueProperty Create(IPropertyChanger viewModel, decimal? value = null, [CallerMemberName] string name = "")
    {
        return new DecimalValueProperty(viewModel, name, value, true);
    }
    public static DecimalValueProperty CreateInvisible(IPropertyChanger viewModel, decimal? value = null, [CallerMemberName] string name = "")
    {
        return new DecimalValueProperty(viewModel, name, value, false);
    }
}