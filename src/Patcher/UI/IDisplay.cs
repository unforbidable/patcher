using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Logging;

namespace Patcher.UI
{
    public interface IDisplay
    {
        void StartProgress(Progress progess);
        Choice OfferChoice(string message, Choice[] choice);
        void WriteText(string text);
    }
}
