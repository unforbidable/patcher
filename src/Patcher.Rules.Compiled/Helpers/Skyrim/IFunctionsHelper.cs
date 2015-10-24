/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers.Skyrim
{
    /// <summary>
    /// Provides methods that can be used to create new <see cref="ICondition"/> based on Papyrus functions. 
    /// <see cref="ICondition">Conditions</see> created with these methods can be added to a <see cref="IConditionCollection"/>.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Methods <code>GenericFunction()</code> can be used to create a new condition based on basically any Papyrus function 
    /// when the proper function number and appropriate arguments are provided. 
    /// The engine knows what arguments every function requires and will issue a warning if an incorrect number or types of arguments is specified.
    /// </p>
    /// <p>
    /// Specific methods that create conditions based on commonly used functions have been added and eventually more functions will be added. 
    /// </p>
    /// </remarks>
    public interface IFunctionsHelper
    {
        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on Papyrus function that has the specified number and no parameters. 
        /// </summary>
        /// <param name="number">Number of the function.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GenericFunction(int number);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on Papyrus function that has the specified number and one parameter.
        /// </summary>
        /// <param name="number">Number of the function.</param>
        /// <param name="paramA">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GenericFunction(int number, object paramA);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on Papyrus function that has the specified number and two parameters.
        /// </summary>
        /// <param name="number">Number of the function.</param>
        /// <param name="paramA">First parameter.</param>
        /// <param name="paramB">Second parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GenericFunction(int number, object paramA, object paramB);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>EPTemperingItemIsEnchanted</b> Papyrus function.
        /// </summary>
        /// <returns>Returns new condition.</returns>
        ICondition EPTemperingItemIsEnchanted();

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetCurrentTime</b> Papyrus function.
        /// </summary>
        /// <returns>Returns new condition.</returns>
        ICondition GetCurrentTime();

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetGlobalValue</b> Papyrus function.
        /// </summary>
        /// <param name="global">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GetGlobalValue(IForm global);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetInCurrentLoc</b> Papyrus function.
        /// </summary>
        /// <param name="location">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GetInCurrentLoc(IForm location);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetItemCount</b> Papyrus function.
        /// </summary>
        /// <param name="item">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GetItemCount(IForm item);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetQuestCompleted</b> Papyrus function.
        /// </summary>
        /// <param name="quest">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GetQuestCompleted(IForm quest);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetStageDone</b> Papyrus function.
        /// </summary>
        /// <param name="quest">First parameter.</param>
        /// <param name="stage">Second parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GetStageDone(IForm quest, int stage);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>GetVMQuestVariable</b> Papyrus function.
        /// </summary>
        /// <param name="quest">First parameter.</param>
        /// <param name="variable">Second parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition GetVMQuestVariable(IForm quest, string variable);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>HasKeyword</b> Papyrus function.
        /// </summary>
        /// <param name="keyword">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition HasKeyword(IForm keyword);

        /// <summary>
        /// Creates a new <see cref="ICondition"/> based on the <b>HasPerk</b> Papyrus function.
        /// </summary>
        /// <param name="perk">First parameter.</param>
        /// <returns>Returns new condition.</returns>
        ICondition HasPerk(IForm perk);

    }
}
