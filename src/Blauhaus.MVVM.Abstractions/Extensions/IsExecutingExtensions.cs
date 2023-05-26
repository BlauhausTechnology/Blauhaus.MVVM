using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using System.Collections.Generic;
using System.Reflection;

namespace Blauhaus.MVVM.Abstractions.Extensions;

public static class IsExecutingExtensions
{
    public static List<IExecutingCommand?> GetExecutingCommands(this IIsExecuting obj)
    {
        var commands = new List<IExecutingCommand?>();
        var type = obj.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (typeof(IExecutingCommand).IsAssignableFrom(property.PropertyType) && property.GetGetMethod().IsPublic)
            {
                commands.Add(property.GetValue(obj) as IExecutingCommand);
            }
        }

        return commands;
    }

    public static IExecutingCommand? GetCommand(this PropertyInfo property, object obj)
    {
        if (typeof(IExecutingCommand).IsAssignableFrom(property.PropertyType) && property.GetGetMethod().IsPublic)
        {
            return property.GetValue(obj) as IExecutingCommand;
        }

        return null;
    }
    public static PropertyInfo[] GetExecutingCommandProperties(this IIsExecuting obj)
    {
        var commands = new List<PropertyInfo>();
        var type = obj.GetType();
        var properties = type.GetProperties();
        
        return properties;
    }
}