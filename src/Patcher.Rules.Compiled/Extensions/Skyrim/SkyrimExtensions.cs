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
        /// Converts this form to an <b>Ammo</b> form or returns <c>null</c> if this form is not an <b>Ammo</b> form.
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
        /// Converts this form to an <b>Armor</b> form or returns <c>null</c> if this form is not an <b>Armor</b> form.
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
        /// Converts this form to a <b>Constructible Object</b> form or returns <c>null</c> if this form is not a <b>Constructible Object</b> form.
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
        /// Converts this form to a <b>Collision Layer</b> form or returns <c>null</c> if this form is not a <b>Collision Layer</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IColl AsColl(this IForm form)
        {
            return form as IColl;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Collision Layer</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IColl> OfColl(this IFormCollection<IForm> collection)
        {
            return collection.Of<IColl>();
        }

        /// <summary>
        /// Converts this form to an <b>Enchantment</b> form or returns <c>null</c> if this form is not an <b>Enchantment</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IEnch AsEnch(this IForm form)
        {
            return form as IEnch;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Enchantment</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IEnch> OfEnch(this IFormCollection<IForm> collection)
        {
            return collection.Of<IEnch>();
        }

        /// <summary>
        /// Converts this form to an <b>Equipment Type</b> form or returns <c>null</c> if this form is not an <b>Equipment Type</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IEqup AsEqup(this IForm form)
        {
            return form as IEqup;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Equipment Type</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IEqup> OfEqup(this IFormCollection<IForm> collection)
        {
            return collection.Of<IEqup>();
        }

        /// <summary>
        /// Converts this form to an <b>Explosion</b> form or returns <c>null</c> if this form is not an <b>Explosion</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IExpl AsExpl(this IForm form)
        {
            return form as IExpl;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Explosion</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IExpl> OfExpl(this IFormCollection<IForm> collection)
        {
            return collection.Of<IExpl>();
        }

        /// <summary>
        /// Converts this form to a <b>Form List</b> form or returns <c>null</c> if this form is not a <b>Form List</b> form.
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
        /// Converts this form to a <b>Global Variable</b> form or returns <c>null</c> if this form is not a <b>Global Variable</b> form.
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
        /// Converts this form to a <b>Game Setting</b> form or returns <c>null</c> if this form is not a <b>Game Setting</b> form.
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
        /// Converts this form to a <b>Impact Data Set</b> form or returns <c>null</c> if this form is not an <b>Impact Data Set</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IIpds AsIpds(this IForm form)
        {
            return form as IIpds;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Impact Data Set</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IIpds> OfIpds(this IFormCollection<IForm> collection)
        {
            return collection.Of<IIpds>();
        }

        /// <summary>
        /// Converts this form to a <b>Keyword</b> form or returns <c>null</c> if this form is not a <b>Keyword</b> form.
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
        /// Converts this form to a <b>Light</b> form or returns <c>null</c> if this form is not a <b>Light</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ILigh AsLigh(this IForm form)
        {
            return form as ILigh;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Light</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ILigh> OfLigh(this IFormCollection<IForm> collection)
        {
            return collection.Of<ILigh>();
        }

        /// <summary>
        /// Converts this form to a <b>Projectile</b> form or returns <c>null</c> if this form is not a <b>Projectile</b> form.
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
        /// Converts this form to a <b>Material</b> form or returns <c>null</c> if this form is not a <b>Material</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IMatt AsMatt(this IForm form)
        {
            return form as IMatt;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Material</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IMatt> OfMatt(this IFormCollection<IForm> collection)
        {
            return collection.Of<IMatt>();
        }

        /// <summary>
        /// Converts this form to a <b>Magic Effect</b> form or returns <c>null</c> if this form is not a <b>Magic Effect</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IMgef AsMgef(this IForm form)
        {
            return form as IMgef;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Magic Effect</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IMgef> OfMgef(this IFormCollection<IForm> collection)
        {
            return collection.Of<IMgef>();
        }

        /// <summary>
        /// Converts this form to a <b>Race</b> form or returns <c>null</c> if this form is not a <b>Race</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IRace AsRace(this IForm form)
        {
            return form as IRace;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Race</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IRace> OfRace(this IFormCollection<IForm> collection)
        {
            return collection.Of<IRace>();
        }

        /// <summary>
        /// Converts this form to a <b>Spell</b> form or returns <c>null</c> if this form is not a <b>Spell</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ISpel AsSpel(this IForm form)
        {
            return form as ISpel;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Spell</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ISpel> OfSpel(this IFormCollection<IForm> collection)
        {
            return collection.Of<ISpel>();
        }

        /// <summary>
        /// Converts this form to a <b>Static</b> form or returns <c>null</c> if this form is not a <b>Static</b> form.
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

        /// <summary>
        /// Converts this form to a <b>Sound</b> form or returns <c>null</c> if this form is not a <b>Sound</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ISndr AsSndr(this IForm form)
        {
            return form as ISndr;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Sound</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ISndr> OfSndr(this IFormCollection<IForm> collection)
        {
            return collection.Of<ISndr>();
        }

        /// <summary>
        /// Converts this form to a <b>Texture Set</b> form or returns <c>null</c> if this form is not a <b>Texture Set</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ITxst AsTxst(this IForm form)
        {
            return form as ITxst;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Texture Set</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ITxst> OfTxst(this IFormCollection<IForm> collection)
        {
            return collection.Of<ITxst>();
        }

        /// <summary>
        /// Converts this form to a <b>Weapon</b> form or returns <c>null</c> if this form is not a <b>Weapon</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IWeap AsWeap(this IForm form)
        {
            return form as IWeap;
        }

        /// <summary>
        /// Convers and fiters this mixed form collection to a collection of <b>Weapon</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IWeap> OfWeap(this IFormCollection<IForm> collection)
        {
            return collection.Of<IWeap>();
        }
    }
}
