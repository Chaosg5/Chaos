//-----------------------------------------------------------------------
// <copyright file="Helper.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Model.Exceptions;

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
                throw new ArgumentNullException(nameof(list));
            }

            if (newOrder == null)
            {
                throw new ArgumentNullException(nameof(newOrder));
            }

            if (newOrder.Count != list.Count)
            {
                throw new ArgumentException("The number of items to order has the be the same as the number of items in the list.");
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (!newOrder.Contains(i))
                {
                    throw new ArgumentOutOfRangeException(nameof(newOrder), string.Format(CultureInfo.InvariantCulture, "The item with the old order '{0}' was not specified in the new order.", i));
                }
            }

            var oldList = list.ToList();
            return newOrder.Select(order => oldList[order]).ToList();
        }

        /// <summary>Validates that the data record contains the specified columns.</summary>
        /// <param name="record">The data record to validate.</param>
        /// <param name="requiredColumns">The list of column names which are required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        public static void ValidateRecord(IDataRecord record, IEnumerable<string> requiredColumns)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var existingColumns = new List<string>();
            for (var i = 0; i < record.FieldCount; i++)
            {
                existingColumns.Add(record.GetName(i));
            }

            foreach (var columnName in requiredColumns.Where(columnName => !existingColumns.Contains(columnName)))
            {
                throw new MissingColumnException(columnName);
            }
        }
    }
}
