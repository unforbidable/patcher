using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Patcher.UI.Windows
{
    sealed class WindowPrompt : Prompt
    {
        readonly WindowDisplay display;

        public WindowPrompt(WindowDisplay display)
        {
            this.display = display;
        }

        protected override ChoiceOption OfferChoice(string message, ChoiceOption[] options)
        {
            return display.Window.OfferChoice(message, options);
        }
    }
}
