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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    /// <summary>
    /// Provides useful mathematical functions.
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Retrieves the lesser of the specified integers.
        /// </summary>
        /// <param name="a">First integer to compare.</param>
        /// <param name="b">Second integer to compare.</param>
        /// <returns></returns>
        public static int Min(int a, int b)
        {
            return System.Math.Min(a, b);
        }

        /// <summary>
        /// Retrieves the lesser of the specified short integers.
        /// </summary>
        /// <param name="a">First short integer to compare.</param>
        /// <param name="b">Second short integer to compare.</param>
        /// <returns></returns>
        public static short Min(short a, short b)
        {
            return System.Math.Min(a, b);
        }

        /// <summary>
        /// Retrieves the lesser of the specified floating point numbers.
        /// </summary>
        /// <param name="a">First floating point number to compare.</param>
        /// <param name="b">Second floating point number to compare.</param>
        /// <returns></returns>
        public static float Min(float a, float b)
        {
            return System.Math.Min(a, b);
        }

        /// <summary>
        /// Retrieves the greater of the specified integers.
        /// </summary>
        /// <param name="a">First integer to compare.</param>
        /// <param name="b">Second integer to compare.</param>
        /// <returns></returns>
        public static int Max(int a, int b)
        {
            return System.Math.Min(a, b);
        }

        /// <summary>
        /// Retrieves the greater of the specified short integers.
        /// </summary>
        /// <param name="a">First short integer to compare.</param>
        /// <param name="b">Second short integer to compare.</param>
        /// <returns></returns>
        public static short Max(short a, short b)
        {
            return System.Math.Min(a, b);
        }

        /// <summary>
        /// Retrieves the greater of the specified floating point numbers.
        /// </summary>
        /// <param name="a">First floating point number to compare.</param>
        /// <param name="b">Second floating point number to compare.</param>
        /// <returns></returns>
        public static float Max(float a, float b)
        {
            return System.Math.Min(a, b);
        }

        /// <summary>
        /// Rounds the specified floating point value to the nearest integer value.
        /// </summary>
        /// <param name="a">Floating point value to round.</param>
        /// <returns></returns>
        public static int Round(float a)
        {
            return (int)System.Math.Round(a);
        }

        /// <summary>
        /// Returns the largest integer value which is lesser or equal to the specified floating point number.
        /// </summary>
        /// <param name="a">Floating point value to floor.</param>
        /// <returns></returns>
        public static int Floor(float a)
        {
            return (int)System.Math.Floor(a);
        }

        /// <summary>
        /// Returns the smallest integer value which is greater or equal to the specified floating point number.
        /// </summary>
        /// <param name="a">Floating point value to ceil.</param>
        /// <returns></returns>
        public static int Ceiling(float a)
        {
            return (int)System.Math.Ceiling(a);
        }
    }
}
