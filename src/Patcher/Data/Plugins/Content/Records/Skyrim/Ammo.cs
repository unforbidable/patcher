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

using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.AMMO)]
    [Game(Games.Skyrim)]
    public sealed class Ammo : GenericFormRecord, IFeaturingObjectBounds
    {
        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.MODL, Names.MODT, Names.MODS)]
        [Initialize]
        private Model ModelData { get; set; }

        [Member(Names.ICON)]
        public string InventoryImage { get; set; }

        [Member(Names.MICO)]
        public string MessageIcon { get; set; }

        [Member(Names.DEST, Names.DSTD, Names.DSTF)]
        private DestructionData Destruction { get; set; }

        [Member(Names.YNAM)]
        [Reference(Names.SNDR)]
        public uint PickUpSound { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.SNDR)]
        public uint PutDownSound { get; set; }

        [Member(Names.DESC)]
        [LocalizedString(LocalizedStringGroups.DLStrings)]
        public string Description { get; set; }

        [Member(Names.KSIZ, Names.KWDA)]
        [Initialize]
        public Keywords Keywords { get; set; }

        [Member(Names.DATA)]
        private AmmoData Data { get; set; }

        [Member(Names.ONAM)]
        public string ShortName { get; set; }
        // ShortName (ONAM) is a zero-terminate string contrary to UESP.NET

        public string WorldModel { get { return ModelData.Path; } set { ModelData.Path = value; } }
        public uint Projectile { get { return Data.Projectile; } set { Data.Projectile = value; } }
        public float Damage { get { return Data.Damage; } set { Data.Damage = value; } }
        public int Value { get { return Data.Value; } set { Data.Value = value; } }

        public bool IsPlayable { get { return !Data.Flags.HasFlag(AmmoFlags.NonPlayable); } set { SetPlayable(value); } }
        public bool IsBolt { get { return !Data.Flags.HasFlag(AmmoFlags.NonBolt); } set { SetBolt(value); } }
        public bool IgnoresNormalWeaponResistance { get { return Data.Flags.HasFlag(AmmoFlags.IgnoresNormalWeaponResistance); } set { SetIgnoresNormalWeaponResistance(value); } }

        private void SetPlayable(bool set)
        {
            if (set)
                Data.Flags &= ~AmmoFlags.NonPlayable;
            else
                Data.Flags |= AmmoFlags.NonPlayable;
        }

        private void SetBolt(bool set)
        {
            if (set)
                Data.Flags &= ~AmmoFlags.NonBolt;
            else
                Data.Flags |= AmmoFlags.NonBolt;
        }

        private void SetIgnoresNormalWeaponResistance(bool set)
        {
            if (set)
                Data.Flags |= AmmoFlags.IgnoresNormalWeaponResistance;
            else
                Data.Flags &= ~AmmoFlags.IgnoresNormalWeaponResistance;
        }

        [Flags]
        enum AmmoFlags : uint
        {
            IgnoresNormalWeaponResistance = 0x01,
            NonPlayable = 0x02,
            NonBolt = 0x04,
        }

        class AmmoData : Field
        {
            public uint Projectile { get; set; }
            public AmmoFlags Flags { get; set; }
            public float Damage { get; set; }
            public int Value { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Projectile = reader.ReadReference(FormKindSet.ProjOnly);
                Flags = (AmmoFlags)reader.ReadUInt32();
                Damage = reader.ReadSingle();
                Value = reader.ReadInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Projectile, FormKindSet.ProjOnly);
                writer.Write((uint)Flags);
                writer.Write(Damage);
                writer.Write(Value);
            }

            public override Field CopyField()
            {
                return new AmmoData()
                {
                    Projectile = Projectile,
                    Flags = Flags,
                    Damage = Damage,
                    Value = Value
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (AmmoData)other;
                return Projectile == cast.Projectile && Flags == cast.Flags && Damage == cast.Damage && Value == cast.Value;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Projectile;
            }

            public override string ToString()
            {
                return string.Format("PROJ {0:X8}", Projectile);
            }
        }
    }
}
