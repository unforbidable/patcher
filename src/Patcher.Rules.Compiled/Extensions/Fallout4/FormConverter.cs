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
using Patcher.Rules.Compiled.Forms.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Extensions.Fallout4
{
    /// <summary>
    /// Extension methods for forms and form collections.
    /// </summary>
    public static class FormConverter
    {
        private static T As<T>(IForm form) where T : class, IForm
        {
            if (form == null)
                return null;
            else
                return form.As<T>();
        }

        private static IFormCollection<T> Of<T>(IFormCollection<IForm> collection) where T : IForm
        {
            return collection.Of<T>();
        }


        /// <summary>
        /// Converts this form to a <b>Activation Rule</b> form or returns <c>null</c> if this form is not a <b>Activation Rule</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IAoru AsAoru(this IForm form)
        {
            return As<IAoru>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Activation Rule</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IAoru> OfAoru(this IFormCollection<IForm> collection)
        {
            return Of<IAoru>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Form List</b> form or returns <c>null</c> if this form is not a <b>Form List</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IFlst AsFlst(this IForm form)
        {
            return As<IFlst>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Form List</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IFlst> OfFlst(this IFormCollection<IForm> collection)
        {
            return Of<IFlst>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>God Reys</b> form or returns <c>null</c> if this form is not a <b>God Reys</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IGdry AsGdry(this IForm form)
        {
            return As<IGdry>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>God Reys</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IGdry> OfGdry(this IFormCollection<IForm> collection)
        {
            return Of<IGdry>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Global Variable</b> form or returns <c>null</c> if this form is not a <b>Global Variable</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IGlob AsGlob(this IForm form)
        {
            return As<IGlob>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Global Variable</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IGlob> OfGlob(this IFormCollection<IForm> collection)
        {
            return Of<IGlob>(collection);
        }


        /// <summary>
        /// Converts this form to a <b>Game Setting</b> form or returns <c>null</c> if this form is not a <b>Game Setting</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IGmst AsGmst(this IForm form)
        {
            return As<IGmst>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Game Setting</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IGmst> OfGmst(this IFormCollection<IForm> collection)
        {
            return Of<IGmst>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Image Space</b> form or returns <c>null</c> if this form is not an <b>Image Space</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IImgs AsImgs(this IForm form)
        {
            return As<IImgs>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Image Space</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IImgs> OfImgs(this IFormCollection<IForm> collection)
        {
            return Of<IImgs>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Keyword</b> form or returns <c>null</c> if this form is not a <b>Keyword</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IKywd AsKywd(this IForm form)
        {
            return As<IKywd>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Keyword</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IKywd> OfKywd(this IFormCollection<IForm> collection)
        {
            return Of<IKywd>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Visual Effect</b> form or returns <c>null</c> if this form is not a <b>Visual Effect</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IRfct AsRfct(this IForm form)
        {
            return As<IRfct>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Visual Effect</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IRfct> OfRfct(this IFormCollection<IForm> collection)
        {
            return Of<IRfct>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Sound</b> form or returns <c>null</c> if this form is not a <b>Sound</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ISndr AsSndr(this IForm form)
        {
            return As<ISndr>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Sound</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ISndr> OfSndr(this IFormCollection<IForm> collection)
        {
            return Of<ISndr>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Shader Particle Geometry</b> form or returns <c>null</c> if this form is not a <b>Shader Particle Geometry</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ISpgd AsSpgd(this IForm form)
        {
            return As<ISpgd>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Shader Particle Geometry</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<ISpgd> OfSpgd(this IFormCollection<IForm> collection)
        {
            return Of<ISpgd>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Static</b> form or returns <c>null</c> if this form is not a <b>Static</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IStat AsStat(this IForm form)
        {
            return As<IStat>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Static</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IStat> OfStat(this IFormCollection<IForm> collection)
        {
            return Of<IStat>(collection);
        }

        /// <summary>
        /// Converts this form to a <b>Weather</b> form or returns <c>null</c> if this form is not a <b>Weather</b> form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IWthr AsWthr(this IForm form)
        {
            return As<IWthr>(form);
        }

        /// <summary>
        /// Convers and filters this mixed form collection to a collection of <b>Weather</b> forms.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFormCollection<IWthr> OfWthr(this IFormCollection<IForm> collection)
        {
            return Of<IWthr>(collection);
        }

    }
}
