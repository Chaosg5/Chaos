//-----------------------------------------------------------------------
// <copyright file="WatchLocation.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using Exceptions;

    /// <summary>Represents a location where a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class WatchLocation
    {
        /// <summary>Private part of the <see cref="Types"/> property.</summary>
        private List<WatchType> types;

        /// <summary>Initializes a new instance of the <see cref="WatchLocation" /> class.</summary>
        /// <param name="name">The name to set for the watch location.</param>
        public WatchLocation(string name)
        {
            this.Id = 0;
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="WatchLocation" /> class.</summary>
        /// <param name="record">The data record containing the data to create the watch location from.</param>
        public WatchLocation(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the location.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of the location.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the watch types available at this location.</summary>
        public ReadOnlyCollection<WatchType> Types
        {
            get
            {
                if (this.types != null)
                {
                    return this.types.AsReadOnly();
                }

                this.types = new List<WatchType>();
                if (this.Id > 0)
                {
                    // ToDo: Read from database
                }

                return this.types.AsReadOnly();
            }
        }
        
        /// <summary>Adds a watch type to this watch location.</summary>
        /// <param name="type">The watch type to add.</param>
        public void AddType(WatchType type)
        {
            if (type == null || type.Id == 0)
            {
                throw new ArgumentNullException("type");
            }

            if (!this.types.Exists(existingType => existingType.Id == type.Id))
            {
                this.types.Add(type);
            }
        }

        /// <summary>Removes the supplied watch type from this watch location.</summary>
        /// <param name="type">The watch type to remove.</param>
        public void RemoveType(WatchType type)
        {
            this.types.RemoveAll(existingType => existingType.Id == type.Id);
        }

        /// <summary>Saves this watch location.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Saves this watch location and connects it to the contained watch types.</summary>
        public void SaveAll()
        {
            ValidateAllSaveCandidates(this);
            SaveAllToDatabase(this);
        }

        /// <summary>Updates a watch location from a record.</summary>
        /// <param name="location">The watch location to update.</param>
        /// <param name="record">The record containing the data for the watch location.</param>
        private static void ReadFromRecord(WatchLocation location, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "WatchLocationId", "Name" });
            location.Id = (int)record["WatchLocationId"];
            location.Name = record["Name"].ToString();
        }

        /// <summary>Saves a watch location to the database.</summary>
        /// <param name="location">The watch location to save.</param>
        private static void SaveToDatabase(WatchLocation location)
        {
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("WatchLocationSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@WatchLocationId", location.Id);
                command.Parameters.AddWithValue("@Name", location.Name);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(location, reader);
                    }
                }
            }
        }

        /// <summary>Saves a watch location and connection to watch types to the database.</summary>
        /// <param name="location">The watch location to save.</param>
        private static void SaveAllToDatabase(WatchLocation location)
        {
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("WatchLocationSaveAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@WatchLocationId", location.Id);
                command.Parameters.AddWithValue("@Name", location.Name);
                var types = command.Parameters.AddWithValue("@WatchTypes", GetTypesIdDataTable(location));
                types.SqlDbType = SqlDbType.Structured;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(location, reader);
                    }
                }
            }
        }

        /// <summary>Extracts the ids of all <see cref="WatchType"/>s in the <param name="location"/> and adds them to a data table.</summary>
        /// <param name="location">The watch location to extract the <see cref="WatchType"/>'s ids from.</param>
        /// <returns>A data table with the ids of the watch types in this location.</returns>
        private static DataTable GetTypesIdDataTable(WatchLocation location)
        {
            using (var typesTable = new DataTable())
            {
                typesTable.Locale = CultureInfo.InvariantCulture;
                typesTable.Columns.Add(new DataColumn("WatchTypeId"));
                foreach (var type in location.types)
                {
                    typesTable.Rows.Add(type.Id);
                }

                return typesTable;
            }
        }

        /// <summary>Validates that the <paramref name="location"/> is valid to be saved.</summary>
        /// <param name="location">The watch location to validate.</param>
        private static void ValidateAllSaveCandidates(WatchLocation location)
        {
            ValidateSaveCandidate(location);
            if (location.types.Any(type => type.Id == 0))
            {
                throw new InvalidSaveCandidateException("The watch location can't contain unsaved watch types.");
            }
        }

        /// <summary>Validates that the <paramref name="location"/> is valid to be saved.</summary>
        /// <param name="location">The watch location to validate.</param>
        // ReSharper disable once UnusedParameter.Local
        private static void ValidateSaveCandidate(WatchLocation location)
        {
            if (string.IsNullOrEmpty(location.Name))
            {
                throw new InvalidSaveCandidateException("The name of the watch location cant be empty.");
            }
        }
    }
}
