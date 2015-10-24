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

using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a function with parameters, operand, operator used to compare the result of the function with the specified operand 
    /// and information on which object the function will be run.
    /// </summary>
    /// <remarks>
    /// <p>
    /// When a condition is created with any of the <see cref="Helpers.Skyrim.IFunctionsHelper"/> helper methods its default state is as if the following methods where used: 
    /// <code>RunOnSubject()</code>, <code>IsEqualTo(0)</code> and <code>AndNext()</code>. 
    /// Calling these methods on a new conditions will have no effect however these methods can still be used to modify an existing conditions.
    /// </p>
    /// <p>
    /// Method <code>RunOnPlayer()</code> is equivalent to <code>RunOn(Forms.Find(0x14))</code> 
    /// however many functions will run on the player reference by default as the player is often the subject. 
    /// </p>
    /// <p>
    /// Methods can be used in any order and when multiple methods that change the same aspect are called the changes made by the last method will be kept.
    /// </p>
    /// <p>
    /// Methods can be chained together into a single statement in a fluent interface fashion.
    /// </p>
    /// </remarks>
    public interface ICondition
    {
        /// <summary>
        /// Sets the operator to <code>==</code> and the operand to <code>1</code>.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition IsTrue();

        /// <summary>
        /// Sets the operator to <code>!=</code> and the operand to <code>1</code>.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition IsFalse();

        /// <summary>
        /// Sets the operator to <code>==</code> and the operand to the specified value.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition IsEqualTo(float value);

        /// <summary>
        /// Sets the operator to <code>==</code> and the operand to the specified <b>Global Variable</b>.
        /// </summary>
        /// <param name="glob"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsEqualTo(IForm glob);

        /// <summary>
        /// Sets the operator to <code>!=</code> and the operand to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsNotEqualTo(float value);

        /// <summary>
        /// Sets the operator to <code>!=</code> and the operand to the specified <b>Global Variable</b>.
        /// </summary>
        /// <param name="glob"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsNotEqualTo(IForm glob);

        /// <summary>
        /// Sets the operator to <code>&lt;</code> and the operand to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsLessThen(float value);

        /// <summary>
        /// Sets the operator to <code>&lt;</code> and the operand to the specified <b>Global Variable</b>.
        /// </summary>
        /// <param name="glob"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsLessThen(IForm glob);

        /// <summary>
        /// Sets the operator to <code>&gt;</code> and the operand to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsGreaterThan(float value);

        /// <summary>
        /// Sets the operator to <code>&gt;</code> and the operand to the specified <b>Global Variable</b>.
        /// </summary>
        /// <param name="glob"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsGreaterThan(IForm glob);

        /// <summary>
        /// Sets the operator to <code>&lt;=</code> and the operand to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsLessThenOrEqualTo(float value);

        /// <summary>
        /// Sets the operator to <code>&lt;=</code> and the operand to the specified <b>Global Variable</b>.
        /// </summary>
        /// <param name="glob"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsLessThenOrEqualTo(IForm glob);

        /// <summary>
        /// Sets the operator to <code>&gt;=</code> and the operand to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsGreaterThanOrEqualTo(float value);

        /// <summary>
        /// Sets the operator to <code>&gt;=</code> and the operand to the specified <b>Global Variable</b>.
        /// </summary>
        /// <param name="glob"></param>
        /// <returns>Returns this condition.</returns>
        ICondition IsGreaterThanOrEqualTo(IForm glob);

        /// <summary>
        /// Causes the function to run on the specified form reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>Returns this condition.</returns>
        ICondition RunOn(IForm reference);

        /// <summary>
        /// Causes the function to run on the current combat target.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition RunOnCombatTarget();

        /// <summary>
        /// Causes the function to run on the player character.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition RunOnPlayer();

        /// <summary>
        /// Causes the function to run on the subject. This is the default behavior.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition RunOnSubject();

        /// <summary>
        /// Causes the function to run on the target.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition RunOnTarget();

        /// <summary>
        /// Causes this condition to swap the subject and the target before it is evaluated.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition SwapSubjectAndTarget();

        /// <summary>
        /// Indicates that logical <code>or</code> operation will be performed between this and the consecutive condition when the <see cref="IConditionCollection"/> is evaluated.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition OrNext();

        /// <summary>
        /// Indicates that logical <code>and</code> operation will be performed between this and the consecutive condition when the <see cref="IConditionCollection"/> is evaluated.
        /// </summary>
        /// <returns>Returns this condition.</returns>
        ICondition AndNext();
    }
}
