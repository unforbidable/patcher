using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    public interface IConditionsHelper
    {
        ICondition GenericFunction(int number);
        ICondition GenericFunction(int number, object paramA);
        ICondition GenericFunction(int number, object paramA, object paramB);
    }
}
