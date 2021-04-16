using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;

namespace Blauhaus.MVVM.ExecutingCommands.Base
{
    internal static class InternalExtensions
    {
        internal static async Task<bool> TryHandle(this Dictionary<Error, Func<Error, Task>>? errorHandlers, string errorMessage)
        {
            if (errorHandlers != null && (errorMessage.IsError(out var error)))
            {
                if (errorHandlers.TryGetValue(error, out var errorHandler))
                {
                    await errorHandler.Invoke(error);
                    return true;
                }
            }
            return false;
        }

        internal static async Task<bool> TryHandle(this Dictionary<Error, Func<Error, Task>>? errorHandlers, Error error)
        {
            if (errorHandlers != null)
            {
                if (errorHandlers.TryGetValue(error, out var errorHandler))
                {
                    await errorHandler.Invoke(error);
                    return true;
                }
            }
            return false;
        }
    }
}