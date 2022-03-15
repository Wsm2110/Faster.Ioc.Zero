using System;
using System.Collections.Generic;
using System.Text;

namespace Faster.Ioc.Zero.SourceGenerator
{
    public static class Declarations
    {

        public const string Factory = @"
using System;

namespace {0} 
{{ 

    public static partial class Container
    {{

            private static void TryDispose(object? value) => (value as IDisposable)?.Dispose();   

            static Container()
            {{
            }}

    }}
}}";

    }
}
