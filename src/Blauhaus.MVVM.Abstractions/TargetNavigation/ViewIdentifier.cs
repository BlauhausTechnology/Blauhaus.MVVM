using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public class ViewIdentifier : BaseValueObject<ViewIdentifier>, IHasName, IHasProperties
{
    public ViewIdentifier(string name, Dictionary<string, string>? properties = null)
    {
        Name = name;
        Properties = properties ?? new Dictionary<string, string>();
        

    }

    public string Name { get; }
    public Dictionary<string, string> Properties { get; }

    public static ViewIdentifier Create([CallerMemberName] string name = "") => new(name);
    
    
    #region Equality
    public override string ToString()
    {
        var uri = new StringBuilder()
            .Append('/').Append(Name);
        
        if (Properties.Count > 0)
        {
            uri.Append('?');
            foreach (var property in Properties)
            {
                uri.Append(property.Key).Append('=').Append(property.Value).Append('&');
            }

            //remove last &
            uri.Length -= 1;
        }

        return uri.ToString();;
    }

    protected override int GetHashCodeCore()
    {
        return ToString().GetHashCode();
    }

    protected override bool EqualsCore(ViewIdentifier other)
    {
        return ToString().Equals(other.ToString());
    }

    #endregion
}