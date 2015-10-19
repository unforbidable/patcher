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
    public interface IAlch : IForm
    {
        IObjectBounds ObjectBounds { get; set; }
        string FullName { get; set; }
        IFormCollection<IKywd> Keywords { get; set; }
        string Description { get; set; }
        string WorldModel { get; set; }
        IForm PickUpSound { get; set; }
        IForm PutDownSound { get; set; }
        IForm UseSound { get; set; }
        float Weight { get; set; }
        int Value { get; set; }
        PotionType Type { get; set; }
        IEffectCollection Effects { get; set; }
    }
}