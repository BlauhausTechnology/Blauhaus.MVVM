using System;
using Blauhaus.Errors;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public static class TestErrors
    {
        public static Error Fail() => Error.Create("An error occured with some random property " + Guid.NewGuid());
    }
}