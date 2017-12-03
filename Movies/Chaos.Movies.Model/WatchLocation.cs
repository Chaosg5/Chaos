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
        /// <param name="record">The record containing the data for the <see cref="WatchLocation"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public WatchLocation(IDataRecord record)
        {
            this.ReadFromRecord(record);
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
                throw new ArgumentNullException(nameof(type));
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
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchLocation"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Saves this watch location and connects it to the contained watch types.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchLocation"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void SaveAll()
        {
            this.ValidateAllSaveCandidates();
            this.SaveAllToDatabase();
        }

        /// <summary>Saves this <see cref="WatchLocation"/> to the database.</summary>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("WatchLocationSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@WatchLocationId", this.Id);
                command.Parameters.AddWithValue("@Name", this.Name);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }
                }
            }
        }

        /// <summary>Saves this <see cref="WatchLocation"/> to the database.</summary>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private void SaveAllToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("WatchLocationSaveAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@WatchLocationId", this.Id);
                command.Parameters.AddWithValue("@Name", this.Name);
                var watchTypes = command.Parameters.AddWithValue("@WatchTypes", this.GetTypesIdDataTable());
                watchTypes.SqlDbType = SqlDbType.Structured;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }
                }
            }
        }

        /// <summary>Extracts the ids of all <see cref="WatchType"/>s in this <see cref="WatchLocation"/> and adds them to a data table.</summary>
        /// <returns>A data table with the ids of the watch types in this location.</returns>
        private DataTable GetTypesIdDataTable()
        {
            using (var typesTable = new DataTable())
            {
                typesTable.Locale = CultureInfo.InvariantCulture;
                typesTable.Columns.Add(new DataColumn("WatchTypeId"));
                foreach (var type in this.types)
                {
                    typesTable.Rows.Add(type.Id);
                }

                return typesTable;
            }
        }

        /// <summary>Validates that this <see cref="WatchLocation"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchLocation"/> is not valid to be saved.</exception>
        private void ValidateAllSaveCandidates()
        {
            this.ValidateSaveCandidate();
            if (this.types.Any(type => type.Id == 0))
            {
                throw new InvalidSaveCandidateException("The watch location can't contain unsaved watch types.");
            }
        }

        /// <summary>Validates that this <see cref="WatchLocation"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchLocation"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The name of the watch location cant be empty.");
            }
        }

        /// <summary>Updates this <see cref="WatchLocation"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="WatchLocation"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "WatchLocationId", "Name" });
            this.Id = (int)record["WatchLocationId"];
            this.Name = record["Name"].ToString();
        }
    }
}
