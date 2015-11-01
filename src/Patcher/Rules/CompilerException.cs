using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules
{
    public sealed class CompilerException : Exception
    {
        public CompilerErrorCollection Errors { get; private set; }

        public CompilerException(string message, CompilerErrorCollection errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}
