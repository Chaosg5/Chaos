//-----------------------------------------------------------------------
// <copyright file="Helper.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>Generic helper class.</summary>
    public static class Helper
    {
        /// <summary>Reorders a dictionary with an order based key.</summary>
        /// <typeparam name="T">The type of the items in the <paramref name="list"/>.</typeparam>
        /// <param name="list">The list to change to order.</param>
        /// <param name="newOrder">The order to set based on the indexes of the old order.</param>
        /// <returns>The reordered list.</returns>
        /// <exception cref="ArgumentNullException">If either parameter is null.</exception>
        /// <exception cref="ArgumentException">If the new order does not match the dictionary.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If an item with an key in the order was not found in the dictionary.</exception>
        public static ICollection<T> ReorderList<T>(ICollection<T> list, ICollection<int> newOrder)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (newOrder == null)
            {
                throw new ArgumentNullException("newOrder");
            }

            if (newOrder.Count != list.Count)
            {
                throw new ArgumentException("The number of items to order has the be the same as the number of items in the list.");
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (!newOrder.Contains(i))
                {
                    throw new ArgumentOutOfRangeException("newOrder", string.Format(CultureInfo.InvariantCulture, "The item with the old order '{0}' was not specified in the new order.", i));
                }
            }

            var oldList = list.ToList();
            return newOrder.Select(order => oldList[order]).ToList();
        }
    }
}
