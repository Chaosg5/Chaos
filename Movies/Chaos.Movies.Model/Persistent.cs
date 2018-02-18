//-----------------------------------------------------------------------
// <copyright file="Persistent.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Contains help methods for <see cref="Persistable{T,TDto}"/>.</summary>
    public static class Persistent
    {
        /// <summary>The connection string to the database read from configuration application settings.</summary>
        public static readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>If database interaction should be made through the service.</summary>
        public static readonly bool UseService = ConfigurationManager.AppSettings["UseService"] != null && ConfigurationManager.AppSettings["UseSaveService"] == "true";

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

        /// <summary>Creates a data table containing a single column and rows for each item in <paramref name="ids"/>.</summary>
        /// <param name="ids">The list of ids.</param>
        /// <returns>The created <see cref="DataTable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ids"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public static DataTable CreateIdCollectionTable(IEnumerable<int> ids)
        {
            var idList = ids?.Distinct().ToList();
            if (idList == null || !idList.Any())
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (idList.Any(i => i <= 0))
            {
                throw new PersistentObjectRequiredException("All items to get needs to be persisted.");
            }

            return CreateTable(idList, "Id");
        }

        /// <summary>Converts the <paramref name="columnName"/> to a variable name.</summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>The variable name.</returns>
        public static string ColumnToVariable(string columnName)
        {
            return $"@{char.ToLowerInvariant(columnName[0])}{columnName.Substring(1)}";
        }

        /// <summary>Creates a data table containing a single column and rows for each item in <paramref name="values"/>.</summary>
        /// <param name="values">The value to add to the table.</param>
        /// <param name="columnName">The name of the column for the table.</param>
        /// <typeparam name="T">The type of the column for the table.</typeparam>
        /// <returns>The created <see cref="DataTable"/>.</returns>
        private static DataTable CreateTable<T>(IEnumerable<T> values, string columnName)
        {
            using (var table = new DataTable())
            {
                table.Locale = CultureInfo.InvariantCulture;
                table.Columns.Add(new DataColumn(columnName, typeof(T)));
                foreach (var value in values)
                {
                    table.Rows.Add(value);
                }

                return table;
            }
        }
    }
}
