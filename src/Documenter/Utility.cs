using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documenter
{
    static class Utility
    {
        public static string GetLocalNamespace(this Type type)
        {
            return type.Namespace.Substring(Program.RootNamespace.Length + 1);
        }

        public static string GetLocalName(this Type type)
        {
            if (type.Namespace.Contains(".Helpers"))
            {
                return type.Name.TrimStart('I').Replace("Helper", string.Empty);
            }
            else if (type.Namespace.Contains(".Forms") || type.Namespace.Contains(".Fields"))
            {
                return type.Name.TrimStart('I').Replace("`1", string.Empty);
            }
            else
            {
                return type.Name;
            }
        }
    }
}
