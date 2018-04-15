//-----------------------------------------------------------------------
// <copyright file="TestExtensions.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>The enumerable extension.</summary>
    public static class TestExtensions
    {
        /// <summary>The pick random.</summary>
        /// <param name="source">The source.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The item.</returns>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        /// <summary>The pick random.</summary>
        /// <param name="source">The source.</param>
        /// <param name="count">The count.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The list>.</returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        /// <summary>The shuffle.</summary>
        /// <param name="source">The source.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The list>.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}