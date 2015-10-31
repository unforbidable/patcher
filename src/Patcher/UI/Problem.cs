using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.UI
{
    public class Problem
    {
        public string Message { get; set; }
        public string File { get; set; }
        public int Line { get; set; }
        public string Solution { get; set; }
    }
}
