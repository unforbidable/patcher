using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields
{
    /// <summary>
    /// Represents a color definition.
    /// </summary>
    /// <remarks>
    /// The valid value range of color components is <code>0.0f</code> through <code>1.0f</code>.
    /// </remarks>
    public interface IColor 
    {
        /// <summary>
        /// Gets or sets the red component value.
        /// </summary>
        float Red { get; set; }
        /// <summary>
        /// Gets or sets the green component value.
        /// </summary>
        float Green { get; set; }
        /// <summary>
        /// Gets or sets the blue component value.
        /// </summary>
        float Blue { get; set; }
    }
}
