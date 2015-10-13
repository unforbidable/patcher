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

namespace Patcher.Rules.Compiled.Extensions.Skyrim
{
    public static class SkyrimExtensions
    {
        public static IAlch AsAlch(this IForm form)
        {
            return form as IAlch;
        }

        public static IFormCollection<IAlch> OfAlch(this IFormCollection<IForm> collection)
        {
            return collection.Of<IAlch>();
        }

        public static IArmo AsArmo(this IForm form)
        {
            return form as IArmo;
        }

        public static IFormCollection<IArmo> OfArmo(this IFormCollection<IForm> collection)
        {
            return collection.Of<IArmo>();
        }

        public static IFlst AsFlst(this IForm form)
        {
            return form as IFlst;
        }

        public static IFormCollection<IFlst> OfFlst(this IFormCollection<IForm> collection)
        {
            return collection.Of<IFlst>();
        }

        public static IGlob AsGlob(this IForm form)
        {
            return form as IGlob;
        }

        public static IFormCollection<IGlob> OfGlob(this IFormCollection<IForm> collection)
        {
            return collection.Of<IGlob>();
        }

        public static IGmst AsGmst(this IForm form)
        {
            return form as IGmst;
        }

        public static IFormCollection<IGmst> OfGmst(this IFormCollection<IForm> collection)
        {
            return collection.Of<IGmst>();
        }

        public static IKywd AsKywd(this IForm form)
        {
            return form as IKywd;
        }

        public static IFormCollection<IKywd> OfKywd(this IFormCollection<IForm> collection)
        {
            return collection.Of<IKywd>();
        }

        public static IStat AsStat(this IForm form)
        {
            return form as IStat;
        }

        public static IFormCollection<IStat> OfStat(this IFormCollection<IForm> collection)
        {
            return collection.Of<IStat>();
        }
    }
}
