//-----------------------------------------------------------------------
// <copyright file="WatchType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Exceptions;

    /// <summary>Represents the way in which a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class WatchType
    {
        /// <summary>Initializes a new instance of the <see cref="WatchType" /> class.</summary>
        /// <param name="name">The name to set for the watch type.</param>
        public WatchType(string name)
        {
            this.Id = 0;
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="WatchType" /> class.</summary>
        /// <param name="record">The data record containing the data to create the watch type from.</param>
        public WatchType(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of this watch type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of this watch type.</summary>
        public string Name { get; private set; }

        /// <summary>Saves this watch type.</summary>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Updates this <see cref="WatchType"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="WatchType"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "Id", "Name" });
            this.Id = (int)record["Id"];
            this.Name = record["Name"].ToString();
        }

        /// <summary>Validates that this <see cref="WatchType"/> is valid to be saved.</summary>
        private void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The name of the watch type cant be empty.");
            }
        }

        /// <summary>Saves a watch type to the database.</summary>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("WatchTypeSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", this.Id);
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
    }
}
