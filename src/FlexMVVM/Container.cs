using System;
using System.Collections.Generic;

namespace FlexMVVM
{
    public static class NameContainer
    {
        public static Dictionary<string, Type> RegisterType = new Dictionary<string, Type> ();

        public static Type RootLayout { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }
    }
}
