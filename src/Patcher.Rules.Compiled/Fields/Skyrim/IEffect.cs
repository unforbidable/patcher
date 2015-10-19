using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    public interface IEffect
    {
        IForm BaseEffect { get; set; }
        float Magnitude { get; set; }
        int Area { get; set; }
        int Duration { get; set; }
        IConditionCollection Conditions { get; set; }
    }
}
