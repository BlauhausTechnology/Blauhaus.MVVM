using System;
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

    public string Serialize() => ToString();

    public static ViewIdentifier Deserialize(string serialized)
    {
        if (serialized.StartsWith("/"))
        {
            serialized = serialized.TrimStart('/');
        }

        var split = serialized.Split('?');
        if (split.Length == 0)
        {
            throw new InvalidOperationException($"{serialized} is not a valid serialized ViewIdentifier");
        }
        var name = split[0];
        Dictionary<string, string>? properties = null;

        if (split.Length > 1)
        {
            properties = new Dictionary<string, string>();
            var queryString = split[1];
            var queryStringParameters = queryString.Split('&');
            foreach (var queryStringParameter in queryStringParameters)
            {
                var kvp = queryStringParameter.Split('=');
                properties[kvp[0]] = kvp[1];
            }
        }
   
        return new ViewIdentifier(name, properties);
    }
    
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