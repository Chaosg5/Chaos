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
    using System.Linq;
    using Exceptions;

    /// <summary>Contains generic database persistence handling.</summary>
    public static class Persistent
    {
        /// <summary>The connection string to the database read from configuration application settings.</summary>
        public static readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>If database interaction should be made through the service.</summary>
        public static readonly bool UseService = ConfigurationManager.AppSettings["UseService"] == null || ConfigurationManager.AppSettings["UseSaveService"] != "false";

        /// <summary>Validates that the data record contains the specified columns.</summary>
        /// <param name="record">The data record to validate.</param>
        /// <param name="requiredColumns">The list of column names which are required.</param>
        // ReSharper disable once UnusedParameter.Local - AssertionMethod
        public static void ValidateRecord(IDataRecord record, IEnumerable<string> requiredColumns)
        {
            if (record == null)
            {
                throw new ArgumentNullException("record");
            }

            foreach (var columnName in requiredColumns.Where(columnName => record[columnName] == null))
            {
                throw new MissingColumnException(columnName);
            }
        }
    }
}
