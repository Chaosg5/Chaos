//-----------------------------------------------------------------------
// <copyright file="Extensions.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Extension methods.</summary>
    public static class Extensions
    {
        /// <summary>A random number generator.</summary>
        private static readonly Random Rng = new Random();

        /// <summary>Creates a shuffled copy this <see cref="IList{T}"/>.</summary>
        /// <param name="list">The <see cref="IList{T}"/>.</param>
        /// <typeparam name="T">The <see langword="type"/> contained in this <see cref="IList{T}"/>.</typeparam>
        /// <returns>The shuffled <see cref="IList{T}"/>.</returns>
        public static List<T> Shuffle<T>(this IList<T> list)
        {
            var newList = list.ToList();
            var n = newList.Count;
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);
                var value = newList[k];
                newList[k] = newList[n];
                newList[n] = value;
            }

            return newList;
        }
    }
}