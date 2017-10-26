//-----------------------------------------------------------------------
// <copyright file="Persistent.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Contains generic database persistence handling.</summary>
    public static class Persistent
    {
        /// <summary>The connection string to the database read from configuration application settings.</summary>
        public static readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>If database interaction should be made through the service.</summary>
        public static readonly bool UseService = ConfigurationManager.AppSettings["UseService"] != null && ConfigurationManager.AppSettings["UseSaveService"] == "true";

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
        public static DataTable CreateIntCollectionTable(IEnumerable<int> ids)
        {
            return CreateTable(ids, "Item");
        }

        /// <summary>Creates a data table containing a single column and rows for each item in <paramref name="values"/>.</summary>
        /// <param name="values">The value to add to the table.</param>
        /// <param name="columnName">The name of the column for the table.</param>
        /// <typeparam name="T">The type of the column for the table.</typeparam>
        /// <returns>The created <see cref="DataTable"/>.</returns>
        public static DataTable CreateTable<T>(IEnumerable<T> values, string columnName)
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
