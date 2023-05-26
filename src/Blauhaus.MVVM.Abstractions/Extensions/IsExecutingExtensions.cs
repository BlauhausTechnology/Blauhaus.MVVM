﻿using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using System.Collections.Generic;

namespace Blauhaus.MVVM.Abstractions.Extensions;

public static class IsExecutingExtensions
{
    public static List<IExecutingCommand> GetExecutingCommands(this IIsExecuting obj)
    {
        var props = new List<IExecutingCommand>();

        var type = obj.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (typeof(IExecutingCommand).IsAssignableFrom(property.PropertyType) && property.GetGetMethod()!.IsPublic)
            {
                props.Add((IExecutingCommand)property.GetValue(obj)!);
            }
        }

        return props;
    }
}