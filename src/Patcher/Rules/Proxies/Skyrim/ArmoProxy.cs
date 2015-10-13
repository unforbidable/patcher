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

using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Objects.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Compiled.Objects;
using Patcher.Rules.Compiled.Constants.Skyrim;

namespace Patcher.Rules.Proxies.Skyrim
{
    [Proxy(typeof(IArmo))]
    public sealed class ArmoProxy : FormProxy<Armo>, IArmo
    {
        public IScriptCollection Scripts { get { return ScriptsToProxy(record.VirtualMachineAdapter); } set { record.VirtualMachineAdapter = ProxyToScripts(value); } }
        public IObjectBounds ObjectBounds { get { return ObjectBoundsToProxy(record.ObjectBounds); } set { record.ObjectBounds = ProxyToObjectBounds(value); } }
        public string FullName { get { EnsureReadable(); return record.FullName; } set { EnsureWritable(); record.FullName = value; } }
        public IForm Enchantment { get { return ReferenceToProxy<IForm>(record.Enchantment); } set { record.Enchantment = ProxyToReference(value); } }
        public string MaleWorldModel { get { EnsureReadable(); return record.MaleWorldModel; } set { EnsureWritable(); record.MaleWorldModel = value; } }
        public string FemaleWorldModel { get { EnsureReadable(); return record.FemaleWorldModel; } set { EnsureWritable(); record.FemaleWorldModel = value; } }
        public Skill Skill { get { return record.SkillUsage.ToSkill(); } set { record.SkillUsage = value.ToArmorSkillUsage(); } }
        public BodyNodes BodyNodes { get { return record.BodyParts.ToBodyNodes(); } set { EnsureWritable(); record.BodyParts = value.ToBodyParts(); } }
        public bool IsPlayable { get { EnsureReadable(); return record.IsPlayable; } set { EnsureWritable(); record.IsPlayable = value; } }
        public bool IsShield { get { EnsureReadable(); return record.IsShield; } set { EnsureWritable(); record.IsShield = value; } }
        public IForm PickUpSound { get { return ReferenceToProxy<IForm>(record.PickUpSound); } set { EnsureWritable(); record.PickUpSound = ProxyToReference(value); } }
        public IForm PutDownSound { get { return ReferenceToProxy<IForm>(record.PutDownSound); } set { record.PutDownSound = ProxyToReference(value); } }
        public IForm EquipType { get { return ReferenceToProxy<IForm>(record.EquipType); } set { record.EquipType = ProxyToReference(value); } }
        public IForm BlockImpactDataSet { get { return ReferenceToProxy<IForm>(record.BlockImpactDataSet); } set { record.BlockImpactDataSet = ProxyToReference(value); } }
        public IForm AlternateBlockMaterial { get { return ReferenceToProxy<IForm>(record.AlternateBlockMaterial); } set { record.AlternateBlockMaterial = ProxyToReference(value); } }
        public IForm Race { get { return ReferenceToProxy<IForm>(record.EquipType); } set { record.EquipType = ProxyToReference(value); } }
        public IFormCollection<IKywd> Keywords { get { return ReferenceListToProxy<IKywd>(record.Keywords.Items); } set { record.Keywords.Items = ProxyToReferenceList(value); } }
        public string Description { get { EnsureReadable(); return record.Description; } set { EnsureWritable(); record.Description = value; } }
        public IFormCollection<IForm> Models { get { return ReferenceListToProxy<IForm>(record.Models); } set { record.Models = ProxyToReferenceList(value); } }
        public int Value { get { EnsureReadable(); return record.Value; } set { EnsureWritable(); record.Value = value; } }
        public float Weight { get { EnsureReadable(); return record.Weight; } set { EnsureWritable(); record.Weight = value; } }
        public float ArmorRating { get { EnsureReadable(); return record.ArmorRating; } set { EnsureWritable(); record.ArmorRating = value; } }
        public IArmo TemplateArmor { get { return ReferenceToProxy<IArmo>(record.TemplateArmor); } set { record.TemplateArmor = ProxyToReference(value); } }

        IScriptCollection ScriptsToProxy(VirtualMachineAdapter objectBounds)
        {
            EnsureReadable();
            ScriptCollectionProxy proxy = Provider.CreateProxy<ScriptCollectionProxy>(Mode);
            proxy.VirtualMachineAdapter = objectBounds;
            return proxy;
        }

        VirtualMachineAdapter ProxyToScripts(IScriptCollection value)
        {
            EnsureWritable();
            return ((ScriptCollectionProxy)value).VirtualMachineAdapter;
        }

        IObjectBounds ObjectBoundsToProxy(ObjectBounds objectBounds)
        {
            EnsureReadable();
            ObjectBoundsProxy proxy = Provider.CreateProxy<ObjectBoundsProxy>(Mode);
            proxy.ObjectBounds = objectBounds;
            return proxy;
        }

        ObjectBounds ProxyToObjectBounds(IObjectBounds value)
        {
            EnsureWritable();
            return ((ObjectBoundsProxy)value).ObjectBounds;
        }

        T ReferenceToProxy<T>(uint formId) where T : IForm
        {
            EnsureReadable();
            if (formId == 0)
            {
                return default(T);
            }
            else
            {
                return Provider.CreateFormProxy<T>(formId, ProxyMode.Referenced);
            }
        }

        uint ProxyToReference(IForm proxy)
        {
            EnsureWritable();
            if (proxy == null)
            {
                return 0;
            }
            else
            {
                return proxy.FormId;
            }
        }

        IFormCollection<T> ReferenceListToProxy<T>(List<uint> formIds) where T : IForm
        {
            EnsureReadable();
            return Provider.CreateFormCollectionProxy<T>(Mode, formIds);
        }

        List<uint> ProxyToReferenceList<T>(IFormCollection<T> value) where T: IForm
        {
            EnsureWritable();
            return value.Select(f => f.FormId).ToList();
        }
    }
}
