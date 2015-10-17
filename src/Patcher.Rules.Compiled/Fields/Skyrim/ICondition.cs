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
    public interface ICondition
    {
        ICondition IsTrue();
        ICondition IsFalse();
        ICondition IsEqualTo(float value);
        ICondition IsEqualTo(IGlob glob);
        ICondition IsNotEqualTo(float value);
        ICondition IsNotEqualTo(IGlob glob);
        ICondition IsLessThen(float value);
        ICondition IsLessThen(IGlob glob);
        ICondition IsGreaterThan(float value);
        ICondition IsGreaterThan(IGlob glob);
        ICondition IsLessThenOrEqualTo(float value);
        ICondition IsLessThenOrEqualTo(IGlob glob);
        ICondition IsGreaterThanOrEqualTo(float value);
        ICondition IsGreaterThanOrEqualTo(IGlob glob);
        ICondition RunOn(IForm reference);
        ICondition RunOnCombatTarget();
        ICondition RunOnPlayer();
        ICondition RunOnSubject();
        ICondition RunOnTarget();
        ICondition SwapSubjectAndTarget();
        ICondition OrNext();
        ICondition AndNext();
    }
}
