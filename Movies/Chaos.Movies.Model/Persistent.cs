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

    /// <summary>Contains generic database persistence handling.</summary>
    [Obsolete]
    public static class Persistent
    {
        /// <summary>The connection string to the database read from configuration application settings.</summary>
        public static readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>If database interaction should be made through the service.</summary>
        public static readonly bool UseService = ConfigurationManager.AppSettings["UseService"] == null || ConfigurationManager.AppSettings["UseSaveService"] != "false";
        
        /// <summary>Creates a data table containing a single column and rows for each item in <paramref name="ids"/>.</summary>
        /// <param name="ids">The list of ids.</param>
        /// <returns>The created <see cref="DataTable"/>.</returns>
        /// <exception cref="InvalidCastException">A value does not match it's respective column type. </exception>
        public static DataTable CreateIntCollectionTable(IEnumerable<int> ids)
        {
            return CreateTable(ids, "Item");
        }

        /// <summary>Creates a data table containing a single column and rows for each item in <paramref name="values"/>.</summary>
        /// <param name="values">The value to add to the table.</param>
        /// <param name="columnName">The name of the column for the table.</param>
        /// <typeparam name="T">The type of the column for the table.</typeparam>
        /// <returns>The created <see cref="DataTable"/>.</returns>
        /// <exception cref="InvalidCastException">A value does not match it's respective column type. </exception>
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
