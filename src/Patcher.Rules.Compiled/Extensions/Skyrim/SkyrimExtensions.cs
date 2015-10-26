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
    /// <summary>
    /// Extension methods for forms and form collections.
    /// </summary>
    public static class SkyrimExtensions
    {
        /// <summary>
        /// Converts this form to a <b>Potion</b> form or returns <c>null</c> if this form is not a <b>Potion</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IAlch AsAlch(this IForm form)
        {
            return form as IAlch;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Potion</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IAlch> OfAlch(this IFormCollection<IForm> collection)
        {
            return collection.Of<IAlch>();
        }

        /// <summary>
        /// Converts this form to an <b>Ammo</b> form or returns <c>null</c> if this form is not an <b>Ammo</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IAmmo AsAmmo(this IForm form)
        {
            return form as IAmmo;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Ammo</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IAmmo> OfAmmo(this IFormCollection<IForm> collection)
        {
            return collection.Of<IAmmo>();
        }

        /// <summary>
        /// Converts this form to an <b>Armor</b> form or returns <c>null</c> if this form is not an <b>Armor</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IArmo AsArmo(this IForm form)
        {
            return form as IArmo;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Armor</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IArmo> OfArmo(this IFormCollection<IForm> collection)
        {
            return collection.Of<IArmo>();
        }

        /// <summary>
        /// Converts this form to a <b>Constructible Object</b> form or returns <c>null</c> if this form is not a <b>Constructible Object</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ICobj AsCobj(this IForm form)
        {
            return form as ICobj;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Constructible Object</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ICobj> OfCobj(this IFormCollection<IForm> collection)
        {
            return collection.Of<ICobj>();
        }

        /// <summary>
        /// Converts this form to a <b>Form List</b> form or returns <c>null</c> if this form is not a <b>Form List</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IFlst AsFlst(this IForm form)
        {
            return form as IFlst;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Form List</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IFlst> OfFlst(this IFormCollection<IForm> collection)
        {
            return collection.Of<IFlst>();
        }

        /// <summary>
        /// Converts this form to a <b>Global Variable</b> form or returns <c>null</c> if this form is not a <b>Global Variable</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IGlob AsGlob(this IForm form)
        {
            return form as IGlob;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Global Variable</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IGlob> OfGlob(this IFormCollection<IForm> collection)
        {
            return collection.Of<IGlob>();
        }

        /// <summary>
        /// Converts this form to a <b>Game Setting</b> form or returns <c>null</c> if this form is not a <b>Game Setting</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IGmst AsGmst(this IForm form)
        {
            return form as IGmst;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Game Setting</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IGmst> OfGmst(this IFormCollection<IForm> collection)
        {
            return collection.Of<IGmst>();
        }


        /// <summary>
        /// Converts this form to a <b>Keyword</b> form or returns <c>null</c> if this form is not a <b>Keyword</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IKywd AsKywd(this IForm form)
        {
            return form as IKywd;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Keyword</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IKywd> OfKywd(this IFormCollection<IForm> collection)
        {
            return collection.Of<IKywd>();
        }

        /// <summary>
        /// Converts this form to a <b>Projectile</b> form or returns <c>null</c> if this form is not a <b>Projectile</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IProj AsProj(this IForm form)
        {
            return form as IProj;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Projectile</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IProj> OfProj(this IFormCollection<IForm> collection)
        {
            return collection.Of<IProj>();
        }

        /// <summary>
        /// Converts this form to a <b>Static</b> form or returns <c>null</c> if this form is not a <b>Static</b>.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IStat AsStat(this IForm form)
        {
            return form as IStat;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Static</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IStat> OfStat(this IFormCollection<IForm> collection)
        {
            return collection.Of<IStat>();
        }
    }
}
