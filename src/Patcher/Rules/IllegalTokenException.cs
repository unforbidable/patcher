using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules
{
    public sealed class IllegalTokenException : Exception
    {
        public IllegalTokenException()
        {
        }

        public IllegalTokenException(string message)
            : base(message)
        {
        }
    }
}
