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

using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    public interface IArmo : IForm
    {
        IScriptCollection Scripts { get; set; }
        IObjectBounds ObjectBounds { get; set; }
        string FullName { get; set; }
        IForm Enchantment { get; set; }
        string MaleWorldModel { get; set; }
        string FemaleWorldModel { get; set; }
        Skill Skill { get; set; }
        BodyNodes BodyNodes { get; set; }
        bool IsPlayable { get; set; }
        bool IsShield { get; set; }
        IForm PickUpSound { get; set; }
        IForm PutDownSound { get; set; }
        IForm EquipType { get; set; }
        IForm BlockImpactDataSet { get; set; }
        IForm AlternateBlockMaterial { get; set; }
        IForm Race { get; set; }
        IFormCollection<IKywd> Keywords { get; set; }
        string Description { get; set; }
        IFormCollection<IForm> Models { get; set; }
        int Value { get; set; }
        float Weight { get; set; }
        float ArmorRating { get; set; }
        IArmo TemplateArmor { get; set; }
    }
}
