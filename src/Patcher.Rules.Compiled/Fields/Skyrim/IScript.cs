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

using Patcher.Rules.Compiled.Constants;
using Patcher.Rules.Compiled.Forms;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    public interface IScript
    {
        string Name { get; }

        IScript AddProperty(string name, Types type);
        IScript SetProperty(string name, int value);
        IScript SetProperty(string name, string value);
        IScript SetProperty(string name, float value);
        IScript SetProperty(string name, bool value);
        IScript SetProperty(string name, IForm value);
        IScript SetProperty(string name, int value, int? index);
        IScript SetProperty(string name, string value, int? index);
        IScript SetProperty(string name, float value, int? index);
        IScript SetProperty(string name, bool value, int? index);
        IScript SetProperty(string name, IForm value, int? index);
        IScript ResetProperty(string name);
    }
}
